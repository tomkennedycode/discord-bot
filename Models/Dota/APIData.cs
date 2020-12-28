using discord_project.Models;
using System.Collections.Generic;

namespace discord_project.Models.Dota
{
    public class APIData
    {
        public string id { get; set; }
        public string name { get; set; }
        public int cost { get; set; }
        public bool secret_shop { get; set; }
        public bool side_shop { get; set; }
        public bool recipe { get; set; }
        public string localized_name { get; set; }
    }
}