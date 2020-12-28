using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using discord_project.Models.Dota;
using Discord.Commands;

namespace discord_project.Modules.Dota
{
    public class Dota : ModuleBase<SocketCommandContext>
    {
        [Command("item")]
        public async Task GetItemsAsync()
        {
            APICalls api = new APICalls();

            List<APIData> data = Task.Run(() => api.GetDotaItems()).Result;
            Console.WriteLine(data);

            await ReplyAsync("");
            
            // StringBuilder builder = new StringBuilder();

            // builder.Append("The next 10 upcoming premier league games are: :clap:");

            // var tenMatches = matches.Take(10);

            // foreach (var match in tenMatches)
            // {
            //     builder.Append(Environment.NewLine);
            //     builder.Append($"{match.HomeTeam} vs {match.AwayTeam}  @  {match.MatchDate}");
            // }

            // await ReplyAsync(builder.ToString());
        }
    }
}