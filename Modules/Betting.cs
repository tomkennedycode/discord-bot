using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using discord_project.Models;

namespace discord_project.Modules
{
    public class Betting: ModuleBase<SocketCommandContext>
    {
        [Command("odds")]
        public async Task OddsAsync()
        {
            List<APIData> data = Task.Run(() => GetOdds()).Result;
            //await ReplyAsync("Here are the following odds");
        }

        private async Task<List<APIData>> GetOdds()
        {
            string apiKey = "";
            
            using var client = new HttpClient();
            var result = await client.GetAsync(apiKey);

            result.EnsureSuccessStatusCode();

            string responseBody = await result.Content.ReadAsStringAsync();
            JObject data = JObject.Parse(responseBody);
            var token = (JArray)data.SelectToken("data");
            var list = new List<APIData>();

            foreach (var item in token)
            {
                list.Add(item.ToObject<APIData>());
            }

            return list;
        }
    }
}