using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.ListingPaymentMethods;
using Api.Shared.DTOs.ListingPaymentMethods;
using Api.Shared.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Infrastructure.Services.ListingPaymentMethodsServices
{
    public class ListingPaymentMethodsServices : IListingPaymentMethodsServices
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ListingPaymentMethodsServices> _logger;

        public ListingPaymentMethodsServices(
            Context context,
            IMapper mapper,
            ILogger<ListingPaymentMethodsServices> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ListingPaymentMethodsDto>> GetAllAsync()
        {
            try
            {
                var items = await _context.ListingPaymentMethods
                    .AsNoTracking()
                    .OrderByDescending(x => x.ListingId)
                    .ThenBy(x => x.PaymentMethodId)
                    .ToListAsync();

                return _mapper.Map<List<ListingPaymentMethodsDto>>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing payment methods");
                throw;
            }
        }

        public async Task<ListingPaymentMethodsDto> GetByIdAsync(int listingId, int paymentMethodId)
        {
            try
            {
                var item = await _context.ListingPaymentMethods
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.ListingId == listingId &&
                        x.PaymentMethodId == paymentMethodId);

                if (item == null)
                    throw new NotFoundException(
                        $"ListingPaymentMethod not found (ListingID: {listingId}, PaymentMethodID: {paymentMethodId})"
                    );

                return _mapper.Map<ListingPaymentMethodsDto>(item);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error retrieving listing payment method {ListingId}-{PaymentMethodId}",
                    listingId, paymentMethodId);

                throw;
            }
        }

        public async Task<ListingPaymentMethodsDto> CreateAsync(string userId, AddListingPaymentMethodsDTO dto)
        {
            try
            {
                var entity = _mapper.Map<ListingPaymentMethod>(dto);
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedByUserId = userId;

                _context.ListingPaymentMethods.Add(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "ListingPaymentMethod created: ListingId {ListingId}, PaymentMethodId {PaymentMethodId}, User {UserId}",
                    entity.ListingId,
                    entity.PaymentMethodId,
                    userId
                );

                return _mapper.Map<ListingPaymentMethodsDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error creating listing payment method for user {UserId}",
                    userId);

                throw;
            }
        }

        public async Task<ListingPaymentMethodsDto> UpdateAsync(string userId, UpdateListingPaymentMethodsDTO dto)
        {
            try
            {
                var entity = await _context.ListingPaymentMethods
                    .FirstOrDefaultAsync(x =>
                        x.ListingId == dto.ListingId &&
                        x.PaymentMethodId == dto.Id);

                if (entity == null)
                    throw new NotFoundException(
                        $"ListingPaymentMethod not found (ListingID: {dto.ListingId}, PaymentMethodID: {dto.Id})"
                    );

                if (entity.CreatedByUserId != userId)
                    throw new UnauthorizedException("You don't have permission to update this listing payment method");

                _mapper.Map(dto, entity);
                entity.ModifiedAt = DateTime.UtcNow;
                entity.ModifiedByUserId = userId;

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "ListingPaymentMethod updated: ListingId {ListingId}, PaymentMethodId {PaymentMethodId}, User {UserId}",
                    entity.ListingId,
                    entity.PaymentMethodId,
                    userId
                );

                return _mapper.Map<ListingPaymentMethodsDto>(entity);
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
                _logger.LogError(ex,
                    "Error updating listing payment method {ListingId}-{PaymentMethodId}",
                    dto.ListingId, dto.Id);

                throw;
            }
        }
    }
}
