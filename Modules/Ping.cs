using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace discord_project.Modules {
    public class Ping : ModuleBase<SocketCommandContext> {
        [Command ("ping")]
        public async Task PingAsync () {
            await ReplyAsync ("Hello World");
        }

        [Command ("lamp")]
        public async Task LampAsync () {
            await ReplyAsync ("Lamp is gay");
        }
    }
}