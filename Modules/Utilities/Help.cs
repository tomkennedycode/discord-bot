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
            await ReplyAsync("");
        }

        [Command("utilityhelp")]
        public async Task UtilityHelpAsync()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("> ");
            await ReplyAsync("no help");
        }

        [Command("oddshelp")]
        public async Task OddsHelpAsync()
        {
            await ReplyAsync("no help");
        }
    }
}