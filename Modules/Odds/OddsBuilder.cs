using System;
using System.Collections.Generic;
using System.Diagnostics;
using discord_project.Models.Odds;
using Discord.Commands;

namespace discord_project.Modules.Odds
{
    public class OddsBuilder : ModuleBase<SocketCommandContext>
    {
        public List<RequestedOdds> DisplayOdds(List<APIData> data, string BettingSite)
        {
            var requested = new List<RequestedOdds>();

            try
            {
                Converter converter = new Converter();

                foreach (var obj in data)
                {
                    var addToList = new RequestedOdds();
                    addToList.HomeTeam = obj.home_team;
                    string[] teams = new string[2] {
                        obj.teams[0],
                        obj.teams[1]
                    };

                    int homeTeamIndex = Array.IndexOf(teams, addToList.HomeTeam);
                    int awayTeamIndex = (homeTeamIndex == 1) ? 0 : 1;

                    addToList.AwayTeam = teams[awayTeamIndex];
                    addToList.MatchDate = converter.ConvertUnixTimeStamp(obj.commence_time);

                    List<AllOdds> allOdds = new List<AllOdds>();

                    foreach (var odds in obj.sites)
                    {
                        if (odds.site_nice == BettingSite)
                        {
                            addToList.BettingSite = odds.site_nice;
                            addToList.HomeOdds = odds.odds.h2h[homeTeamIndex];
                            addToList.AwayOdds = odds.odds.h2h[awayTeamIndex];
                            addToList.DrawOdds = odds.odds.h2h[2];
                        }
                        else if (BettingSite == "ALL")
                        {
                            var addToAllOdds = new AllOdds();
                            addToAllOdds.BettingSite = odds.site_nice;
                            addToAllOdds.HomeOdds = odds.odds.h2h[homeTeamIndex];
                            addToAllOdds.AwayOdds = odds.odds.h2h[awayTeamIndex];
                            addToAllOdds.DrawOdds = odds.odds.h2h[2];

                            allOdds.Add(addToAllOdds);
                        }

                        addToList.AllOdds = allOdds;
                    }

                    addToList.HomeOddsFraction = converter.ConvertDecimalToFractionOdds((float)addToList.HomeOdds);
                    addToList.AwayOddsFraction = converter.ConvertDecimalToFractionOdds((float)addToList.AwayOdds);
                    addToList.DrawOddsFraction = converter.ConvertDecimalToFractionOdds((float)addToList.DrawOdds);

                    requested.Add(addToList);
                }

                return requested;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return requested;
        }

        public List<MVPSite> FindBestOdds(List<RequestedOdds> data)
        {
            List<MVPSite> list = new List<MVPSite>();
            foreach (var games in data)
            {
                MVPSite mvpSite = new MVPSite();

                mvpSite.HomeTeam = games.HomeTeam;
                mvpSite.AwayTeam = games.AwayTeam;

                double homeOddsMaxNumber = 0;
                string bestBettingSiteHome = String.Empty;

                double awayOddsMaxNumber = 0;
                string bestBettingSiteAway = String.Empty;

                double drawOddsMaxNumber = 0;
                string bestBettingSiteDraw = String.Empty;

                foreach (var odds in games.AllOdds)
                {

                    if (odds.HomeOdds > homeOddsMaxNumber)
                    {
                        homeOddsMaxNumber = odds.HomeOdds;
                        bestBettingSiteHome = odds.BettingSite;
                    }

                    if (odds.AwayOdds > awayOddsMaxNumber)
                    {
                        awayOddsMaxNumber = odds.AwayOdds;
                        bestBettingSiteAway = odds.BettingSite;
                    }

                    if (odds.DrawOdds > drawOddsMaxNumber)
                    {
                        drawOddsMaxNumber = odds.DrawOdds;
                        bestBettingSiteDraw = odds.BettingSite;
                    }
                }
                mvpSite.BettingSiteHome = bestBettingSiteHome;
                mvpSite.HomeOdds = homeOddsMaxNumber;

                mvpSite.BettingSiteAway = bestBettingSiteAway;
                mvpSite.AwayOdds = awayOddsMaxNumber;

                mvpSite.BettingSiteDraw = bestBettingSiteDraw;
                mvpSite.DrawOdds = drawOddsMaxNumber;

                list.Add(mvpSite);
            }

            return list;
        }

        public double CreateAccumulator(List<RequestedOdds> AllOdds, List<String> teams)
        {

            double accumulatorTotal = 1;
            try
            {

                Converter convert = new Converter();

                List<RequestedOdds> teamsOdds = new List<RequestedOdds>();

                for (int i = 0; i < teams.Count; i++)
                {
                    //Make first letter uppercase
                    teams[i] = convert.ConvertToUpperCase(teams[i]);

                    //Get object from list of the teams in the Teamslist
                    var oddsFromRequestedTeams = AllOdds.Find(s => s.HomeTeam.Contains(teams[i]) || s.AwayTeam.Contains(teams[i]));

                    teamsOdds.Add(oddsFromRequestedTeams);
                }

                var oddsList = new List<double>();

                for (int i = 0; i < teams.Count; i++)
                {
                    if (teamsOdds[i].HomeTeam.Contains(teams[i]))
                    {
                        oddsList.Add(teamsOdds[i].HomeOdds);
                    }
                    else
                    {
                        oddsList.Add(teamsOdds[i].AwayOdds);
                    }
                }

                for (int i = 0; i < oddsList.Count; i++)
                {
                    accumulatorTotal *= oddsList[i];
                }

                accumulatorTotal = Math.Round(accumulatorTotal, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ReplyAsync(ex.Message);
            }

            return accumulatorTotal;

        }
    }
}