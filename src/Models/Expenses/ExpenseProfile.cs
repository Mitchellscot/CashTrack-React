using AutoMapper;
using CashTrack.Data.Entities;
using System.Linq;

namespace CashTrack.Models.Expenses
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            this.CreateMap<Data.Entities.Expenses, Expense>()
                .ForMember(e => e.Id, o => o.MapFrom(m => m.id))
                .ForMember(e => e.PurchaseDate, o => o.MapFrom(m => m.purchase_date))
                .ForMember(e => e.Amount, o => o.MapFrom(m => m.amount))
                .ForMember(e => e.Notes, o => o.MapFrom(m => m.notes))
                .ForMember(e => e.Merchant, o => o.MapFrom(src => src.merchant.name))
                .ForMember(e => e.SubCategory, o => o.MapFrom(m => m.category.sub_category_name))
                .ForMember(e => e.MainCategory, o => o.MapFrom(m => m.category.main_category.main_category_name));

            this.CreateMap<Tags, Tag>()
                .ForMember(t => t.Id, o => o.MapFrom(src => src.id))
                .ForMember(t => t.TagName, o => o.MapFrom(src => src.tag_name));

            //this.CreateMap<ExpenseMainCategories, ExpenseMainCategory>()
            //    .ForMember(mc => mc.Id, o => o.MapFrom(m => m.id))
            //    .ForMember(mc => mc.MainCategory, o => o.MapFrom(m => m.main_category_name));

            //this.CreateMap<ExpenseSubCategories, ExpenseSubCategory>()
            //    .ForMember(sc => sc.Id, o => o.MapFrom(m => m.id))
            //    .ForMember(sc => sc.CategoryName, o => o.MapFrom(m => m.sub_category_name))
            //    .ForMember(sc => sc.MainCategoryName, o => o.MapFrom(m => m.sub_category_name));
        }

    }
}
