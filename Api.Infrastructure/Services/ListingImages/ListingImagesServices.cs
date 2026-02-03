using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.ListingImages;
using Api.Shared.DTOs.ListingImages;
using Api.Shared.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Infrastructure.Services.ListingImagesServices
{
    public class ListingImagesServices : IListingImagesServices
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ListingImagesServices> _logger;

        public ListingImagesServices(
            Context context,

            IMapper mapper,
            ILogger<ListingImagesServices> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ListingImagesDto>> GetAllAsync()
        {
            try
            {
                var images = await _context.ListingImages
                    .AsNoTracking()
                    .OrderByDescending(x => x.ListingImageId)
                    .ToListAsync();

                return _mapper.Map<List<ListingImagesDto>>(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing images");
                throw;
            }
        }

        public async Task<ListingImagesDto> GetByIdAsync(int id)
        {
            try
            {
                var image = await _context.ListingImages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ListingImageId == id);

                if (image == null)
                    throw new NotFoundException($"ListingImage with ID {id} not found");

                return _mapper.Map<ListingImagesDto>(image);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing image {ImageId}", id);
                throw;
            }
        }

        public async Task<ListingImagesDto> CreateAsync(string userId, AddListingImagesDTO dto)
        {
            try
            {
                var image = _mapper.Map<ListingImage>(dto);
                image.CreatedAt = DateTime.UtcNow;
                image.CreatedByUserId = userId;
                image.IsActive = true;

                _context.ListingImages.Add(image);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "ListingImage {ListingImageId} created by user {UserId}",
                    image.ListingImageId,
                    userId
                );

                return _mapper.Map<ListingImagesDto>(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing image for user {UserId}", userId);
                throw;
            }
        }

        public async Task<ListingImagesDto> UpdateAsync(string userId, UpdateListingImagesDTO dto)
        {
            try
            {
                var image = await _context.ListingImages
                    .FirstOrDefaultAsync(x => x.ListingImageId == dto.Id);

                if (image == null)
                    throw new NotFoundException($"ListingImage with ID {dto.Id} not found");

                if (image.CreatedByUserId != userId)
                    throw new UnauthorizedException("You don't have permission to update this image");

                _mapper.Map(dto, image);
                image.ModifiedAt = DateTime.UtcNow;
                image.ModifiedByUserId = userId;

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "ListingImage {ListingImageId} updated by user {UserId}",
                    image.ListingImageId,
                    userId
                );

                return _mapper.Map<ListingImagesDto>(image);
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
                _logger.LogError(ex, "Error updating listing image {ListingImageId}", dto.Id);
                throw;
            }
        }
    }
}
