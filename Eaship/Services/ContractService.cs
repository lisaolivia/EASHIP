using System.Collections.Generic;
using System.Threading.Tasks;
using Eaship.Models;

namespace Eaship.Services
{
    public class ContractService : IContractService
    {
        public Task<List<Contract>> GetPendingAsync()
        {
            return Task.FromResult(new List<Contract>());
        }
    }
}
