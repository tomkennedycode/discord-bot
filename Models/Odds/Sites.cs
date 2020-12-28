namespace discord_project.Models.Odds
{
    public class Sites
    {
        public string site_key { get; set; }
        public string site_nice { get; set; }
        public int last_update { get; set; }
        public Odds odds { get; set; }
    }
}