using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using discord_project.Models;
using Discord.Commands;

namespace discord_project.Modules
{
    public class Betting : ModuleBase<SocketCommandContext>
    {
        [Command("bet")]
        public async Task CreateAccumulator(params string[] message)
        {
            OddsBuilder odds = new OddsBuilder();
            Converter converter = new Converter();
            APICalls api = new APICalls();
            List<string> info = new List<string>(message);

            string bettingSite = converter.ConvertSportKeyToNice(info[0]);

            //Remove first entry from list - betting site
            info.RemoveAt(0);

            List<APIData> data = Task.Run(() => api.GetOdds()).Result;
            List<RequestedOdds> allOdds = odds.DisplayOdds(data, bettingSite);

            double accumulatorOdds = odds.CreateAccumulator(allOdds, info);
            await ReplyAsync($"If you put £1 on the accumulator via {bettingSite}, returns will be £{accumulatorOdds} :thinking: (returns may differ by pennies)");

        }

        [Command("siteodds")]
        public async Task GetSelectSiteOddsAsync(string BettingSite)
        {
            OddsBuilder odds = new OddsBuilder();
            Converter converter = new Converter();
            APICalls api = new APICalls();

            List<APIData> data = Task.Run(() => api.GetOdds()).Result;

            BettingSite = converter.ConvertSportKeyToNice(BettingSite);
            List<RequestedOdds> siteOdds = odds.DisplayOdds(data, BettingSite);

            StringBuilder builder = new StringBuilder();

            var tenMatches = siteOdds.Take(10);

            builder.Append($"Odds for {BettingSite} matches (some odds may be like 0.02 wrong): :goal:");

            foreach (var match in tenMatches)
            {
                builder.Append(Environment.NewLine);
                builder.Append($"{match.HomeTeam}: {match.HomeOddsFraction} | ");
                builder.Append($"{match.AwayTeam}: {match.AwayOddsFraction} | ");
                builder.Append($"Draw: {match.DrawOddsFraction}");
            }

            await ReplyAsync(builder.ToString());
        }

        [Command("bestodds")]
        public async Task BestOddsAsync()
        {
            OddsBuilder odds = new OddsBuilder();
            APICalls api = new APICalls();

            List<APIData> data = Task.Run(() => api.GetOdds()).Result;
            List<RequestedOdds> allOdds = odds.DisplayOdds(data, "ALL");

            List<MVPSite> bestOdds = odds.FindBestOdds(allOdds);
            Converter converter = new Converter();

            foreach (var match in bestOdds)
            {
                match.HomeOddsFraction = converter.ConvertDecimalToFractionOdds((float)match.HomeOdds);
                match.AwayOddsFraction = converter.ConvertDecimalToFractionOdds((float)match.AwayOdds);
                match.DrawOddsFraction = converter.ConvertDecimalToFractionOdds((float)match.DrawOdds);
            }

            StringBuilder builder = new StringBuilder();

            builder.Append("Showing the sites who have the best odds: :moneybag:");
            var tenMatches = bestOdds.Take(10);
            foreach (var match in tenMatches)
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
            OddsBuilder odds = new OddsBuilder();
            APICalls api = new APICalls();

            List<APIData> data = Task.Run(() => api.GetOdds()).Result;
            List<RequestedOdds> matches = odds.DisplayOdds(data, "Sky Bet");

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
    }
}