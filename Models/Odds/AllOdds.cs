using System.Collections.Generic;


namespace discord_project.Models
{
    public class AllOdds
    {
        public string BettingSite { get; set; }
        public double HomeOdds { get; set; }
        public double AwayOdds { get; set; }
        public double DrawOdds { get; set; }
        public string HomeOddsFraction { get; set; }
        public string AwayOddsFraction { get; set; }
        public string DrawOddsFraction { get; set; }
    }
}