using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.Interface;
using Api.Shared.DTOs.Listings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims; // Necesario para Claims
using Api.Infrastructure.Services.Interface;
using Api.Shared.DTOs.ListingHours;
using Api.Shared.DTOs.ListingImages;
using Api.Shared.DTOs.ListingSocialLinks;
using Api.Shared.DTOs.ListingPhones;

namespace Api.Infrastructure.Services.Listings
{
    public class ListingsServices : IListingsServices
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ListingsServices> _logger;
        private readonly IUsersServices _userServices; // 1. Inyectar el servicio de usuario
        private readonly IHttpContextAccessor _httpContext; // 2. Para obtener el username del token

        public ListingsServices(
            Context context,
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
        public async Task<GetAllListingsResult> GetAllListingsAsync()
        {
            try
            {
                var query =
                    from l in _context.Listings

                    join c in _context.Categories
                        on l.CategoryId equals c.CategoryId into catJoin
                    from c in catJoin.DefaultIfEmpty()

                    join ci in _context.Cities
                        on l.CityId equals ci.CityId into cityJoin
                    from ci in cityJoin.DefaultIfEmpty()

                    select new GetAllListing
                    {
                        ListingId = l.ListingId,
                        TenantId = l.TenantId,

                        CategoryId = l.CategoryId,
                        Category = c != null ? c.Name : string.Empty,

                        CityId = l.CityId,
                        City = ci != null ? ci.Name : string.Empty,

                        Name = l.Name,
                        ShortDescription = l.ShortDescription,
                        LongDescription = l.LongDescription,
                        LogoUrl = l.LogoUrl,
                        Address = l.Address,
                        Latitude = l.Latitude,
                        Longitude = l.Longitude,
                        Email = l.Email,
                        WebsiteUrl = l.WebsiteUrl,
                        VideoUrl = l.VideoUrl,
                        CatalogUrl = l.CatalogUrl,
                        SortOrder = l.SortOrder,
                        IsActive = l.IsActive,
                        UserId = l.CreatedByUserId ?? 0,

                        // SERVICES
                        Services = _context.ListingServices
                            .Where(ls => ls.ListingId == l.ListingId)
                            .Select(ls => new GetListingServices
                            {
                                ServiceId = ls.ServiceId,
                                CreatedAt = ls.CreatedAt
                            })
                            .ToList(),

                        // HOURS
                        Hours = _context.ListingHours
                        .Where(lh => lh.ListingId == l.ListingId)
                        .Select(lh => new GetListingHours
                        {
                        DayOfWeek = lh.DayOfWeek,
                        OpenTime = lh.OpenTime.HasValue
                                ? lh.OpenTime.Value.ToString(@"hh\:mm")
                                : string.Empty,
                            CloseTime = lh.CloseTime.HasValue
                                ? lh.CloseTime.Value.ToString(@"hh\:mm")
                                : string.Empty
                        })
                        .ToList(),
               
                        //IMAGES
                    Images = _context.ListingImages
                    .Where(li => li.ListingId == l.ListingId)
                    .Select(li => new GetListingImages
                    {
                    ListingImageID = li.ListingImageId,
                        ImageUrl = li.ImageUrl,
                        Caption = li.Caption
                    })
                    .ToList(),

                //SOCIAL LINKS
                socialLinks = _context.ListingSocialLinks
                .Where(lsl => lsl.ListingId == l.ListingId)
                .Select(lsl => new GetListingSocialLinks
                {
                    ListingSocialLinkID = lsl.ListingSocialLinkId,
                    NetworkName = lsl.NetworkName,
                    ProfileUrl = lsl.ProfileUrl
                })
                .ToList(),

                //PHONES
                phones = _context.ListingPhones
                .Where(lp => lp.ListingId == l.ListingId)
                .Select(lp => new GetListingPhones
                {
                    ListingPhoneID = lp.ListingPhoneId,
                    PhoneType = lp.PhoneType,
                    PhoneNumber = lp.PhoneNumber
                })
                .ToList()
                    };

            var totalCount = await query.CountAsync();

                var listings = await query
                    .OrderByDescending(l => l.ListingId)
                    .ToListAsync();

                return new GetAllListingsResult
                {
                    TotalCount = totalCount,
                    Items = listings
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listings by category");
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

        public async Task<ListingDTO> CreateListingAsync(int userId, AddListingDTO listingDto)
        {
            try
            {
                var listing = _mapper.Map<Listing>(listingDto);
                listing.CreatedAt = DateTime.UtcNow;
                listing.CreatedByUserId = userId;

                _context.Listings.Add(listing);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Listing {ListingId} created by user {UserId}", listing.ListingId, userId);

                // Agrego la relacion entre usuarios y listing

                _context.ListingUsers.Add(new ListingUser
                {
                    ListingId = listing.ListingId,
                    UserId = Convert.ToInt32(userId)
                });

                await _context.SaveChangesAsync();

                return _mapper.Map<ListingDTO>(listing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing for user {UserId}", userId);
                throw;
            }
        }

        public async Task<ListingDTO> UpdateListingAsync(int userId, UpdateListingDTO listingDto, int listingId)
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


        public async Task<GetAllListingsByResult> GetListingsByCategoryIdAsync(int categoryId)
        {
            try
            {
                var query =
                from l in _context.Listings
                where l.CategoryId == categoryId

                join c in _context.Categories
                        on l.CategoryId equals c.CategoryId into catJoin
                    from c in catJoin.DefaultIfEmpty()

                    join ci in _context.Cities
                        on l.CityId equals ci.CityId into cityJoin
                    from ci in cityJoin.DefaultIfEmpty()

                    select new GetAllListingBy
                    {
                        ListingId = l.ListingId,
                        TenantId = l.TenantId,

                        CategoryId = l.CategoryId,
                        Category = c != null ? c.Name : string.Empty,

                        CityId = l.CityId,
                        City = ci != null ? ci.Name : string.Empty,

                        Name = l.Name,
                        ShortDescription = l.ShortDescription,
                        LongDescription = l.LongDescription,
                        LogoUrl = l.LogoUrl,
                        Address = l.Address,
                        Latitude = l.Latitude,
                        Longitude = l.Longitude,
                        Email = l.Email,
                        WebsiteUrl = l.WebsiteUrl,
                        VideoUrl = l.VideoUrl,
                        CatalogUrl = l.CatalogUrl,
                        SortOrder = l.SortOrder,
                        IsActive = l.IsActive,
                        UserId = l.CreatedByUserId ?? 0
                    };

                var totalCount = await query.CountAsync();

                var listings = await query
                    .OrderByDescending(l => l.ListingId)
                    .ToListAsync();

                return new GetAllListingsByResult
                {
                    TotalCount = totalCount,
                    Items = listings
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all listings");
                throw;
            }
        }


    }
}