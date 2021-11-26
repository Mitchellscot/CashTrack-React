using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;
using CashTrack.Models.TagModels;
using CashTrack.Models.UserModels;
using System.Linq;

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
                .ForMember(u => u.Email, o => o.MapFrom(src => src.email));

            CreateMap<Expenses, Expense>()
                .ForMember(e => e.Id, o => o.MapFrom(m => m.id))
                .ForMember(e => e.PurchaseDate, o => o.MapFrom(m => m.purchase_date))
                .ForMember(e => e.Amount, o => o.MapFrom(m => m.amount))
                .ForMember(e => e.Notes, o => o.MapFrom(m => m.notes))
                .ForMember(e => e.Merchant, o => o.MapFrom(src => src.merchant.name))
                .ForMember(e => e.SubCategory, o => o.MapFrom(m => m.category.sub_category_name))
                .ForMember(e => e.MainCategory, o => o.MapFrom(m => m.category.main_category.main_category_name))
                .ForMember(e => e.Tags, o => o.MapFrom(
                    m => m.expense_tags.Select(a => new Tag() { Id = a.tag_id, TagName = a.tag.tag_name })));

            CreateMap<Tags, Tag>()
                .ForMember(t => t.Id, o => o.MapFrom(src => src.id))
                .ForMember(t => t.TagName, o => o.MapFrom(src => src.tag_name))
                .ReverseMap();
        }
    }
}