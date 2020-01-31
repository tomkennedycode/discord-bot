using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using discord_project.Models;
using System.Linq;

namespace discord_project.Modules
{
    public class Betting: ModuleBase<SocketCommandContext>
    {
        [Command("skyodds")]
        public async Task SkyOddsAsync()
        {
            List<APIData> data = Task.Run(() => GetOdds()).Result;
            List<RequestedOdds> skyBetOdds = DisplayOdds(data, "Sky Bet");

            StringBuilder builder = new StringBuilder();

            var tenMatches = skyBetOdds.Take(10);

            builder.Append("Odds for skybet matches (some odds may be like 0.02 wrong): :goal:");

            foreach(var match in tenMatches)
            {
                builder.Append(Environment.NewLine);
                builder.Append($"{match.HomeTeam}: {match.HomeOddsFraction} | ");
                builder.Append($"{match.AwayTeam}: {match.AwayOddsFraction} | ");
                builder.Append($"Draw: {match.DrawOddsFraction}");
            }

            await ReplyAsync(builder.ToString());
        }

        [Command("bestodds")]
        public async Task BestOddsAsync() {
            List<APIData> data = Task.Run(() => GetOdds()).Result;
            List<RequestedOdds> allOdds = DisplayOdds(data, "ALL");

            List<MVPSite> bestOdds = FindBestOdds(allOdds);

            foreach (var match in bestOdds) {
                match.HomeOddsFraction = ConvertDecimalToFractionOdds((float)match.HomeOdds);
                match.AwayOddsFraction = ConvertDecimalToFractionOdds((float)match.AwayOdds);
                match.DrawOddsFraction = ConvertDecimalToFractionOdds((float)match.DrawOdds);
            }

            StringBuilder builder = new StringBuilder();

            builder.Append("Showing the sites who have the best odds: :moneybag:");
            var tenMatches = bestOdds.Take(10);
            foreach(var match in tenMatches)
            {
                builder.Append(Environment.NewLine);
                builder.Append($"{match.HomeTeam}: {match.HomeOddsFraction} - **{match.BettingSiteHome}** | ");
                builder.Append($"{match.AwayTeam}: {match.AwayOddsFraction} - **{match.BettingSiteAway}** | ");
                builder.Append($"Draw: {match.DrawOddsFraction} - **{match.BettingSiteDraw}**");
            }

            await ReplyAsync(builder.ToString());

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
                builder.Append($"{match.HomeTeam} vs {match.AwayTeam}  @  {match.MatchDate}");
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

                    List<AllOdds> allOdds = new List<AllOdds>();

                    foreach (var odds in obj.sites)
                    {
                        if (odds.site_nice == BettingSite)
                        {
                            addToList.BettingSite = odds.site_nice;
                            addToList.HomeOdds = odds.odds.h2h[homeTeamIndex];
                            addToList.AwayOdds = odds.odds.h2h[awayTeamIndex];
                            addToList.DrawOdds = odds.odds.h2h[2];
                        } else if (BettingSite == "ALL") 
                        {
                            var addToAllOdds = new AllOdds();
                            addToAllOdds.BettingSite = odds.site_nice;
                            addToAllOdds.HomeOdds = odds.odds.h2h[homeTeamIndex];
                            addToAllOdds.AwayOdds = odds.odds.h2h[awayTeamIndex];
                            addToAllOdds.DrawOdds = odds.odds.h2h[2];

                            allOdds.Add(addToAllOdds);
                        }

                        addToList.AllOdds = allOdds;
                    }


                    addToList.HomeOddsFraction = ConvertDecimalToFractionOdds((float)addToList.HomeOdds);
                    addToList.AwayOddsFraction = ConvertDecimalToFractionOdds((float)addToList.AwayOdds);
                    addToList.DrawOddsFraction = ConvertDecimalToFractionOdds((float)addToList.DrawOdds);

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

        private List<MVPSite> FindBestOdds(List<RequestedOdds> data)
        {
            List<MVPSite> list = new List<MVPSite>();
            foreach (var games in data) 
            {
                MVPSite mvpSite = new MVPSite();

                mvpSite.HomeTeam = games.HomeTeam;
                mvpSite.AwayTeam = games.AwayTeam;

                double homeOddsMaxNumber = 0;
                string bestBettingSiteHome = String.Empty;

                double awayOddsMaxNumber = 0;
                string bestBettingSiteAway = String.Empty;

                double drawOddsMaxNumber = 0;
                string bestBettingSiteDraw = String.Empty;

                foreach (var odds in games.AllOdds) 
                {
 
                    if (odds.HomeOdds > homeOddsMaxNumber)
                    {
                        homeOddsMaxNumber = odds.HomeOdds;
                        bestBettingSiteHome = odds.BettingSite;
                    }

                    if (odds.AwayOdds > awayOddsMaxNumber)
                    {
                        awayOddsMaxNumber = odds.AwayOdds;
                        bestBettingSiteAway = odds.BettingSite;
                    }

                    if (odds.DrawOdds > drawOddsMaxNumber)
                    {
                        drawOddsMaxNumber = odds.DrawOdds;
                        bestBettingSiteDraw = odds.BettingSite;
                    }
                }
                mvpSite.BettingSiteHome = bestBettingSiteHome;
                mvpSite.HomeOdds = homeOddsMaxNumber;

                mvpSite.BettingSiteAway = bestBettingSiteAway;
                mvpSite.AwayOdds = awayOddsMaxNumber;

                mvpSite.BettingSiteDraw = bestBettingSiteDraw;
                mvpSite.DrawOdds = drawOddsMaxNumber;

                list.Add(mvpSite);
            }

            return list;
        }

        private string ConvertUnixTimeStamp(int time) 
        {
            DateTime convertUnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            convertUnixTime = convertUnixTime.AddSeconds(time).ToLocalTime();

            var formattedTime = convertUnixTime.Date.ToString("dd/MM/yyy") + " " + convertUnixTime.TimeOfDay.ToString();
            return formattedTime;
        }

        private string ConvertDecimalToFractionOdds(float odds)
        {
            int calc = 0;

            while(true) {
                calc++;
                if (odds * calc == (int) (odds * calc))
                {
                    break;
                }
            }

            int topFraction = (int)(odds * calc) - calc;
            int bottomFraction = calc;

            return $"{topFraction}/{bottomFraction}";
        }
    }
}