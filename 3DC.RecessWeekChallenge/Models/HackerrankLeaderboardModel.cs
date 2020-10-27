using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _3DC.RecessWeekChallenge.Models
{
    public class HackerrankLeaderboardModel
    {
        [Required]
        public HackerObj[] Models { get; set; }

        public class HackerObj
        {
            [Required]
            public String Hacker { get; set; }

            [Required]
            public float Score { get; set; }
        }
    }
}
