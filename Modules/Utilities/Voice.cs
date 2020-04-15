using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Audio;
using Discord;
using System.Collections.Concurrent;

namespace discord_project.Modules
{
    public class Voice : ModuleBase<SocketCommandContext>
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinConnectedChannel()
        {
            await JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
            Console.WriteLine("test");
        }

        private async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            try
            {
                IAudioClient client;
                if (ConnectedChannels.TryGetValue(guild.Id, out client))
                {
                    return;
                }
                if (target.Guild.Id != guild.Id)
                {
                    return;
                }

                var audioClient = await target.ConnectAsync();
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
                throw;
            }
            
        }

        [Command("leave")]
        public async Task LeaveConnectedChannel()
        {
            await LeaveAudio(Context.Guild);
        }

        private async Task LeaveAudio(IGuild guild)
        {
            IAudioClient client;
            if (ConnectedChannels.TryRemove(guild.Id, out client))
            {
                await client.StopAsync();
            }
        }
    }
}