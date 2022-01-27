using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Helpers.Exceptions;
using CashTrack.Models.IncomeCategoryModels;
using CashTrack.Repositories.IncomeCategoryRepository;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Services.IncomeCategoryService;

public interface IIncomeCategoryService
{
    Task<IncomeCategoryModels.Response> GetIncomeCategoriesAsync(IncomeCategoryModels.Request request);
}
public class IncomeCategoryService : IIncomeCategoryService
{
    public Task<IncomeCategoryModels.Response> GetIncomeCategoriesAsync(IncomeCategoryModels.Request request)
    {
        throw new NotImplementedException();
    }
}

