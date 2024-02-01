using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Models;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Repositories.Interfaces;
using RealEstateApi.Models.RequestModelsDTO;
using Microsoft.AspNetCore.Authorization;
using RealEstateApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using RealEstateApi.RealEstateDbContext;
using RealEstateApi.Enumeratori;
using Microsoft.AspNetCore.Authorization;

namespace RealEstateApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IssueController : Controller    
    {
        private readonly IIssueRepository _issueRepository;
        private readonly ILogger<IssueController> _logger;
        private readonly IMailTemplate _mailTemplate;
        private readonly INotifier _notifier;
        private readonly IHouseRepository _houseRepository;
        private readonly IUserRepository _userRepository;

        public IssueController(IMailTemplate mailTemplate, INotifier notifier, IIssueRepository issueRepository,
            ILogger<IssueController> logger, IHouseRepository houseRepository, IUserRepository userRepository)
        {
            _mailTemplate = mailTemplate;
            _notifier = notifier;
            _issueRepository = issueRepository;
            _logger = logger;
            _houseRepository = houseRepository;
            _userRepository = userRepository;
        }

        [HttpGet("GetIssueById/{issueId}")]
        public async Task<IActionResult> GetIssueById(string issueId)
        {
            try
            {
                _logger.LogInformation($"Start GetIssueById for ID: {issueId}"); //log informativo

                //converte l'ID della stringa in un oggetto Guid
                if (!Guid.TryParse(issueId, out Guid guidIssueId))
                {
                    //se l'ID non è valido, restituisce un errore BadRequest
                    return BadRequest("Invalid Issue ID format");
                }

                //metodo della repository per ottenere l'issue per ID
                var issue = await _issueRepository.GetIssueById(guidIssueId);

                //verifica
                if (issue == null)
                {
                    return NotFound("Issue not found");
                }

                return Ok(issue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to retrieve the issue by ID");
                return BadRequest(ex.Message);
            }
        }

        //metodo HTTP POST che segnala il nuovo issue
         [HttpPost]
        public async Task<IActionResult> ReportIssue([FromBody] RequestIssueDTO issueDTO)
        {
            try
            {
                var house = await _houseRepository.GetByIdAsync(Guid.Parse(issueDTO.HouseId));

                if (issueDTO == null)
                {
                    _logger.LogInformation($"Issue creation failed,insufficient data");
                    return null;
                }

                //richiamo il metodo della repository per creare un nuovo issue
                var createdIssue = await _issueRepository.CreateIssue(issueDTO);

                if(createdIssue == null )
                {
                    _logger.LogInformation($"Issue creation failed");
                    return Problem();
                }

                _logger.LogInformation($"Issue created successfully.");
                //ritorna una risposta di successo della segnalazione

                if (!SendMail(house).Result)
                {
                    _logger.LogError($"Email not sent");
                }

                return Ok("Issue reported successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error in ReportIssue");
                return BadRequest(ex.Message);
            }
        }

        private async Task<bool> SendMail(HouseDTO house)
        {
            var userOwner = await _userRepository.GetByID(house.UserId);
            var userTenant = await _userRepository.GetByID(house.UserTenantId);

            if (userOwner == null || userTenant == null)
            {
                return false;
            }
            var ownerUserName = userOwner.Name;
            var issuerUserName = userTenant.Name;

            var issueTemplate = _mailTemplate.GetMailTemplateForNewIssue(ownerUserName, issuerUserName);

            _notifier.SendNotification(userOwner.Email, "New issue reported", issueTemplate);
            return true;
        }

        //metodo HTTP GET per ottenere gli issue di una specifica casa
        [HttpGet("{houseId}")]
        public async Task<IActionResult> GetIssuesForHouse(string houseId)
        {

            try
            {
                _logger.LogInformation("Start GetIssuesForHouse"); // Log informativo

                //ottiene gli issue per la casa specificata utilizzando la repository
                var issues = await _issueRepository.GetIssuesForHouse(houseId);

                //verifica se non sono stati trovati issue per la casa specificata
                if (issues == null || !issues.Any())
                {
                    //risposta NotFound se non ci sono issue per la casa
                    return NotFound("No issues were found for the house");
                }

                //risposta OK con la lista degli issue trovati
                return Ok(issues);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to retrieve the issue");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{issueId}/ChangeStatus/{newStatus}")]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> ChangeIssueStatus(Guid issueId, IssueStatus newStatus)
        {
            try
            {
                //ottiene l'issue dal repo
                var issueExists = await _issueRepository.IssueExists(issueId);

                if (!issueExists)
                    return NotFound("Issue not found");
                

                //qui verifico se lo stato è uno di quelli validi
                if (!Enum.IsDefined(typeof(IssueStatus), newStatus))
                {
                    return BadRequest("Invalid issue status");
                }
                //salvo le modifiche nello storage (nel database o repository)
                var updatedIssue = await _issueRepository.UpdateIssueStatus(issueId,newStatus);

                if(!updatedIssue)
                {
                    return StatusCode(500, $"Unable to update the issue"); ;
                }

                return Ok("Issue status updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to update issue status");
                return BadRequest(ex.Message);
            }
        }
    }
}


