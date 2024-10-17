using Data.Model;
using Share.DTOs;

namespace API.Services.Interfaces
{
    public interface IV008EntityService
    {
        IEnumerable<DictionaryDTO> GetAll();
        Task<DictionaryDTO?> GetByIdAsync(int id);
        Task AddAsync(V008Entity entity);
        Task UpdateAsync(V008Entity entity);
        Task DeleteAsync(int id);
        Task UploadFromFileAsync(IFormFile file);
    }
}
