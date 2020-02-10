using System;
using System.Net;
using System.Threading.Tasks;
using discord_project.Models;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using System.Linq;

namespace discord_project.Modules {
    public class Scraper : ModuleBase<SocketCommandContext>
    {
        public async Task<string> GetData()
        {
            Uri uri = new Uri("https://www.flashscore.com/football/england/premier-league/results/");
            string str;
            using(WebClient client = new WebClient())
            {
                str = await client.DownloadStringTaskAsync(uri);
            }

            HtmlDocument html = new HtmlDocument();

            var scores = html.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("soccer")).ToList();

            var list = scores[0].Descendants();

            //good tut
            //https://www.youtube.com/watch?v=B4x4pnLYMWI
            //13.24

            return str;
        }
    }
}