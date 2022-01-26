using CashTrack.Models.MainCategoryModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Shouldly;
using Newtonsoft.Json;
using System.Net;

namespace CashTrack.IntegrationTests
{
    public class MainCategoryEndpointsShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        private readonly ITestOutputHelper _output;
        const string ENDPOINT = "api/maincategory";

        public MainCategoryEndpointsShould(TestServerFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }
        [Fact]
        public async Task ReturnAllMainCategories()
        {
            var response = await _fixture.Client.GetAsync(ENDPOINT);
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<MainCategoryModels.Response>(await response.Content.ReadAsStringAsync());
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            responseObject.TotalMainCategories.ShouldBeGreaterThan(15);
            var categoryNumber = responseObject.TotalMainCategories;
            responseObject.MainCategories.Count().ShouldBe(categoryNumber);
        }
        [Fact]
        public async Task SearchBySearchTerm()
        {
            var response = await _fixture.Client.GetAsync(ENDPOINT + $"?searchterm=food");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<MainCategoryModels.Response>(await response.Content.ReadAsStringAsync());
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            responseObject.MainCategories.Count().ShouldBe(1);
            responseObject.MainCategories.FirstOrDefault()!.Name.ShouldBe("Food");
        }
        [Fact]
        public async Task CreateUpdateDeleteMainCategory()
        {
            //refactor if you use an in memory database in the future
            var testId = 0;
            try
            {
                //create
                var uniqueName = Guid.NewGuid().ToString();
                var request = new AddEditMainCategory() with { Name = uniqueName };
                var response = await _fixture.SendPostRequestAsync(ENDPOINT, request);
                response.StatusCode.ShouldBe(HttpStatusCode.Created);
                var responseObject = JsonConvert.DeserializeObject<AddEditMainCategory>(await response.Content.ReadAsStringAsync());
                testId = responseObject.Id!.Value;
                response.Headers.Location!.ToString().ShouldContain(responseObject.Id.ToString()!);
                responseObject.Name.ShouldBe(uniqueName);
                responseObject.Id.ShouldNotBeNull();
                //update
                var updateObject = new AddEditMainCategory() with
                {
                    Id = testId,
                    Name = Guid.NewGuid().ToString()
                };
                var updateResponse = await _fixture.SendPutRequestAsync(ENDPOINT, updateObject);
                updateResponse.EnsureSuccessStatusCode();
            }
            finally
            {
                //delete
                var deleteResponse = await _fixture.Client.DeleteAsync(ENDPOINT + $"/{testId}");
                deleteResponse.EnsureSuccessStatusCode();
            }

        }
    }
}
