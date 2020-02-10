using discord_project.Models;
using System.Collections.Generic;

namespace discord_project.Models
{
    public class APIData
    {
    public string sport_key { get; set; }
    public string sport_nice { get; set; }
    public List<string> teams { get; set; }
    public int commence_time { get; set; }
    public string home_team { get; set; }
    public List<Sites> sites { get; set; }
    public int sites_count { get; set; }
    }
}