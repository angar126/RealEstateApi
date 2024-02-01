using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstateApi.Models;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Repository;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using RealEstateApi.Repositories.Interfaces;
using RealEstateApi.RealEstateDbContext;
using RealEstateApi.Models.RequestModelsDTO;
using Serilog.Core;

namespace RealEstateApi.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        protected readonly AirbnbDataContext _context;
        private readonly ILogger<CommentRepository> _logger;
        public CommentRepository(AirbnbDataContext context, ILogger<CommentRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<CommentDTO>> GetAllAsync(Guid issueId)
        {
            try
            {
                var comments = await _context.Comments
                                    .Where(c => c.IssueId == issueId)
                                    .ToListAsync();

                if (!comments.Any())
                {
                    _logger.LogInformation($"No comments found for issue with id: {issueId}");
                    return new List<CommentDTO>();
                }

                return comments.Select(c => new CommentDTO(c));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving comments for issue with id: {issueId}");
                throw;
            }
        }




        //TODO pensare se va bene cosi o dobbiamo cambiare qualcosa
        public async Task<CommentDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
                if (comment == null)
                {
                    _logger.LogInformation($"No Comment with id: {id}");
                    return null;
                }

                return new CommentDTO(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error while trying to retrieve data from Comment table");
                throw;
            }
        }

        public async Task AddAsync(RequestCommentDTO commentDTO)
        {
            try
            {
                var issue = await _context.Issues.FirstOrDefaultAsync(i => i.Id == Guid.Parse(commentDTO.IssueId));

                if (issue == null)
                {
                    throw new ArgumentException("Issue with provided ID does not exist.");
                }

                Comment comment = new Comment
                {
                    Id = Guid.NewGuid(),
                    CommentDescription = commentDTO.CommentDescription,
                    IssueId = Guid.Parse(commentDTO.IssueId),
                    UserId = commentDTO.UserId
                };

                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new comment");
                throw;
            }
        }


        public async Task UpdateAsync(Guid id, RequestCommentDTO commentDTO)
        {
            try
            {
                var existingComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
                if (existingComment == null)
                {
                    _logger.LogInformation($"No Comment with id: {id}");
                    return;
                }


                if (existingComment.UserId != commentDTO.UserId)
                {
                    _logger.LogError($"Unauthorized attempt to update comment with id {id} by user {commentDTO.UserId}");
                    throw new InvalidOperationException("You are not authorized to update this comment.");
                }

                if (existingComment.IssueId != Guid.Parse(commentDTO.IssueId))
                {
                    _logger.LogError($"Attempt to update comment with id {id} for wrong issue {commentDTO.IssueId}");
                    throw new InvalidOperationException("Issue ID mismatch. Cannot update comment.");
                }


                existingComment.CommentDescription = commentDTO.CommentDescription;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating comment with id {id}");
                throw;
            }
        }



        public async Task DeleteAsync(CommentDTO commentDTO)
        {
            try
            {
                var existingComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentDTO.Id);
                if (existingComment == null)
                {
                    _logger.LogInformation($"No Comment found with id: {commentDTO.Id}");
                    return;
                }


                if (existingComment.UserId != commentDTO.UserId)
                {
                    _logger.LogError($"Unauthorized attempt to delete comment with id {commentDTO.Id} by user {commentDTO.UserId}");
                    throw new InvalidOperationException("You are not authorized to delete this comment.");
                }

                _context.Comments.Remove(existingComment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting comment with id {commentDTO.Id}");
                throw;
            }
        }



    }
}
