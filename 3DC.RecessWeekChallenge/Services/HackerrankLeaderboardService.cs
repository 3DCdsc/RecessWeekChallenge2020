using _3DC.RecessWeekChallenge.Data;
using _3DC.RecessWeekChallenge.Models;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace _3DC.RecessWeekChallenge.Services
{
    public class HackerrankLeaderboardService : BackgroundService
    {
        private int UPDATE_FREQ_SECONDS;

        private HttpClient _httpClient;
        private readonly ILogger<HackerrankLeaderboardService> _logger;
        private readonly IConfiguration _configuration;

        public IServiceProvider Services { get; }

        public HackerrankLeaderboardService(ILogger<HackerrankLeaderboardService> logger, 
            IConfiguration configuration, IServiceProvider services)
        {
            _logger = logger;
            _configuration = configuration;
            Services = services;
            UPDATE_FREQ_SECONDS = int.Parse(_configuration["LeaderBoardUpdateFreq"]);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ASP.NET", "3.1"));
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    HackerrankLeaderboardModel model = await UpdateHackerrankLeaderBoard();
                    HackerrankLeaderboardModel finalModel = await UpdateHackerrankFinalLeaderBoard();
                    await UpdateDatabase(model, finalModel);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Exception from worker {message}", ex.StackTrace);
                }
                finally
                {
                    await Task.Delay(1000 * UPDATE_FREQ_SECONDS, stoppingToken);

                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _httpClient.Dispose();
            return base.StopAsync(cancellationToken);
        }

        private async Task<HackerrankLeaderboardModel> UpdateHackerrankLeaderBoard()
        {
            var result = await _httpClient.GetAsync(_configuration["Urls:HackerrankChallenge"]);

            if (result.IsSuccessStatusCode)
            {
                _logger.LogInformation("Hackerrank Leaderboard obtained. Status Code: {StatusCode}", result.StatusCode);
                HackerrankLeaderboardModel model = JsonConvert
                    .DeserializeObject<HackerrankLeaderboardModel>(await result.Content.ReadAsStringAsync());
                if (model == null)
                {
                    throw new Exception("No response from Hackerrank");
                }
                return model;

            }
            else
            {
                throw new Exception(String.Format("Unable to reach Hackerrank Leaderboard. Status Code: {0} {1}", 
                    result.StatusCode, await result.Content.ReadAsStringAsync()));
                
            }
        }

        private async Task<HackerrankLeaderboardModel> UpdateHackerrankFinalLeaderBoard()
        {
            var result = await _httpClient.GetAsync(_configuration["Urls:HackerrankFinal"]);
            result.EnsureSuccessStatusCode();
            _logger.LogInformation("Hackerrank Final Leaderboard Obtained");
            HackerrankLeaderboardModel model = JsonConvert
                .DeserializeObject<HackerrankLeaderboardModel>(await result.Content.ReadAsStringAsync());
            if (model == null)
            {
                throw new Exception("No response from Hackerrank Final");
            }
            return model;

        }

        private async Task UpdateDatabase(HackerrankLeaderboardModel model, HackerrankLeaderboardModel finalModel)
        {
            List<string> hackerList = model.Models.Select(m => m.Hacker).ToList();
            List<string> finalHackerList = finalModel.Models.Select(m => m.Hacker).ToList();
            using (var scope = Services.CreateScope())
            {
                var scopedContext = scope.ServiceProvider.GetRequiredService<_3DCRecessWeekChallengeContext>();
                scopedContext.LeaderboardRow
                    .Where(row => hackerList.Contains(row.HackerrankUsername))
                    .ToList()
                    .ForEach((user) => {
                        var _m = model.Models
                            .FirstOrDefault(m => m.Hacker == user.HackerrankUsername);
                        if (_m != null)
                        user.HackerrankScore = (int)(_m.Score);
                        });

                scopedContext.LeaderboardRow
                    .Where(row => finalHackerList.Contains(row.HackerrankUsername))
                    .ToList()
                    .ForEach((user) => {
                        var _m = finalModel.Models
                            .FirstOrDefault(m => m.Hacker == user.HackerrankUsername);
                        if (_m != null)
                        {
                            user.HackerrankFinalScore = (int)(_m.Score);
                            user.HackerrankTimeInt = (int)(_m.TimeTaken);
                        }
                    });

                await scopedContext.SaveChangesAsync();
                
            }

        }
    }

}
