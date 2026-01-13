using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.ListingHours;
using Api.Shared.DTOs.ListingHours;
using Api.Shared.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Infrastructure.Services.ListingHoursServices
{
    public class ListingHoursServices : IListingHoursServices
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ListingHoursServices> _logger;

        public ListingHoursServices(
            Context context,
            IMapper mapper,
            ILogger<ListingHoursServices> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ListingHoursDto>> GetAllAsync()
        {
            try
            {
                var hours = await _context.ListingHours
                    .AsNoTracking()
                    .OrderBy(x => x.ListingId)
                    .ThenBy(x => x.DayOfWeek)
                    .ThenBy(x => x.OpenTime)
                    .ToListAsync();

                return _mapper.Map<List<ListingHoursDto>>(hours);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing hours");
                throw;
            }
        }

        public async Task<ListingHoursDto> GetByIdAsync(int id)
        {
            try
            {
                var hour = await _context.ListingHours
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ListingHourId == id);

                if (hour == null)
                    throw new NotFoundException($"ListingHour with ID {id} not found");

                return _mapper.Map<ListingHoursDto>(hour);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing hour {Id}", id);
                throw;
            }
        }

        public async Task<ListingHoursDto> CreateAsync(string userId, AddListingHoursDTO dto)
        {
            try
            {
                var hour = _mapper.Map<ListingHour>(dto);
                hour.CreatedAt = DateTime.UtcNow;
                hour.CreatedByUserId = userId;
                hour.IsActive = true;

                _context.ListingHours.Add(hour);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "ListingHour {ListingHourId} created by user {UserId}",
                    hour.ListingHourId,
                    userId
                );

                return _mapper.Map<ListingHoursDto>(hour);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing hour for user {UserId}", userId);
                throw;
            }
        }

        public async Task<ListingHoursDto> UpdateAsync(string userId, UpdateListingHoursDTO dto)
        {
            try
            {
                var hour = await _context.ListingHours
                    .FirstOrDefaultAsync(x => x.ListingHourId == dto.Id);

                if (hour == null)
                    throw new NotFoundException($"ListingHour with ID {dto.Id} not found");

                if (hour.CreatedByUserId != userId)
                    throw new UnauthorizedException("You don't have permission to update these hours");

                _mapper.Map(dto, hour);
                hour.ModifiedAt = DateTime.UtcNow;
                hour.ModifiedByUserId = userId;

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "ListingHour {ListingHourId} updated by user {UserId}",
                    hour.ListingHourId,
                    userId
                );

                return _mapper.Map<ListingHoursDto>(hour);
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
                _logger.LogError(ex, "Error updating listing hour {ListingHourId}", dto.Id);
                throw;
            }
        }
    }
}
