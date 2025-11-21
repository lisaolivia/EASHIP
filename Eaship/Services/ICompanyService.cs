using Eaship.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eaship.Services
{
    public interface ICompanyService
    {
        Task<RenterCompany> CreateAsync(RenterCompany company);
        Task<List<RenterCompany>> GetPendingAsync();
        Task<List<RenterCompany>> GetActiveAsync();
        Task<RenterCompany?> GetByIdAsync(int id);
        Task ApproveAsync(int id, User admin);
        Task RejectAsync(int id, User admin);
    }
}
