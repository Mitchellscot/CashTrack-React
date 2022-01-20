using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using CashTrack.Repositories.ExpenseRepository;
using Microsoft.AspNetCore.Http;
using CashTrack.Models.ExpenseModels;
using System;
using CashTrack.Helpers.Exceptions;
using CashTrack.Services.ExpenseService;
using Microsoft.AspNetCore.Routing;

namespace CashTrack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        //Install this tool to test:
        //https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-5.0

        //also, think about how you can reduce the number of controllers here this is rediculous

        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        public async Task<ActionResult<ExpenseModels.Response>> GetAllExpenses([FromQuery] ExpenseModels.Request request)
        {
            try
            {
                var response = await _expenseService.GetExpensesAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseTransaction>> GetAnExpenseById(int id)
        {
            //I'M THINKING that we will have this displayed in a modal
            //I don't think it needs it's own dedicated page, just a modal that hovers over
            //a table that displays all the expenses in a table. Click on the expense in the table,
            //modal pops up, can edit or delete or whatever... makes it easy as there isn't much info here.
            try
            {
                var result = await _expenseService.GetExpenseByIdAsync(id);
                return Ok(result);
            }
            catch (ExpenseNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("notes")]
        public async Task<ActionResult<ExpenseModels.Response>> GetExpensesByNotes([FromQuery] ExpenseModels.NotesSearchRequest request)
        {
            if (string.IsNullOrEmpty(request.SearchTerm))
            {
                return BadRequest("This endpoint requres a search term");
            }

            try
            {
                var response = await _expenseService.GetExpensesByNotesAsync(request);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("amount")]
        public async Task<ActionResult<ExpenseModels.Response>> GetExpensesByAmount([FromQuery] ExpenseModels.AmountSearchRequest request)
        {
            if (string.IsNullOrEmpty(request.Query.ToString()))
            {
                return BadRequest("This endpoint requres an amount to search for.");
            }

            try
            {
                var response = await _expenseService.GetExpensesByAmountAsync(request);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        //api/expense/category
        //returns transactions from a given category
        [HttpGet("category")]
        public async Task<ActionResult<ExpenseModels.Response>> GetExpenseByCategory([FromQuery] ExpenseModels.SubCategorySearchRequest request)
        {
            if (string.IsNullOrEmpty(request.SubCategoryId.ToString()))
            {
                return BadRequest("This endpoint requres a sub category id.");
            }

            try
            {
                var response = await _expenseService.GetExpensesBySubCategoryAsync(request);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AddEditExpense>> CreateExpense(AddEditExpense request)
        {
            try
            {
                var result = await _expenseService.CreateUpdateExpenseAsync(request);

                var location = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, $"/expense/{result.Id.Value}");
                return Created(location, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<AddEditExpense>> UpdateExpense([FromBody] AddEditExpense request)
        {
            if (request.Id == null)
                return BadRequest("Need an Id to update an expense.");

            try
            {
                var result = await _expenseService.CreateUpdateExpenseAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteExpense(int id)
        {
            try
            {
                var result = await _expenseService.DeleteExpenseAsync(id);
                if (!result)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Unable to delete expense");

                return Ok();
            }
            catch (ExpenseNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException);
            }
        }

        //NEXT UP - MAIN CATEGORY SEARCH

        //    //api/expense/tags/{tagId}
        //    //Accepts a tag id
        //    //returns all transactions that contain that tag Id
        //    [HttpGet("/tags/[tagId]")]
        //    //create a Tag Request model that has a tag Id
        //    public async Task<ActionResult<ExpenseModel[]>> GetExpensesByTag(int tagId)
        //    {
        //        try
        //        {
        //            //logic goes here
        //            return Content("This is the data you were looking for.");
        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }

    }
}
