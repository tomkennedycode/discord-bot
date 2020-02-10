using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using discord_project.Models;
using Discord.Commands;

namespace discord_project.Modules {
    public class Commands : ModuleBase<SocketCommandContext> {
        [Command ("results")]
        public async Task GetPreviousResults()
        {
            Scraper scrape = new Scraper();
            string stuff = Task.Run(() => scrape.GetData()).Result;

            await ReplyAsync("");

        }

    }
}