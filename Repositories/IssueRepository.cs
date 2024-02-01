using RealEstateApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.RealEstateDbContext;
using RealEstateApi.Repositories.Interfaces;
using RealEstateApi.Models.RequestModelsDTO;
using RealEstateApi.Models.ResponseModelsDTO;
using RealEstateApi.Enumeratori;

namespace RealEstateApi.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly AirbnbDataContext _context;
        private readonly ILogger<IssueRepository> _logger;

        public IssueRepository(AirbnbDataContext context, ILogger<IssueRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseIssueDTO> GetIssueById(Guid issueId)
        {
            try
            {
                var issue = await _context.Issues.Include(i => i.Comments)
                           .FirstOrDefaultAsync(issue => issue.Id == issueId);

                //verifica se l'issue è stato trovato
                if (issue == null)
                {
                    _logger.LogWarning("No issue was found for ID: {@IssueId}", issueId);
                    return null;
                }

                return new ResponseIssueDTO(issue);

            } catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while trying to retrieve the issue");
                throw;
            }
        }

        public async Task<IEnumerable<ResponseIssueDTO>> GetIssuesForHouse(string houseId)
        {
            try
            {
                //filtra gli issue per una specifica casa
                var issues = await _context.Issues.Include(i => i.Comments).
                    Where(issue => issue.HouseId == houseId).ToListAsync();
                if (!issues.Any())
                {
                    _logger.LogInformation($"No issues were found for the house {houseId}");
                    return null;
                }

                return issues.Select(i => new ResponseIssueDTO(i));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while trying to retrieve the issues");
                throw;
            }
        }

        public async Task<IssueDTO> CreateIssue(RequestIssueDTO newIssueDTO)
        {
            Issue issue = new Issue()
            {
                Id = Guid.NewGuid(),
                HouseId = newIssueDTO.HouseId,
                UserHouseId = newIssueDTO.UserHouseId,
                Description = newIssueDTO.Description,
                State = newIssueDTO.State,
                Date = DateTime.Now,
                CreatedByUserId = newIssueDTO.CreatedByUserId
            };

            try
            {
                 _context.Issues.Add(issue);
                //aggiunge l'issue al database
                await _context.SaveChangesAsync();

                _logger.LogInformation("New issue created: {@Issue}", newIssueDTO);

                //lo salva
                return new IssueDTO(issue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during the creation of a new issue");
                throw; //rilancia l'eccezione
            }
        }

        public async Task<bool> UpdateIssueStatus(Guid id, IssueStatus newStatus)
        {
            try
            {
                var issue = await _context.Issues.FirstOrDefaultAsync(i => i.Id == id);
                _context.Entry(issue).CurrentValues.SetValues(new { State = newStatus });
                var changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to update the issue status");
                throw;
            }
        }

        public async Task<bool> IssueExists(Guid issueId)
        {
            try
            {
                return await _context.Issues.AnyAsync(i => i.Id == issueId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "An Error occurred while tryng to retrieve an issue");
                throw;
            }
        }
    }
}

