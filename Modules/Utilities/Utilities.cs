using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;

namespace discord_project.Modules
{
    public class Utilities : ModuleBase<SocketCommandContext>
    {

        [Command("settimer")]
        public async Task SetTimer(int timer)
        {
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

        [Command("flipcoin")]
        public async Task FlipCoin()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            //Random 1000 times
            string flipped1000 = String.Join("\n", Enumerable.Repeat(0, 1000).Select(i => random.Next(0, 2) == 1 ? "Tails" : "Heads"));

            //Count how many times each won
            int heads = flipped1000.Count(i => i == 'H');
            int tails = flipped1000.Count(i => i == 'T');

            await ReplyAsync((heads > tails ? "**Heads**" : "**Tails**") + " is the winner!");
        }
    }
}