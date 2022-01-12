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

            //Users is the entity... might have to fix this in the future
            CreateMap<Users, Authentication.Response>()
                .ForMember(u => u.Id, o => o.MapFrom(src => src.id))
                .ForMember(u => u.FirstName, o => o.MapFrom(src => src.first_name))
                .ForMember(u => u.LastName, o => o.MapFrom(src => src.last_name))
                .ForMember(u => u.Email, o => o.MapFrom(src => src.email));

            CreateMap<Expenses, ExpenseModels.ExpenseTransaction>()
                .ForMember(e => e.Id, o => o.MapFrom(src => src.id))
                .ForMember(e => e.PurchaseDate, o => o.MapFrom(src => src.purchase_date))
                .ForMember(e => e.Amount, o => o.MapFrom(src => src.amount))
                .ForMember(e => e.Notes, o => o.MapFrom(src => src.notes))
                .ForMember(e => e.Merchant, o => o.MapFrom(src => src.merchant.name))
                .ForMember(e => e.SubCategory, o => o.MapFrom(src => src.category.sub_category_name))
                .ForMember(e => e.MainCategory, o => o.MapFrom(src => src.category.main_category.main_category_name))
                .ForMember(e => e.Tags, o => o.MapFrom(
                    src => src.expense_tags.Select(a => new Tag() { Id = a.tag_id, TagName = a.tag.tag_name })));

            CreateMap<Tags, Tag>()
                .ForMember(t => t.Id, o => o.MapFrom(src => src.id))
                .ForMember(t => t.TagName, o => o.MapFrom(src => src.tag_name))
                .ReverseMap();

            CreateMap<Merchants, MerchantModels.Merchant>()
                .ForMember(m => m.Id, o => o.MapFrom(src => src.id))
                .ForMember(m => m.Name, o => o.MapFrom(src => src.name))
                .ForMember(m => m.City, o => o.MapFrom(src => src.city))
                .ForMember(m => m.State, o => o.MapFrom(src => src.state))
                .ForMember(m => m.Notes, o => o.MapFrom(src => src.notes))
                .ForMember(m => m.IsOnline, o => o.MapFrom(src => src.is_online))
                .ReverseMap();
        }
    }
}