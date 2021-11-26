using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.Users;

namespace CashTrack.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, Authentication.Response>()
                .ForMember(u => u.Id, o => o.MapFrom(src => src.id))
                .ForMember(u => u.FirstName, o => o.MapFrom(src => src.first_name))
                .ForMember(u => u.LastName, o => o.MapFrom(src => src.last_name))
                .ForMember(u => u.Email, o => o.MapFrom(src => src.email))
                ;
            //CreateMap<Expense, ExpenseModel>()
            //    .ForMember(model => model.Maincategory, o => o.MapFrom(src => src.category.category));
        }
    }
}