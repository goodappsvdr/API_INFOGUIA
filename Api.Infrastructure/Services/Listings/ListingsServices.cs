using Api.Infrastructure.Services.Listings;
using Api.Shared.DTOs.Listing;
using Api.Shared.Models;
using System.ComponentModel;

namespace Api.Infrastructure.Services.Listings
{
    public class ListingsServices : IListingsServices
    {
        private readonly Context _context;

        public ListingsServices(Context context)
        {
            _context = context;
        }

        public async Task<List<Listing>> GetAllListingsAsync()
        {
            return await _context.Listings
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    } 
}


