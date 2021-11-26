using AutoMapper;
using CashTrack.Data.Entities;

namespace CashTrack.Models.expenses
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            this.CreateMap<Expenses, Expense>()
                .ForMember(e => e.Id, o => o.MapFrom(m => m.id))
                .ForMember(e => e.PurchaseDate, o => o.MapFrom(m => m.purchase_date))
                .ForMember(e => e.Amount, o => o.MapFrom(m => m.amount))
                .ForMember(e => e.Notes, o => o.MapFrom(m => m.notes))
                .ForMember(e => e.SubCategory, o => o.MapFrom(m => m.category))
                .ForMember(e => e.MainCategory, o => o.MapFrom(m => m.category.category));

            this.CreateMap<Tags, Tag>()
                .ForMember(t => t.Id, o => o.MapFrom(x => x.id))
                .ForMember(t => t.Name, o => o.MapFrom(x => x.tag_name));

            this.CreateMap<ExpenseMainCategories, ExpenseMainCategory>()
                .ForMember(mc => mc.Id, o => o.MapFrom(m => m.id))
                .ForMember(mc => mc.MainCategory, o => o.MapFrom(m => m.category));

            this.CreateMap<ExpenseSubCategories, ExpenseSubCategory>()
                .ForMember(sc => sc.Id, o => o.MapFrom(m => m.id))
                .ForMember(sc => sc.CategoryName, o => o.MapFrom(m => m.name))
                .ForMember(sc => sc.MainCategoryName, o => o.MapFrom(m => m.category.category));
        }

    }
}
