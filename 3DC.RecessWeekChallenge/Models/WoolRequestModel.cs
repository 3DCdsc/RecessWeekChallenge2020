using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _3DC.RecessWeekChallenge.Models
{
    public class WoolRequestModel
    {
        [Required]
        public int NumRedWool { get; set; }
        [Required]
        public int NumGreenWool { get; set; }
        [Required]
        public int NumBlueWool { get; set; }

    }
}
