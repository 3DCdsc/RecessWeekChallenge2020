using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _3DC.RecessWeekChallenge.Models;

namespace _3DC.RecessWeekChallenge.Data
{
    public class _3DCRecessWeekChallengeContext : DbContext
    {
        public _3DCRecessWeekChallengeContext (DbContextOptions<_3DCRecessWeekChallengeContext> options)
            : base(options)
        {
        }

        public DbSet<_3DC.RecessWeekChallenge.Models.LeaderboardRow> LeaderboardRow { get; set; }
    }
}
