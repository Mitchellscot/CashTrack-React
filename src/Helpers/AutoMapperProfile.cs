using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.AuthenticationModels;
using CashTrack.Models.ExpenseModels;
using CashTrack.Models.MerchantModels;
using CashTrack.Models.TagModels;
using CashTrack.Models.UserModels;
using System.Linq;

namespace CashTrack.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Users, User.Response>()
                .ForMember(u => u.id, o => o.MapFrom(src => src.id))
                .ForMember(u => u.FirstName, o => o.MapFrom(src => src.first_name))
                .ForMember(u => u.LastName, o => o.MapFrom(src => src.last_name))
                .ForMember(u => u.Email, o => o.MapFrom(src => src.email));

            CreateMap<Expenses, ExpenseTransaction>()
                .ForMember(e => e.Id, o => o.MapFrom(src => src.id))
                .ForMember(e => e.PurchaseDate, o => o.MapFrom(src => src.purchase_date))
                .ForMember(e => e.Amount, o => o.MapFrom(src => src.amount))
                .ForMember(e => e.Notes, o => o.MapFrom(src => src.notes))
                .ForMember(e => e.Merchant, o => o.MapFrom(src => src.merchant.name))
                .ForMember(e => e.SubCategory, o => o.MapFrom(src => src.category.sub_category_name))
                .ForMember(e => e.MainCategory, o => o.MapFrom(src => src.category.main_category.main_category_name))
                .ForMember(e => e.Tags, o => o.MapFrom(
                    src => src.expense_tags.Select(a => new Tag() { Id = a.tag_id, TagName = a.tag.tag_name })));

            CreateMap<AddEditExpense, Expenses>()
                .ForMember(e => e.id, o => o.MapFrom(src => src.Id))
                .ForMember(e => e.purchase_date, o => o.MapFrom(src => src.PurchaseDate.ToUniversalTime()))
                .ForMember(e => e.amount, o => o.MapFrom(src => src.Amount))
                .ForMember(e => e.notes, o => o.MapFrom(src => src.Notes))
                .ForMember(e => e.merchantid, o => o.MapFrom(src => src.MerchantId))
                .ForMember(e => e.categoryid, o => o.MapFrom(src => src.SubCategoryId))
                .ReverseMap();

            CreateMap<Tags, Tag>()
                .ForMember(t => t.Id, o => o.MapFrom(src => src.id))
                .ForMember(t => t.TagName, o => o.MapFrom(src => src.tag_name))
                .ReverseMap();
        }
    }
}