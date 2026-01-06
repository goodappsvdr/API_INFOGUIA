using Api.Shared.DTOs.Listings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure.Services.Listings
{
    public class ListingsServices : IListingsServices
    {
        private readonly ContextInfoGuia _context;
        private readonly IMapper _mapper;

        public ListingsServices(ContextInfoGuia context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // Implement listing-related methods here
        //Create, Read, Update, Delete (CRUD) operations for listings

        //Method to get all listings

        public async Task<List<ListingDTO>> GetAllListingsAsync()
        {
            var listings = await _context.Listings
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<ListingDTO>>(listings);
        }

        // Method to get listing by id
        public async Task<Listing> GetListingByIdAsync(int id)
        {
            var listing = await _context.Listings
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ListingId == id);
            return _mapper.Map<Listing>(listing);
        }

        // Method to create a new listing
        public async Task<ListingDTO> CreateListingAsync(string userId, AddListingDTO listingDto)
        {
            var listing = _mapper.Map<Listing>(listingDto);
            listing.CreatedAt = DateTime.UtcNow;
            listing.CreatedByUserId = userId.ToString();
            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();
            return _mapper.Map<ListingDTO>(listing);
        }

        // Method to update a listing
        public async Task<ListingDTO> UpdateListingAsync(string userId, ListingDTO listingDto)
        {

            var listing = await _context.Listings
                .FirstOrDefaultAsync(x => x.ListingId == listingDto.Id);
            if (listing == null)
            {
                return null;
            }
            _mapper.Map(listingDto, listing);
            listing.ModifiedAt = DateTime.UtcNow;
            listing.ModifiedByUserId = userId.ToString();
            await _context.SaveChangesAsync();
            return _mapper.Map<ListingDTO>(listing);
        }

    }
}
