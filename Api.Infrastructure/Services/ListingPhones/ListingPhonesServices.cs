using Api.Infrastructure.Exceptions;
using Api.Shared.DTOs.ListingPhones;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Infrastructure.Services.ListingPhones
{
    public class ListingPhonesServices : IListingPhonesServices
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ListingPhonesServices> _logger;

        public ListingPhonesServices(
            Context context,
            IMapper mapper,
            ILogger<ListingPhonesServices> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ListingPhonesDto>> GetAllAsync()
        {
            try
            {
                var phones = await _context.ListingPhones
                    .AsNoTracking()
                    .OrderByDescending(p => p.ListingPhoneId)
                    .ToListAsync();

                return _mapper.Map<List<ListingPhonesDto>>(phones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing phones");
                throw;
            }
        }

        public async Task<ListingPhonesDto?> GetByIdAsync(int id)
        {
            try
            {
                var phone = await _context.ListingPhones
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ListingPhoneId == id);

                if (phone == null)
                    throw new NotFoundException($"ListingPhone with ID {id} not found");

                return _mapper.Map<ListingPhonesDto>(phone);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing phone {ListingPhoneId}", id);
                throw;
            }
        }

        public async Task<ListingPhonesDto> CreateAsync(string userId, AddListingPhonesDTO dto)
        {
            try
            {
                var phone = _mapper.Map<ListingPhone>(dto);
                phone.CreatedAt = DateTime.UtcNow;
                phone.CreatedByUserId = userId;
                phone.IsActive = true;

                _context.ListingPhones.Add(phone);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "ListingPhone {ListingPhoneId} created by user {UserId}",
                    phone.ListingPhoneId,
                    userId);

                return _mapper.Map<ListingPhonesDto>(phone);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing phone for user {UserId}", userId);
                throw;
            }
        }

        public async Task<ListingPhonesDto> UpdateAsync(string userId, UpdateListingPhonesDTO dto)
        {
            try
            {
                var phone = await _context.ListingPhones
                    .FirstOrDefaultAsync(x => x.ListingPhoneId == dto.Id);

                if (phone == null)
                    throw new NotFoundException($"ListingPhone with ID {dto.Id} not found");

                if (phone.CreatedByUserId != userId)
                    throw new UnauthorizedException("You don't have permission to update this phone");

                _mapper.Map(dto, phone);
                phone.ModifiedAt = DateTime.UtcNow;
                phone.ModifiedByUserId = userId;

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "ListingPhone {ListingPhoneId} updated by user {UserId}",
                    phone.ListingPhoneId,
                    userId);

                return _mapper.Map<ListingPhonesDto>(phone);
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
                _logger.LogError(ex, "Error updating listing phone {ListingPhoneId}", dto.Id);
                throw;
            }
        }
    }
}
