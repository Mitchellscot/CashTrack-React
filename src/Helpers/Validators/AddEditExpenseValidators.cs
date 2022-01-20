﻿using CashTrack.Data;
using CashTrack.Models.ExpenseModels;
using CashTrack.Repositories.ExpenseRepository;
using CashTrack.Repositories.MerchantRepository;
using CashTrack.Repositories.SubCategoriesRepository.cs;
using CashTrack.Services.ExpenseService;
using FluentValidation;
using System;
using System.Globalization;
using System.Linq;

namespace CashTrack.Helpers.Validators
{
    public class AddEditExpenseValidators : AbstractValidator<AddEditExpense>
    {
        public AddEditExpenseValidators(ISubCategoryRepository _categoryRepo, IMerchantRepository _merchantRepo)
        {
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
            RuleFor(x => x.PurchaseDate).NotEmpty();
            RuleFor(x => x.SubCategoryId).NotEmpty().GreaterThan(0).WithMessage("Must provide a category ID");
            RuleFor(x => x.SubCategoryId).MustAsync(async (model, value, _) =>
            {
                return (await _categoryRepo.GetAllSubCategoriesAsync()).Any(x => x.id == value);
            }).WithMessage("Invalid Category Id");

            When(x => x.MerchantId != null,
                () =>
                {
                    RuleFor(x => x.MerchantId).GreaterThan(0).MustAsync(async (model, value, _) =>
                    {
                        return ((int)await _merchantRepo.GetCountOfAllMerchants()) > value;
                    }).WithMessage("Invalid Merchant Id");
                });
        }
    }
}
