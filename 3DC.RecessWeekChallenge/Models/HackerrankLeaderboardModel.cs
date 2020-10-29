using Newtonsoft.Json;
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
            [Key]
            [Required]
            public String Hacker { get; set; }

            [Required]
            public float Score { get; set; }

            [JsonProperty(PropertyName="time_taken")]
            public float TimeTaken { get; set; }
        }
    }
}
