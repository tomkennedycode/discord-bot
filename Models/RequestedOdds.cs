namespace discord_project.Models
{
    public class RequestedOdds
    {
        public string BettingSite { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public double HomeOdds { get; set; }
        public double AwayOdds { get; set; }
        public double DrawOdds { get; set; }
    }
}