using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstateApi.Models;
using RealEstateApi.Repositories;
using RealEstateApi.Repository;
using System.Threading.Tasks;
using System;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Repositories.Interfaces;
using RealEstateApi.Models.RequestModelsDTO;
using RealEstateApi.Models.Interfaces;
using RealEstateApi.Models.ResponseModelsDTO;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.AspNetCore.Authorization;

namespace RealEstateApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ILogger<CommentRepository> _logger;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        public readonly IMailTemplate _mailTemplate;
        public readonly INotifier _notifier;
        public readonly IIssueRepository _issueRepository;


        public CommentController(ILogger<CommentRepository> logger, 
                                ICommentRepository commentRepository,
                                IUserRepository userRepository,
                                IMailTemplate mailTemplate,
                                INotifier notifier,
                                IIssueRepository issueRepository)
        {
            _commentRepository = commentRepository;
            _logger = logger;
            _userRepository = userRepository;
            _mailTemplate = mailTemplate;
            _notifier = notifier;
            _issueRepository = issueRepository;
        }

        [HttpGet("Issue/{issueId}")]
        public async Task<IActionResult> GetCommentsByIssue(Guid issueId)
        {
            try
            {
                var comments = await _commentRepository.GetAllAsync(issueId);
                if (comments == null)
                {
                    return NotFound($"No comments found for issue with id: {issueId}");
                }
                return Ok(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving comments for issue with id: {issueId}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(Guid id)
        {
            try
            {
                var comment = await _commentRepository.GetByIdAsync(id);
                if (comment == null)
                {
                    return NotFound($"No comment found with id: {id}");
                }
                return Ok(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving comment with id: {id}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] RequestCommentDTO commentDTO)
        {
            try
            {
                if (!Guid.TryParse(commentDTO.IssueId, out Guid issueId))
                {
                    return BadRequest("Invalid Issue ID.");
                }


                var user = await _userRepository.GetByID(commentDTO.UserId);
                var issue = await _issueRepository.GetIssueById(issueId);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }
                if (issue == null)
                {
                    return BadRequest("Issue not found.");
                }

                var issueOwner = await _userRepository.GetByID(issue.CreatedByUserId);
                var houseOwner = await _userRepository.GetByID(issue.UserHouseId);

                ResponseUserDTO sendTo = user == issueOwner? houseOwner : issueOwner;

                await _commentRepository.AddAsync(commentDTO);
                var response = new
                {
                    message = $"comment created correctly",
                    comment = commentDTO
                };


                var emailTemplate = _mailTemplate.GetMailTemplateForNewComment(user.Name, sendTo.Name, issue.Id.ToString());
                _notifier.SendNotification(sendTo.Email, "Commento aggiunto", emailTemplate);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment");
                return StatusCode(500, $"Internal server error {ex}");
            }
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid id, [FromBody] RequestCommentDTO commentDTO)
        {
            try
            {
                await _commentRepository.UpdateAsync(id, commentDTO);
                var response = new
                {
                    message = $"comment update correctly",
                    comment = commentDTO

                };
                return Ok(response);
            }
            catch (InvalidOperationException ioe)
            {
                return Unauthorized(ioe.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating comment with id: {id}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id, [FromQuery] string userId)
        {
            try
            {
                CommentDTO commentDTO = new CommentDTO { Id = id, UserId = userId };
                await _commentRepository.DeleteAsync(commentDTO);
                var response = new
                {
                    message = $"comment {id} deleted correctly"

                };
                return Ok(response);
            }
            catch (InvalidOperationException ioe)
            {
                return Unauthorized(ioe.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting comment with id: {id}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


    }
}
