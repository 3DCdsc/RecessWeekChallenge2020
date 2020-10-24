using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _3DC.RecessWeekChallenge.Models
{
    public class CheckPriceResponse
    {
        [Required]
        public int Price { get; set; }
    }
}
