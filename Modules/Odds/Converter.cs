using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace discord_project.Modules.Odds
{
    public class Converter : ModuleBase<SocketCommandContext>
    {
        public string ConvertUnixTimeStamp(int time)
        {
            DateTime convertUnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            convertUnixTime = convertUnixTime.AddSeconds(time).ToLocalTime();

            var formattedTime = convertUnixTime.Date.ToString("dd/MM/yyy") + " " + convertUnixTime.TimeOfDay.ToString();
            return formattedTime;
        }

        public string ConvertDecimalToFractionOdds(float odds)
        {
            int calc = 0;

            while (true)
            {
                calc++;
                if (odds * calc == (int)(odds * calc))
                {
                    break;
                }
            }

            int topFraction = (int)(odds * calc) - calc;
            int bottomFraction = calc;

            return $"{topFraction}/{bottomFraction}";
        }

        public string ConvertSportKeyToNice(string key)
        {
            switch (key)
            {
                case "skybet":
                    return "Sky Bet";
                case "betfred":
                    return "Betfred";
                case "williamhill":
                    return "William Hill";
                case "paddypower":
                    return "Paddy Power";
                case "ladbrokes":
                    return "Ladbrokes";
                case "betfair":
                    return "Betfair";
                case "marathonbet":
                    return "Marathon Bet";
                case "betvictor":
                    return "Bet Victor";
                case "unibet":
                    return "Unibet";
                case "sport888":
                    return "888sport";
                case "matchbook":
                    return "Matchbook";
                case "onexbet":
                    return "1xBet";
                default:
                    return "Sky Bet";
            }
        }

        public string ConvertToUpperCase(string name)
        {
            return char.ToUpper(name[0]) + name.Substring(1);
        }
    }
}