using System.Collections.Generic;
using System.Threading.Tasks;
using Eaship.Models;

namespace Eaship.Services
{
    public interface IContractService
    {
        Task<List<Contract>> GetPendingAsync();
    }
}
