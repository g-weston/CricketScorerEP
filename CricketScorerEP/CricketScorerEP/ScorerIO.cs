using System;
using System.IO;
using System.Collections.Generic;

namespace CricketScorerEP
{
    public class ScorerIO
    {
        static string GetDownloadFilename(string filename)
        {
            var pathFile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            string fileNameWithPath = Path.Combine(pathFile.ToString(), filename);
            return fileNameWithPath;
        }

        public static void PopulateTeamFromFile(int teamNumber)
        {
            string teamNumberWord = "One";
            if (teamNumber == 2)
                teamNumberWord = "Two"; ;

            string filename = "Team" + teamNumberWord + "Definition.txt";
        //    string fileNameWithPath = GetDownloadFilename(filename);
            var pathFile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            string fileNameWithPath = Path.Combine(pathFile.ToString(), filename);
            List<string> fileContents = new List<string>();
            StreamReader file = new StreamReader(fileNameWithPath);

            string line;
            while ((line = file.ReadLine()) != null)
            {
                fileContents.Add(line);
            }
            string clubName = fileContents[0];
            if (teamNumber == 1)
                Match.HomeTeam = clubName;
            else if (teamNumber == 2)
                Match.AwayTeam = clubName;
            int.TryParse(fileContents[1], out int numberOfPlayers);
            string[] lineContents = { };
            string forename, surname, playerName;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                lineContents = fileContents[i + 2].Split(' ');
                int.TryParse(lineContents[0], out int playerId);
                forename = lineContents[1];
                surname = lineContents[2];
                playerName = forename[0].ToString().ToUpper() + ' ' + surname[0].ToString().ToUpper() + surname.Substring(1);
                if (teamNumber == 1)
                    Teams.teamOnePlayers.Add(new Player(clubName, playerId, playerName));
                else if (teamNumber == 2)
                    Teams.teamTwoPlayers.Add(new Player(clubName, playerId, playerName));
            }
            file.Close();
        }

        public static void PopulateTeamFromConsole(int teamNumber)
        {
            string teamName = "one";
            if (teamNumber == 2)
                teamName = "two";

            Console.WriteLine("Please enter the name of team {0}", teamName);
            string clubName = Console.ReadLine();

            Console.WriteLine("Enter the number of players in {0}", clubName);
            string input = Console.ReadLine();
            int.TryParse(input, out int numberOfPlayers);

            string playerName;
            int playerId;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Console.WriteLine("Please enter the name of player {0} from {1}", i + 1, clubName);
                playerName = Console.ReadLine();
                Console.WriteLine("Please enter the id of {0} from {1}", playerName, clubName);
                playerId = int.Parse(Console.ReadLine());
                if (teamNumber == 1)
                    Teams.teamOnePlayers.Add(new Player(clubName, playerId, playerName));
                else if (teamNumber == 2)
                    Teams.teamTwoPlayers.Add(new Player(clubName, playerId, playerName));
            }
        }

        public static void ReadMatchDetails(string filename)
        {
            var pathFile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            string fileNameWithPath = Path.Combine(pathFile.ToString(), filename);

            List<string> fileContents = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(fileNameWithPath);

            string line;
            while ((line = file.ReadLine()) != null)
            {
                fileContents.Add(line);
            }
            Match.Date = DateTime.ParseExact(fileContents[0], "d", System.Globalization.CultureInfo.CurrentCulture);
            Match.Competition = fileContents[1];
            Match.Venue = fileContents[2];
            Match.Format = fileContents[3];
            Match.AgeGroup = fileContents[4];
            int.TryParse(fileContents[5], out int input);
            Match.ScheduledOvers = input;
            int.TryParse(fileContents[6], out input);
            Match.RunsPerWideOrNoBall = input;
            int.TryParse(fileContents[7], out input);
            Match.RebowlDeliveriesFromOver = input;

            file.Close();
        }

        public static void WriteScorecard()
        {
            // Base this on play-cricket scorecard format
            // This code to find the mobile Download folder is used in several places - perhaps extract to a utility method, and also perhaps think about making the location configurable.
            string filename = Match.HomeTeam + "v" + Match.AwayTeam + DateTime.Today.ToString("dd-MM-yyyy") + ".html";
            var pathFile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            string fileNameWithPath = Path.Combine(pathFile.ToString(), filename);

            System.IO.StreamWriter file = new System.IO.StreamWriter(fileNameWithPath);

            file.WriteLine("<html>");
            file.WriteLine("<body>");

            file.WriteLine("<pre>");
            file.WriteLine(Match.Venue);
            // Use HTML <table> with large width for player name and dismissal data, then narrow columns for runs, 4, 6, balls.
            file.WriteLine("<table>");
            file.WriteLine("<tr>");
            file.WriteLine("<th width = 50%>Name</th><th>Runs</th><th>4s</th><th>6s</th><th>Balls</th>");
            file.WriteLine("</tr>");
            for (int i = 0; i < Teams.teamOnePlayers.Count; i++)
            {
                file.WriteLine("<tr>");
                if (Teams.teamOnePlayers[i].IsOut)
                {
                    // WriteBatsmanData()
                    file.WriteLine("<td width=50%>{0}</td><TD>Bowled</TD><TD>{1}</td>,<TD>{2}</td>,<TD>{3}</td>,<TD>{4}</td>,<TD>{5}</td>",
                                                                         Teams.teamOnePlayers[i].Name, Teams.teamTwoPlayers[Teams.teamOnePlayers[i].DismissingBowler].Name,
                                                                         Teams.teamOnePlayers[i].RunsScored, Teams.teamOnePlayers[i].NumberOfFoursScored,
                                                                         Teams.teamOnePlayers[i].NumberOfSixesScored, Teams.teamOnePlayers[i].DeliveriesFaced);
                }

                else
                {
                    file.WriteLine("<td width=50%>{0}</td><TD>Not Out</tD><TD>{1}</td>,<TD>{2}</td>,<TD>{3}</td>,<TD>{4}</td>", Teams.teamOnePlayers[i].Name,
                                                                                                Teams.teamOnePlayers[i].RunsScored, Teams.teamOnePlayers[i].NumberOfFoursScored,
                                                                                                Teams.teamOnePlayers[i].NumberOfSixesScored, Teams.teamOnePlayers[i].DeliveriesFaced);
                }
                file.WriteLine("</tr>");
            }
            file.WriteLine("</table>");
            // Need a fall of wicket section (Innings class needs new members to store that data)
            file.WriteLine("Fall of wicket"); // Needs not out batsman score

            file.WriteLine("<table>");
            file.WriteLine("<tr>");
            file.WriteLine("<th width = 50%>Name</th><th>Overs</th><th>Maidens</th><th>Runs</th><th>Wickets</th>");
            file.WriteLine("</tr>");
            // Bowling analysis - need another member on player to determine the bowling order
            for (int i = 0; i < Teams.teamTwoPlayers.Count; i++)
            {
                file.WriteLine("<tr>");
                if (Teams.teamTwoPlayers[i].NumberOfOversBowled > 0)
                {
                    // WriteBowlerData()
                    file.WriteLine("<td width=50%>{0}</td>Not Out<TD>{1}</td>,<TD>{2}</td>,<TD>{3}</td>,<TD>{4}</td>", Teams.teamTwoPlayers[i].Name, Teams.teamTwoPlayers[i].NumberOfOversBowled,
                                                                        Teams.teamTwoPlayers[i].NumberOfMaidensBowled, Teams.teamTwoPlayers[i].RunsConceded,
                                                                        Teams.teamTwoPlayers[i].NumberOfWicketsTaken);
                }
                file.WriteLine("</tr>");
            }
            file.WriteLine("</table>");

            file.WriteLine("<table>");
            file.WriteLine("<tr>");
            file.WriteLine("<th>Byes</th><th>Legbyes</th><th>Wides</th><th>Noballs</th><th>Total</th>");
            file.WriteLine("</tr>");
            file.WriteLine("<tr>");
            file.WriteLine("<td width=50%>{0}</td><TD>{1}</td>,<TD>{2}</td>,<TD>{3}</td>,<TD>{4}</td>", Innings.Byes, Innings.LegByes, Innings.Wides, Innings.NoBalls,
                                                                                                       (Innings.Byes + Innings.LegByes + Innings.Wides + Innings.NoBalls));
            file.WriteLine("</tr>");
            file.WriteLine("</table>");

            file.WriteLine("Total  {0}", Innings.Runs);
            file.WriteLine("</pre>");

            file.WriteLine("</body>");
            file.WriteLine("</html>");

            file.Close();

            return;
        }

        public static void WriteHTMLScorecard()
        {
            // This will ultimately replace WriteScorecard(), but will be developed in parallel, leaving WriteScorecard() working in the meantime
            StringWriter stringWriter = new StringWriter();
            /*  using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
              {
                  writer.RenderBeginTag(HtmlTextWriterTag.Html);
                  writer.RenderBeginTag(HtmlTextWriterTag.Head);
                  writer.RenderBeginTag(HtmlTextWriterTag.Title);
                  writer.Write(this.Title);
                  writer.RenderEndTag();
                  writer.AddAttribute(HtmlTextWriterAttribute.Href, ServerPath + ResetCssUrl);
                  writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
                  writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
                  writer.RenderBeginTag(HtmlTextWriterTag.Link);
                  writer.RenderEndTag();
                  writer.RenderBeginTag(HtmlTextWriterTag.Body);
                  writer.RenderBeginTag(HtmlTextWriterTag.H1);
                  writer.Write(this.Title);
                  writer.RenderEndTag();
                  writer.RenderBeginTag(HtmlTextWriterTag.H2);
                  writer.Write("Standard Operating Procedure");
                  writer.RenderEndTag();
                  writer.RenderBeginTag(HtmlTextWriterTag.Table);
                  writer.RenderBeginTag(HtmlTextWriterTag.Tr); */
        }
    }
}