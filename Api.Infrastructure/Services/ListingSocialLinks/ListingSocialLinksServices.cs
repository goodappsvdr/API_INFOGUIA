using Api.Infrastructure.Exceptions;
using Api.Shared.DTOs.ListingSocialLinks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Infrastructure.Services.ListingSocialLinks
{
    public class ListingSocialLinksServices : IListingSocialLinksServices
    {
        private readonly ContextInfoGuia _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ListingSocialLinksServices> _logger;

        public ListingSocialLinksServices(
            ContextInfoGuia context,
            IMapper mapper,
            ILogger<ListingSocialLinksServices> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // ===================== CREATE =====================

        public async Task<ListingSocialLinksDto> CreateAsync(
            string userId,
            AddListingSocialLinksDTO dto)
        {
            // Validar que el Listing exista
            var listingExists = await _context.Listings
                .AnyAsync(x => x.ListingId == dto.ListingId);

            if (!listingExists)
                throw new BadRequestException($"Listing {dto.ListingId} does not exist");

            // Validar que no exista la misma red para el listing
            var duplicated = await _context.ListingSocialLinks
                .AnyAsync(x =>
                    x.ListingId == dto.ListingId &&
                    x.NetworkName == dto.NetworkName &&
                    x.IsActive);

            if (duplicated)
                throw new BadRequestException("This social network already exists for the listing");

            var entity = _mapper.Map<ListingSocialLink>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedByUserId = userId;
            entity.IsActive = true;

            _context.ListingSocialLinks.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Social link {Id} created for listing {ListingId} by user {UserId}",
                entity.ListingSocialLinkId,
                entity.ListingId,
                userId);

            return _mapper.Map<ListingSocialLinksDto>(entity);
        }

        // ===================== GET BY LISTING =====================

        public async Task<List<ListingSocialLinksDto>> GetByListingIdAsync(int listingId)
        {
            var links = await _context.ListingSocialLinks
                .AsNoTracking()
                .Where(x => x.ListingId == listingId && x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            return _mapper.Map<List<ListingSocialLinksDto>>(links);
        }

        // ===================== GET BY ID =====================

        public async Task<ListingSocialLinksDto> GetByIdAsync(int id)
        {
            var link = await _context.ListingSocialLinks
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ListingSocialLinkId == id);

            if (link == null)
                throw new NotFoundException($"Social link with ID {id} not found");

            return _mapper.Map<ListingSocialLinksDto>(link);
        }

        // ===================== UPDATE =====================

        public async Task<ListingSocialLinksDto> UpdateAsync(
            string userId,
            UpdateListingSocialLinksDTO dto)
        {
            var entity = await _context.ListingSocialLinks
                .FirstOrDefaultAsync(x => x.ListingSocialLinkId == dto.Id);

            if (entity == null)
                throw new NotFoundException($"Social link with ID {dto.Id} not found");

            _mapper.Map(dto, entity);
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedByUserId = userId;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Social link {Id} updated by user {UserId}",
                entity.ListingSocialLinkId,
                userId);

            return _mapper.Map<ListingSocialLinksDto>(entity);
        }

        // ===================== DELETE (SOFT) =====================

        public async Task DeleteAsync(string userId, int id)
        {
            var entity = await _context.ListingSocialLinks
                .FirstOrDefaultAsync(x => x.ListingSocialLinkId == id);

            if (entity == null)
                throw new NotFoundException($"Social link with ID {id} not found");

            entity.IsActive = false;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedByUserId = userId;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Social link {Id} deleted by user {UserId}",
                id,
                userId);
        }
    }
}
