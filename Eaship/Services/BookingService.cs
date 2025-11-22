using System.Collections.Generic;
using System.Threading.Tasks;
using Eaship.Models;

namespace Eaship.Services
{
    public class BookingService : IBookingService
    {
        public Task<List<Booking>> GetPendingAsync()
        {
            return Task.FromResult(new List<Booking>());
        }
    }
}
