using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using netzweltapi.Models;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            var resultMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);

            List<TerritoryData>? res = JsonConvert.DeserializeObject<List<TerritoryData>>(resultMap["data"].ToString());

            // Create a dictionary to store the data items by their ID
            Dictionary<string, TerritoryData> dataDictionary = new Dictionary<string, TerritoryData>();

            if(res != null)
            {
                // Add the data items to the dictionary using their ID as the key
                foreach (TerritoryData dataItem in res)
                {
                    dataDictionary[dataItem.id] = dataItem;
                }

                // Create a list to store the final data with parent-child relationship
                List<TerritoryData> finalList = new List<TerritoryData>();

                // Iterate over the data items and assign parent-child relationships
                foreach (TerritoryData dataItem in res)
                {
                    if (dataItem.parent != null && dataDictionary.ContainsKey(dataItem.parent))
                    {
                        TerritoryData parentItem = dataDictionary[dataItem.parent];
                        parentItem.children ??= new List<TerritoryData>();
                        parentItem.children.Add(dataItem);
                    }
                    else
                    {
                        finalList.Add(dataItem);
                    }
                }

                Data data = new Data();
                data.data = finalList;

                // return Ok(JsonConvert.SerializeObject(data));
                return Ok(finalList);
            }

            return Ok();
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
