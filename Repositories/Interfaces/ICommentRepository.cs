using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Models.RequestModelsDTO;

namespace RealEstateApi.Repositories.Interfaces
{

    public interface ICommentRepository
    {
        public Task<IEnumerable<CommentDTO>> GetAllAsync(Guid IssueId);
        public Task<CommentDTO> GetByIdAsync(Guid id);
        public Task AddAsync(RequestCommentDTO entity);
        public Task UpdateAsync(Guid id, RequestCommentDTO entity);
        public Task DeleteAsync(CommentDTO commentDTO);

    }

}
