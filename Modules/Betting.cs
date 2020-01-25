using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using discord_project.Models;

namespace discord_project.Modules
{
    public class Betting: ModuleBase<SocketCommandContext>
    {
        [Command("odds")]
        public async Task OddsAsync()
        {
            List<APIData> data = Task.Run(() => GetOdds()).Result;

            List<RequestedOdds> skyBetOdds = DisplayOdds(data);
            var firstMatch = skyBetOdds[0];
            string returnedString = "Here are the following odds for the next match: " + firstMatch.HomeTeam + " vs " + firstMatch.AwayTeam + ", and if you put £1 on the home team, you will get £" + firstMatch.HomeOdds + " back via " + firstMatch.BettingSite;
            await ReplyAsync(returnedString);
        }

        private async Task<List<APIData>> GetOdds()
        {
	        string apiKey = "";
            
            using var client = new HttpClient();
            var result = await client.GetAsync(apiKey);

            result.EnsureSuccessStatusCode();

            string responseBody = await result.Content.ReadAsStringAsync();
            JObject data = JObject.Parse(responseBody);
            var token = (JArray)data.SelectToken("data");
            var list = new List<APIData>();

            foreach (var item in token)
            {
                list.Add(item.ToObject<APIData>());
            }

            return list;
        }

        private List<RequestedOdds> DisplayOdds(List<APIData> data) {
            var requested = new List<RequestedOdds>();

            foreach (var obj in data)
            {
                var addToList = new RequestedOdds();
                addToList.HomeTeam = obj.home_team;
                string[] teams = new string[2]
                {
                    obj.teams[0],
                    obj.teams[1]
                };

                int homeTeamIndex = Array.IndexOf(teams, addToList.HomeTeam);
                int awayTeamIndex = (homeTeamIndex == 1) ? 0: 1;

                addToList.AwayTeam = teams[awayTeamIndex];

                foreach (var odds in obj.sites)
                {
                    if (odds.site_nice == "Sky Bet") {
                        addToList.BettingSite = odds.site_nice;
                        addToList.HomeOdds = odds.odds.h2h[homeTeamIndex];
                        addToList.AwayOdds = odds.odds.h2h[awayTeamIndex];
                        addToList.DrawOdds = odds.odds.h2h[2];
                    }
                }

                requested.Add(addToList);                
            }


            return requested;
        }
    }
}