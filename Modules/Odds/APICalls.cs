using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using discord_project.Models;
using Discord.Commands;
using Newtonsoft.Json.Linq;

namespace discord_project.Modules
{
    public class APICalls : ModuleBase<SocketCommandContext>
    {
        public async Task<List<APIData>> GetOdds()
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