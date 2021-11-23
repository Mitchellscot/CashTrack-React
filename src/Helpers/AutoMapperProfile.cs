using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.Users;
using CashTrack.Models.Expenses;

//this class doesn't do shit as automapper can't find it
namespace CashTrack.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, AuthenticateResponse>();
            //CreateMap<Expense, ExpenseModel>()
            //    .ForMember(model => model.MainCatagory, o => o.MapFrom(src => src.Catagory.Catagory));
        }
    }
}