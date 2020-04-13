using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;

namespace discord_project.Modules {
    public class Utilities : ModuleBase<SocketCommandContext> {

        [Command ("settimer")]
        public async Task SetLampTimer(int timer) {
            try
            {
                double milliseconds = TimeSpan.FromMinutes(timer).TotalMilliseconds;
                System.Timers.Timer timer1 = new System.Timers.Timer
                {
                    Interval = milliseconds,
                    AutoReset = false
                };
                var timerEnd = DateTime.Now.AddMilliseconds(milliseconds);
                new Thread(() =>
                {
                    timer1.Start();
                    ReplyAsync($"Timer Started - Will end at {timerEnd} :timer:");

                    while (timer1.Enabled)
                    {

                    }
                    ReplyAsync("Times up lamp");
                }).Start();
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
                throw;
            }            
        }
    }
}