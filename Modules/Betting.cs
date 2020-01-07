using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Discord.Commands;
using Newtonsoft.Json;

namespace discord_project.Modules
{
    public class Betting: ModuleBase<SocketCommandContext>
    {
        [Command("odds")]
        public async Task OddsAsync()
        {
            GetOdds();
            //await ReplyAsync("Here are the following odds");
        }

        private async void GetOdds()
        {
            string apiKey = "";
            
            using var client = new HttpClient();

            var result = await client.GetAsync(apiKey);

            var data = result.Content.ReadAsStringAsync().Result;

            dynamic dataObj = JsonConvert.DeserializeObject(data);
        
            var test = dataObj.sport_key;
        
        }
    }
}