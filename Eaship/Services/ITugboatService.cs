using Eaship.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eaship.Services
{
    public interface ITugboatService
    {
        Task<List<Tugboat>> GetAllAsync();
        Task<Tugboat?> GetByIdAsync(long id);
        Task AddAsync(Tugboat tugboat);
        Task UpdateAsync(Tugboat tugboat);
    }
}
