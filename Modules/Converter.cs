using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace discord_project.Modules {
    public class Converter : ModuleBase<SocketCommandContext> {
        public string ConvertUnixTimeStamp (int time) {
            DateTime convertUnixTime = new DateTime (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            convertUnixTime = convertUnixTime.AddSeconds (time).ToLocalTime ();

            var formattedTime = convertUnixTime.Date.ToString ("dd/MM/yyy") + " " + convertUnixTime.TimeOfDay.ToString ();
            return formattedTime;
        }

        public string ConvertDecimalToFractionOdds (float odds) {
            int calc = 0;

            while (true) {
                calc++;
                if (odds * calc == (int) (odds * calc)) {
                    break;
                }
            }

            int topFraction = (int) (odds * calc) - calc;
            int bottomFraction = calc;

            return $"{topFraction}/{bottomFraction}";
        }
    }
}