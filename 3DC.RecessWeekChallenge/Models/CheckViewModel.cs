using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _3DC.RecessWeekChallenge.Models
{
    public class CheckViewModel
    {
        [Required]
        public string Url { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
