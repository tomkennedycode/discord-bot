using System.Collections.Generic;


namespace discord_project.Models
{
    public class Odds
    {
        public List<double> h2h { get; set; }
        public List<double?> h2h_lay { get; set; }
    }
}