using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eaship.Services
{
    public class TugboatService : ITugboatService
    {
        private readonly EashipDbContext _context;

        public TugboatService(EashipDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tugboat>> GetAllAsync()
        {
            return await _context.Tugboats.ToListAsync();
        }

        public async Task<Tugboat?> GetByIdAsync(long id)
        {
            return await _context.Tugboats.FirstOrDefaultAsync(x => x.TugboatId == id);
        }

        public async Task AddAsync(Tugboat tugboat)
        {
            _context.Tugboats.Add(tugboat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tugboat tugboat)
        {
            _context.Tugboats.Update(tugboat);
            await _context.SaveChangesAsync();
        }
    }
}
