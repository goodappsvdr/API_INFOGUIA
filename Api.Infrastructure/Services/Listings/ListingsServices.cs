using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.Interface;
using Api.Shared.DTOs.Listings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims; // Necesario para Claims
using Api.Infrastructure.Services.Interface;

namespace Api.Infrastructure.Services.Listings
{
    public class ListingsServices : IListingsServices
    {
        private readonly ContextInfoGuia _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ListingsServices> _logger;
        private readonly IUsersServices _userServices; // 1. Inyectar el servicio de usuario
        private readonly IHttpContextAccessor _httpContext; // 2. Para obtener el username del token

        public ListingsServices(
            ContextInfoGuia context,
            IMapper mapper,
            ILogger<ListingsServices> logger,
            IUsersServices userServices,
            IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _userServices = userServices;
            _httpContext = httpContext;
        }

        public async Task<List<ListingDTO>> GetAllListingsAsync()
        {
            try
            {
                var listings = await _context.Listings
                    .AsNoTracking()
                    .OrderByDescending(l => l.CreatedAt)
                    .ToListAsync();

                return _mapper.Map<List<ListingDTO>>(listings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all listings");
                throw;
            }
        }

        public async Task<ListingDTO> GetListingByIdAsync(int id)
        {
            try
            {
                var listing = await _context.Listings
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ListingId == id);

                if (listing == null)
                {
                    throw new NotFoundException($"Listing with ID {id} not found");
                }

                return _mapper.Map<ListingDTO>(listing);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing {ListingId}", id);
                throw;
            }
        }

        public async Task<ListingDTO> CreateListingAsync(string userId, AddListingDTO listingDto)
        {
            try
            {
                var listing = _mapper.Map<Listing>(listingDto);
                listing.CreatedAt = DateTime.UtcNow;
                listing.CreatedByUserId = userId;

                _context.Listings.Add(listing);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Listing {ListingId} created by user {UserId}", listing.ListingId, userId);

                return _mapper.Map<ListingDTO>(listing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing for user {UserId}", userId);
                throw;
            }
        }

        public async Task<ListingDTO> UpdateListingAsync(string userId, UpdateListingDTO listingDto, int listingId)
        {
            try
            {
                

                var listing = await _context.Listings
                    .FirstOrDefaultAsync(x => x.ListingId == listingId);

                if (listing == null)
                {
                    throw new NotFoundException($"Listing with ID {listingId} not found");
                }

                // Verificar que el usuario sea el dueño del listing
                if (listing.CreatedByUserId != userId)
                {
                    throw new UnauthorizedException("You don't have permission to update this listing");
                }

                _mapper.Map(listingDto, listing);
                listing.ModifiedAt = DateTime.UtcNow;
                listing.ModifiedByUserId = userId;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Listing {ListingId} updated by user {UserId}", listing.ListingId, userId);

                return _mapper.Map<ListingDTO>(listing);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating listing {ListingId}", listingId);
                throw;
            }
        }

    
    }
}