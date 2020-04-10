using System;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;

namespace discord_project.Modules {
    public class Utilities : ModuleBase<SocketCommandContext> {

        [Command ("settimer")]
        public async Task SetLampTimer(int timer) {

            double milliseconds = TimeSpan.FromMinutes(timer).TotalMilliseconds;
            System.Timers.Timer timer1 = new System.Timers.Timer {
                Interval = milliseconds,
                AutoReset = false
            };
            var timerEnd = DateTime.Now.AddMilliseconds(milliseconds);
            timer1.Start();
            await ReplyAsync($"Timer Started - Will end at {timerEnd} :timer:");

            while (timer1.Enabled) {
                
            }

            await ReplyAsync("Times up lamp");
        }
    }
}