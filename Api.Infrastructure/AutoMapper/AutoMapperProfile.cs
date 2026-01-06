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


            // Listings
            CreateMap<Listing, ListingDTO>()
                .ReverseMap();
            CreateMap<AddListingDTO, Listing>()
                .ReverseMap();
            CreateMap<UpdateListingDTO, Listing>()
                .ReverseMap();

        }
    }
}
