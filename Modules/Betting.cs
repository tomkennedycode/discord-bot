using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using discord_project.Models;
using Discord.Commands;

namespace discord_project.Modules {
    public class Betting : ModuleBase<SocketCommandContext> {
        [Command ("Bet")]
        public async Task CreateAccumulator (params string[] message) {
            OddsBuilder odds = new OddsBuilder();
            var bettingSite = message[0];

            List<APIData> data = Task.Run (() => odds.GetOdds ()).Result;

        }

        [Command ("skyodds")]
        public async Task SkyOddsAsync () {
            OddsBuilder odds = new OddsBuilder();

            List<APIData> data = Task.Run (() => odds.GetOdds ()).Result;
            List<RequestedOdds> skyBetOdds = odds.DisplayOdds (data, "Sky Bet");

            StringBuilder builder = new StringBuilder ();

            var tenMatches = skyBetOdds.Take (10);

            builder.Append ("Odds for skybet matches (some odds may be like 0.02 wrong): :goal:");

            foreach (var match in tenMatches) {
                builder.Append (Environment.NewLine);
                builder.Append ($"{match.HomeTeam}: {match.HomeOddsFraction} | ");
                builder.Append ($"{match.AwayTeam}: {match.AwayOddsFraction} | ");
                builder.Append ($"Draw: {match.DrawOddsFraction}");
            }

            await ReplyAsync (builder.ToString ());
        }

        [Command ("bestodds")]
        public async Task BestOddsAsync () {
            OddsBuilder odds = new OddsBuilder();

            List<APIData> data = Task.Run (() => odds.GetOdds ()).Result;
            List<RequestedOdds> allOdds = odds.DisplayOdds (data, "ALL");

            List<MVPSite> bestOdds = odds.FindBestOdds(allOdds);
            Converter converter = new Converter();


            foreach (var match in bestOdds) {
                match.HomeOddsFraction = converter.ConvertDecimalToFractionOdds ((float) match.HomeOdds);
                match.AwayOddsFraction = converter.ConvertDecimalToFractionOdds ((float) match.AwayOdds);
                match.DrawOddsFraction = converter.ConvertDecimalToFractionOdds ((float) match.DrawOdds);
            }

            StringBuilder builder = new StringBuilder ();

            builder.Append ("Showing the sites who have the best odds: :moneybag:");
            var tenMatches = bestOdds.Take (10);
            foreach (var match in tenMatches) {
                builder.Append (Environment.NewLine);
                builder.Append ($"{match.HomeTeam}: {match.HomeOddsFraction} - **{match.BettingSiteHome}** | ");
                builder.Append ($"{match.AwayTeam}: {match.AwayOddsFraction} - **{match.BettingSiteAway}** | ");
                builder.Append ($"Draw: {match.DrawOddsFraction} - **{match.BettingSiteDraw}**");
            }

            await ReplyAsync (builder.ToString ());

        }

        [Command ("matches")]
        public async Task GetMatchesAsync () {
            OddsBuilder odds = new OddsBuilder();

            List<APIData> data = Task.Run (() => odds.GetOdds ()).Result;
            List<RequestedOdds> matches = odds.DisplayOdds (data, "Sky Bet");

            StringBuilder builder = new StringBuilder ();

            builder.Append ("The next 10 upcoming premier league games are: :clap:");

            var tenMatches = matches.Take (10);

            foreach (var match in tenMatches) {
                builder.Append (Environment.NewLine);
                builder.Append ($"{match.HomeTeam} vs {match.AwayTeam}  @  {match.MatchDate}");
            }

            await ReplyAsync (builder.ToString ());
        }
    }
}