namespace Api.Infrastructure.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            #region Identity User 

            CreateMap<IdentityUserProfile, User_Update>().ReverseMap();

            CreateMap<IdentityUserProfile, User_Create>().ReverseMap();

            CreateMap<AspNetUser, Jwt_Claims>()
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                 .AfterMap((src, dest) =>
                 {
                     dest.RoleName = src.Roles.Select(x => x.Name).FirstOrDefault();
                 })
                 .ReverseMap();
            #endregion

            #region Notifications
            //CreateMap<Notification, Notifications_GetByUser>()
            //    .ForMember(dest => dest.NotificationId, opt => opt.MapFrom(src => src.Id))
            //    .ReverseMap();

            //CreateMap<Notification, Notifications_SendModel>()
            //    .ForMember(dest => dest.NotificationId, opt => opt.MapFrom(src => src.Id))
            //    .ReverseMap();
            #endregion

            #region Politics
            CreateMap<Politic, Politic_Create>().ReverseMap();

            CreateMap<Politic, Politic_Update>().ReverseMap();

            CreateMap<Politic, Politic_Get>().ReverseMap();
            #endregion

            #region Help
            CreateMap<Help, Help_Create>().ReverseMap();

            CreateMap<Help, Help_Update>()
                .ForMember(dest => dest.HelpId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<Help, Help_Get>()
                 .ForMember(dest => dest.HelpId, opt => opt.MapFrom(src => src.Id))
                 .ReverseMap();
            #endregion



        }
    }
}
