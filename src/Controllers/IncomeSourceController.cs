using CashTrack.Helpers.Exceptions;
using CashTrack.Models.IncomeSourceModels;
using CashTrack.Services.IncomeSourceService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CashTrack.Controllers
{
    public class IncomeSourceController : ControllerBase
    {
        private readonly IIncomeSourceService _service;

        public IncomeSourceController(IIncomeSourceService service) => _service = service;

    }
}
