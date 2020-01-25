using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using discord_project.Models;
using System.Linq;
using Discord.WebSocket;
using System.Reflection;

namespace discord_project.Modules
{
    public class Betting: ModuleBase<SocketCommandContext>
    {
        [Command("skyodds")]
        public async Task SkyOddsAsync()
        {
            List<APIData> data = Task.Run(() => GetOdds()).Result;
            List<RequestedOdds> skyBetOdds = DisplayOdds(data, "Sky Bet");

            //await ReplyAsync(returnedString);
        }

        [Command("matches")]
        public async Task GetMatchesAsync()
        {
            List<APIData> data = Task.Run(() => GetOdds()).Result;
            List<RequestedOdds> matches = DisplayOdds(data, "Sky Bet");

            StringBuilder builder = new StringBuilder();

            builder.Append("The next 10 upcoming premier league games are: :clap:");

            var tenMatches = matches.Take(10);

            foreach (var match in tenMatches)
            {
                builder.Append(Environment.NewLine);
                builder.Append(match.HomeTeam + " vs " + match.AwayTeam + " @ " + match.MatchDate);
            }

            await ReplyAsync(builder.ToString());
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

        private List<RequestedOdds> DisplayOdds(List<APIData> data, string BettingSite) {
            
            var requested = new List<RequestedOdds>();

            try {

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
                    addToList.MatchDate = ConvertUnixTimeStamp(obj.commence_time);

                    foreach (var odds in obj.sites)
                    {
                        if (odds.site_nice == BettingSite) {
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return requested;            
        }

        private string ConvertUnixTimeStamp(int time) 
        {
            DateTime convertUnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            convertUnixTime = convertUnixTime.AddSeconds(time).ToLocalTime();

            var formattedTime = convertUnixTime.Date.ToString("dd/MM/yyy") + " " + convertUnixTime.TimeOfDay.ToString();
            return formattedTime;
        }
    }
}