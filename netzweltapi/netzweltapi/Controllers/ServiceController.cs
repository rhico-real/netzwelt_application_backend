using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using netzweltapi.Models;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace netzweltapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {

        [HttpGet]
        [Route("getAllTerritories")]
        public async Task<ActionResult<string>> getAllTerritories()
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://netzwelt-devtest.azurewebsites.net/Territories/All");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> login(UserParams userParams)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://netzwelt-devtest.azurewebsites.net/Account/SignIn");
            var userLogin = Newtonsoft.Json.JsonConvert.SerializeObject(userParams);
            request.Content = new StringContent(userLogin, System.Text.Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            //TODO: Map the string into User Model
            return Ok(responseContent);
        }
    }
}
