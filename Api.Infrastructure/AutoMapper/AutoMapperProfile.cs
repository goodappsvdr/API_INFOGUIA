using Api.Shared.DTOs.Listings;
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

        }
    }
}
