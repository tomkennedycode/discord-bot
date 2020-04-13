using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace discord_project.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {

        [Command("lamp")]
        public async Task LampAsync()
        {
            await ReplyAsync("Lamp isn't cool");
        }

        [Command("help")]
        public async Task HelpAsync()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("> :speech_balloon: To get help on utilities, type __**bot!utilityhelp**__");
            builder.Append(Environment.NewLine);
            builder.Append("> :money_mouth: To get help on retrieving odds data, type __**bot!oddshelp**__");
            await ReplyAsync(builder.ToString());
        }

        [Command("utilityhelp")]
        public async Task UtilityHelpAsync()
        {
            StringBuilder builder = new StringBuilder();
            //Timer help
            builder.Append("> :timer: To set a timer, type __**bot!settimer**__ followed by a space and number of minutes. This command only supports minutes at the moment.");
            //
            await ReplyAsync(builder.ToString());
        }

        [Command("oddshelp")]
        public async Task OddsHelpAsync()
        {
            StringBuilder builder = new StringBuilder();
            //AccumulatorBuilder
            builder.Append(">>> :moneybag: To create an accumulator, type __**bot!bet**__ followed by a space and the name of the betting site you wish to use. After that you will type the teams you wish to create the accumulator with, each having a space between them.");
            builder.Append(Environment.NewLine);

            //Site Odds
            builder.Append(":money_with_wings: To check the odds on a specific site, type __**bot!siteodds**__ followed by a space and the name of the betting site you wish to use. Make sure to keep it as one word e.g. skybet");
            builder.Append(Environment.NewLine);

            //Best Odds
            builder.Append(":first_place: To check which site has the best odds, type __**bot!bestodds**__");
            builder.Append(Environment.NewLine);

            //Get Matches
            builder.Append(":volleyball: To get a list of the next 10 upcoming matches, type __**bot!matches**__");

            await ReplyAsync(builder.ToString());
        }
    }
}