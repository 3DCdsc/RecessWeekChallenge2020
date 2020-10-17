using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _3DC.RecessWeekChallenge.Models
{
    public class LeaderboardRow
    {
        [Required]
        [Display(Name = "idName", ResourceType = typeof(Resources.Leaderboard))]
        public int Id { get; set; }

        [Required]
        [Display(Name = "nameName", ResourceType = typeof(Resources.Leaderboard))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "hackerrankUsernameName", ResourceType = typeof(Resources.Leaderboard))]
        public string HackerrankUsername { get; set; }

        [Required]
        [Display(Name = "labScoreName", ResourceType = typeof(Resources.Leaderboard))]
        public int LabScore { get; set; }

        [Required]
        [Display(Name = "hackerrankScoreName", ResourceType = typeof(Resources.Leaderboard))]
        public int HackerrankScore { get; set; }

        [NotMapped]
        [Display(Name = "totalScoreName", ResourceType = typeof(Resources.Leaderboard))]
        public int TotalScore { get => HackerrankScore + LabScore; }

        [NotMapped]
        [Display(Name = "rankName", ResourceType = typeof(Resources.Leaderboard))]
        public int Rank { get; set; }
    }
}
