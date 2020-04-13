using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace discord_project.Modules {
    public class Help : ModuleBase<SocketCommandContext> {

        [Command ("lamp")]
        public async Task LampAsync () {
            await ReplyAsync ("Lamp isn't cool");
        }

        [Command ("commandhelp")]
        public async Task HelpAsync () {
            await ReplyAsync("no help");
        }

        [Command ("bettingsitehelp")]
        public async Task SiteHelp() {
            await ReplyAsync("no help");
        }
    }
}