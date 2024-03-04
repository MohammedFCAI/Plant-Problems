namespace Plant_Problems.Core.Mappings.ImagePredications
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            AddImageMapping();
            GetImagePredicationMapping();
        }

        private void AddImageMapping()
        {

            CreateMap<AddImagePredicationRequestCommand, ImagePredication>()
                .ForPath(dest => dest.User.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Prdication, opt => opt.MapFrom(src => src.Response))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        }

        private void GetImagePredicationMapping()
        {
            CreateMap<ImagePredication, GetImagePredictionRequestQuery>()
                .ForMember(des => des.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(des => des.Username, opt => opt.MapFrom(src => src.User.UserName));
        }
    }

}
