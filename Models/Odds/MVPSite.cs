using System.Collections.Generic;


namespace discord_project.Models.Odds
{
    public class MVPSite
    {
        public string BettingSiteHome { get; set; }
        public string BettingSiteAway { get; set; }
        public string BettingSiteDraw { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public double HomeOdds { get; set; }
        public double AwayOdds { get; set; }
        public double DrawOdds { get; set; }
        public string HomeOddsFraction { get; set; }
        public string AwayOddsFraction { get; set; }
        public string DrawOddsFraction { get; set; }
    }
}