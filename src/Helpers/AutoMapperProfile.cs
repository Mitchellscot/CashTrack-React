using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.Users;

namespace CashTrack.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, AuthenticateResponse>()
                .ForMember(u => u.FirstName, o => o.MapFrom(r => r.first_name))
                .ForMember(u => u.LastName, o => o.MapFrom(r => r.last_name));
            //CreateMap<Expense, ExpenseModel>()
            //    .ForMember(model => model.Maincategory, o => o.MapFrom(src => src.category.category));
        }
    }
}