using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _3DC.RecessWeekChallenge.Models;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Composition.Hosting.Core;
using _3DC.RecessWeekChallenge.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Authentication;
using System.Text;

namespace _3DC.RecessWeekChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly _3DCRecessWeekChallengeContext _context;
        private readonly IConfiguration Configuration;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory,
            _3DCRecessWeekChallengeContext context, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = clientFactory.CreateClient("Home");
            _context = context;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ActionName("Check")]
        public async Task<ContentResult> Check(CheckViewModel viewModel)
        {
            if (!ModelState.IsValid || viewModel == null)
            {
                return Content("Make sure you have filled up the fields");
            }

            string url = viewModel.Url;

            var content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                { "num_red_wool", "1" },
                { "num_green_wool", "1" },
                { "num_blue_wool", "1" },

            });
            var uriB = new UriBuilder(url);
            uriB.Path = "/request_price";
            using var httpResponse = await _httpClient.PostAsync(uriB.Uri, content);
            httpResponse.EnsureSuccessStatusCode();
            CheckPriceResponse checkPriceResponse = JsonConvert.DeserializeObject<CheckPriceResponse>(await httpResponse.Content.ReadAsStringAsync());
            if (checkPriceResponse != null && checkPriceResponse.Price == 111)
            {
                var leaderBoardRow = await _context.LeaderboardRow
                    .FirstOrDefaultAsync(m => m.HackerrankUsername == viewModel.Username);

                if (leaderBoardRow == null)
                {
                    return Content("Your application is working great, but are you sure your username is correct?");
                }

                leaderBoardRow.LabScore = int.Parse(Configuration["Scores:Lab"]);
                
                try
                {
                    _context.Update(leaderBoardRow);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException)
                {
                    return Content("Unknown Error occured");
                }

                return Content("Your Lab score has been updated to 200! Multiple submissions will not get you more marks btw");
            }
            else
            {
                return Content("Couldn't reach your application");
            }
            
        }
    }
}
