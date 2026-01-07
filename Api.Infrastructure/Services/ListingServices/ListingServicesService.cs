using Api.Infrastructure.Exceptions;
using Api.Shared.DTOs.ListingServices;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Infrastructure.Services.ListingServices
{
    public class ListingServicesService : IListingServicesService
    {
        private readonly ContextInfoGuia _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ListingServicesService> _logger;

        public ListingServicesService(
            ContextInfoGuia context,
            IMapper mapper,
            ILogger<ListingServicesService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // ===================== CREATE =====================

        public async Task<ListingServicesDto> CreateAsync(
            string userId,
            AddListingServicesDTO dto)
        {
            // Validar Listing
            var listingExists = await _context.Listings
                .AnyAsync(x => x.ListingId == dto.ListingId);

            if (!listingExists)
                throw new BadRequestException($"Listing {dto.ListingId} does not exist");

            // Validar Service
            var serviceExists = await _context.Services
                .AnyAsync(x => x.ServiceId == dto.ServiceId);

            if (!serviceExists)
                throw new BadRequestException($"Service {dto.ServiceId} does not exist");

            // Validar duplicado
            var exists = await _context.ListingServices
                .AnyAsync(x =>
                    x.ListingId == dto.ListingId &&
                    x.ServiceId == dto.ServiceId);

            if (exists)
                throw new BadRequestException("Service already associated with this listing");

            var entity = _mapper.Map<ListingService>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedByUserId = userId;

            _context.ListingServices.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Service {ServiceId} added to listing {ListingId} by user {UserId}",
                dto.ServiceId,
                dto.ListingId,
                userId);

            return _mapper.Map<ListingServicesDto>(entity);
        }

        // ===================== GET BY LISTING =====================

        public async Task<List<ListingServicesDto>> GetByListingIdAsync(int listingId)
        {
            var services = await _context.ListingServices
                .AsNoTracking()
                .Where(x => x.ListingId == listingId)
                .OrderBy(x => x.ServiceId)
                .ToListAsync();

            return _mapper.Map<List<ListingServicesDto>>(services);
        }

        // ===================== DELETE =====================

        public async Task DeleteAsync(
            string userId,
            int listingId,
            int serviceId)
        {
            var entity = await _context.ListingServices
                .FirstOrDefaultAsync(x =>
                    x.ListingId == listingId &&
                    x.ServiceId == serviceId);

            if (entity == null)
                throw new NotFoundException("Service not associated with this listing");

            _context.ListingServices.Remove(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Service {ServiceId} removed from listing {ListingId} by user {UserId}",
                serviceId,
                listingId,
                userId);
        }
    }
}
