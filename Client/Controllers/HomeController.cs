using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44386");
            if (disco.IsError)
            {
                return BadRequest(disco.Error);
            }


            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync
                (new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,

                    ClientId = "client",
                    ClientSecret = "secret",
                    Scope = "api"
                });

            if (tokenResponse.IsError)
            {
                return BadRequest(tokenResponse.Error);
            }



            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:44333/identity");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(new { tokenResponse.AccessToken, content });
            }

        }
    }
}
