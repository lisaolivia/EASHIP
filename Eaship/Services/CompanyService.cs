using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eaship.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly EashipDbContext _db;

        public CompanyService(EashipDbContext db)
        {
            _db = db;
        }

        public async Task<RenterCompany> CreateAsync(RenterCompany company)
        {
            _db.RenterCompanies.Add(company);
            await _db.SaveChangesAsync();
            return company;
        }

        public async Task<List<RenterCompany>> GetApprovedAsync()
        {
            return await _db.RenterCompanies
                .Where(c => c.Status == CompanyStatus.Active)
                .ToListAsync();

        }


        public async Task<List<RenterCompany>> GetPendingAsync()
        {
            return await _db.RenterCompanies
                .Where(c => c.Status == CompanyStatus.Validating)
                .ToListAsync();
        }

        public async Task<List<RenterCompany>> GetActiveAsync()
        {
            return await _db.RenterCompanies
                .Where(c => c.Status == CompanyStatus.Active)
                .ToListAsync();
        }

        public async Task<RenterCompany?> GetByIdAsync(int id)
        {
            return await _db.RenterCompanies.FindAsync(id);
        }

        public async Task ApproveAsync(int id, User admin)
        {
            var company = await GetByIdAsync(id);

            if (company == null) return;

            company.Status = CompanyStatus.Active;
            company.ApprovedAt = DateTime.UtcNow;
            company.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        public async Task RejectAsync(int id, User admin, string reason)
        {
            var company = await GetByIdAsync(id);
            if (company == null) return;

            company.Status = CompanyStatus.Rejected;
            company.RejectedReason = reason;
            company.RejectedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }


    }
}
