using Api.Shared.DTOs.Categories;
using Api.Shared.DTOs.ListingHours;
using Api.Shared.DTOs.ListingImages;
using Api.Shared.DTOs.ListingPaymentMethods;
using Api.Shared.DTOs.ListingPhones;
using Api.Shared.DTOs.Listings;
using Api.Shared.DTOs.ListingServices;
using Api.Shared.DTOs.ListingSocialLinks;
using Api.Shared.DTOs.Roles;
using Api.Shared.Models;
using MimeKit;

namespace Api.Infrastructure.AutoMapper
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{

            #region Auth

            //CreateMap<Usuario, Jwt_Claims>()
            //   .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.IdUsuario))
            //   .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.IdSucursal))
            //   .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Usuario1))
            //   .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Nombre))
            //   .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            //   .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Imagen))
            //   .ReverseMap();

            //    
            #endregion


            // ========== LISTINGS ==========

            // Listing -> ListingDTO
            CreateMap<Listing, ListingDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ListingId))
                .ForMember(d => d.LogoUrl, o => o.MapFrom(s => s.LogoUrl ?? string.Empty))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address ?? string.Empty));

            // AddListingDTO -> Listing
            CreateMap<AddListingDTO, Listing>()
                .ForMember(d => d.ListingId, o => o.Ignore())
                .ForMember(d => d.IsActive, o => o.MapFrom(s => true))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());


            // UpdateListingDTO -> Listing
            CreateMap<UpdateListingDTO, Listing>()
                .ForMember(d => d.ListingId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.TenantId, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // Listing -> UpdateListingDTO
            CreateMap<Listing, UpdateListingDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ListingId));



            // ========== LISTINGS SOCIAL LINKS ==========

            // ListingSocialLink -> ListingSocialLinkDTO
            CreateMap<ListingSocialLink, ListingSocialLinksDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ListingSocialLinkId))
                .ForMember(d => d.ListingId, o => o.MapFrom(s => s.ListingId))
                .ForMember(d => d.NetworkName, o => o.MapFrom(s => s.NetworkName))
                .ForMember(d => d.ProfileUrl, o => o.MapFrom(s => s.ProfileUrl))
                .ForMember(d => d.SortOrder, o => o.MapFrom(s => s.SortOrder))
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive));

            // AddListingSocialLinkDTO -> ListingSocialLink
            CreateMap<AddListingSocialLinksDTO, ListingSocialLink>()
                .ForMember(d => d.ListingSocialLinkId, o => o.Ignore())
                .ForMember(d => d.IsActive, o => o.MapFrom(_ => true))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());


            // UpdateListingSocialLinkDTO -> ListingSocialLink
            CreateMap<UpdateListingSocialLinksDTO, ListingSocialLink>()
                .ForMember(d => d.ListingSocialLinkId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());


            // ListingSocialLink -> UpdateListingSocialLinkDTO
            CreateMap<ListingSocialLink, UpdateListingSocialLinksDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ListingSocialLinkId));

            // ========== LISTING SERVICES ==========

            // ListingService -> ListingServicesDto
            CreateMap<ListingService, ListingServicesDto>()
                .ForMember(d => d.ListingId, o => o.MapFrom(s => s.ListingId))
                .ForMember(d => d.ServiceId, o => o.MapFrom(s => s.ServiceId));

            // AddListingServicesDTO -> ListingService
            CreateMap<AddListingServicesDTO, ListingService>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // ========== LISTING PHONES ==========

            // ListingPhone -> ListingPhonesDto
            CreateMap<ListingPhone, ListingPhonesDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ListingPhoneId))
                .ForMember(d => d.ListingId, o => o.MapFrom(s => s.ListingId))
                .ForMember(d => d.PhoneType, o => o.MapFrom(s => s.PhoneType))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.PhoneNumber))
                .ForMember(d => d.SortOrder, o => o.MapFrom(s => s.SortOrder))
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive));

            // AddListingPhonesDTO -> ListingPhone
            CreateMap<AddListingPhonesDTO, ListingPhone>()
                .ForMember(d => d.ListingPhoneId, o => o.Ignore())
                .ForMember(d => d.IsActive, o => o.MapFrom(_ => true))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // UpdateListingPhonesDTO -> ListingPhone
            CreateMap<UpdateListingPhonesDTO, ListingPhone>()
                .ForMember(d => d.ListingPhoneId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // ========== LISTING IMAGES ==========

            // ListingImage -> ListingImagesDto
            CreateMap<ListingImage, ListingImagesDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ListingImageId));

            // AddListingImagesDTO -> ListingImage
            CreateMap<AddListingImagesDTO, ListingImage>()
                .ForMember(d => d.ListingImageId, o => o.Ignore())
                .ForMember(d => d.IsActive, o => o.MapFrom(_ => true))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // UpdateListingImagesDTO -> ListingImage
            CreateMap<UpdateListingImagesDTO, ListingImage>()
                .ForMember(d => d.ListingImageId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // ========== LISTING HOURS ==========

            // ListingHour -> ListingHoursDto
            CreateMap<ListingHour, ListingHoursDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ListingHourId))
                .ForMember(d => d.ListingId, o => o.MapFrom(s => s.ListingId))
                .ForMember(d => d.DayOfWeek, o => o.MapFrom(s => s.DayOfWeek))
                .ForMember(d => d.OpenTime, o => o.MapFrom(s => s.OpenTime))
                .ForMember(d => d.CloseTime, o => o.MapFrom(s => s.CloseTime))
                .ForMember(d => d.ValidFrom, o => o.MapFrom(s => s.ValidFrom))
                .ForMember(d => d.ValidUntil, o => o.MapFrom(s => s.ValidUntil))
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive));

            // AddListingHoursDTO -> ListingHour
            CreateMap<AddListingHoursDTO, ListingHour>()
                .ForMember(d => d.ListingHourId, o => o.Ignore())
                .ForMember(d => d.IsActive, o => o.MapFrom(_ => true))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // UpdateListingHoursDTO -> ListingHour
            CreateMap<UpdateListingHoursDTO, ListingHour>()
                .ForMember(d => d.ListingHourId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // ListingHour -> UpdateListingHoursDTO
            CreateMap<ListingHour, UpdateListingHoursDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ListingHourId));

            // ========== LISTING PAYMENT METHODS ==========

            // ListingPaymentMethod -> ListingPaymentMethodsDto
            CreateMap<ListingPaymentMethod, ListingPaymentMethodsDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.PaymentMethodId))
                .ForMember(d => d.ListingId, o => o.MapFrom(s => s.ListingId));

            // AddListingPaymentMethodsDTO -> ListingPaymentMethod
            CreateMap<AddListingPaymentMethodsDTO, ListingPaymentMethod>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // UpdateListingPaymentMethodsDTO -> ListingPaymentMethod
            CreateMap<UpdateListingPaymentMethodsDTO, ListingPaymentMethod>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // ListingPaymentMethod -> UpdateListingPaymentMethodsDTO
            CreateMap<ListingPaymentMethod, UpdateListingPaymentMethodsDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.PaymentMethodId));


            // ========== ROLES ==========


            // CreateRoleDto -> Role (entidad)
            CreateMap<CreateRoleDto, Role>()
                .ForMember(d => d.RoleId, o => o.Ignore()); // El ID se genera automáticamente

            // Role (entidad) -> RoleDto
            CreateMap<Role, RoleDto>();


            // ========== CATEGORIES ==========

            // AddCategoryDTO -> Category
            CreateMap<AddCategoryDTO, Category>()
                .ForMember(d => d.CategoryId, o => o.Ignore())
                .ForMember(d => d.TenantId, o => o.Ignore()) // Se suele asignar en el servicio
                .ForMember(d => d.IsActive, o => o.MapFrom(_ => true))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // Category -> CategorieDTO
            CreateMap<Category, CategorieDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.CategoryId));

            // UpdateCategoryDTO -> Category
            CreateMap<UpdateCategoryDTO, Category>()
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.ModifiedAt, o => o.Ignore())
                .ForMember(d => d.ModifiedByUserId, o => o.Ignore());

            // Category -> CategoryWithStatsDTO
            CreateMap<Category, CategoryWithStatsDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.ListingCount, o => o.Ignore()); // Se calcula manualmente usualmente
        }
    }
}
