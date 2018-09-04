using System;
using System.IO;
using System.Xml;
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
            string forename, surname, playerName, fullName;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                lineContents = fileContents[i + 2].Split(' ');
                int.TryParse(lineContents[0], out int playerId);
                forename = lineContents[1];
                surname = lineContents[2];
                playerName = forename[0].ToString().ToUpper() + ' ' + surname[0].ToString().ToUpper() + surname.Substring(1);  // Initial and surname, initial caps
                fullName = forename[0].ToString().ToUpper() + forename.Substring(1) + ' ' + surname[0].ToString().ToUpper() + surname.Substring(1);  // Forename and surname, initial caps
                if (teamNumber == 1)
                    Teams.teamOnePlayers.Add(new Player(clubName, playerId, playerName, fullName));
                else if (teamNumber == 2)
                    Teams.teamTwoPlayers.Add(new Player(clubName, playerId, playerName, fullName));
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
                playerName = Console.ReadLine(); // TODO split this into forename & surname, extract initial caps as for "from file" method above
                Console.WriteLine("Please enter the id of {0} from {1}", playerName, clubName);
                playerId = int.Parse(Console.ReadLine());
                if (teamNumber == 1)
                    Teams.teamOnePlayers.Add(new Player(clubName, playerId, playerName, playerName));
                else if (teamNumber == 2)
                    Teams.teamTwoPlayers.Add(new Player(clubName, playerId, playerName, playerName));
            }
        }

        public static void ReadMatchDetails(string filename)
        {
            // TODO guard / try/catch around this if file does not exist, or insufficient permissions granted
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
            string filename = Match.HomeTeam + "_vs_" + Match.AwayTeam + "_" + DateTime.Today.ToString("dd-MM-yyyy") + ".html";
            var pathFile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            string fileNameWithPath = Path.Combine(pathFile.ToString(), filename);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileNameWithPath))
            {
                file.AutoFlush = true;

                file.WriteLine("<html>");
                file.WriteLine("<body>");

                file.WriteLine("<H3>" + Match.HomeTeam + " vs " + Match.AwayTeam + "</H3>");
                file.WriteLine("<H4>Ground: " + Match.Venue + "</H4>");
                file.WriteLine("<H4>Date: " + DateTime.Today.ToString("D", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")) + "</H4>");
                // Use HTML <table> with large width for player name and dismissal data, then narrow columns for runs, 4, 6, balls.
                file.WriteLine("<table>");
                file.WriteLine("<tr><th>&nbsp;</th><th width = 25%>Name</th><th width = 25%>How Out</th><th width = 25%>Bowler</th><th>Runs</th><th>4s</th><th>6s</th><th>Balls</th></tr>");

                string dismissalColumnText, bowlerColumnText;
                for (int i = 0; i < Teams.teamOnePlayers.Count; i++)
                {
                    dismissalColumnText = ""; bowlerColumnText = "";
                    if (Teams.teamOnePlayers[i].IsOut && (Teams.teamOnePlayers[i].DismissalMethod == "b" ||
                                                          Teams.teamOnePlayers[i].DismissalMethod == "lbw"))
                    {
                        bowlerColumnText = "b " + Teams.teamTwoPlayers[Teams.teamOnePlayers[i].DismissingBowler].FullName;
                    }
                    else if (Teams.teamOnePlayers[i].IsOut && (Teams.teamOnePlayers[i].DismissalMethod == "ct" ||
                                                               Teams.teamOnePlayers[i].DismissalMethod == "st"))
                    {
                        dismissalColumnText = Teams.teamOnePlayers[i].DismissalMethod + " " + Teams.teamTwoPlayers[Teams.teamOnePlayers[i].DismissingFielder].FullName;
                        bowlerColumnText = "b " + Teams.teamTwoPlayers[Teams.teamOnePlayers[i].DismissingBowler].FullName;
                    }
                    else if (Teams.teamOnePlayers[i].IsOut && Teams.teamOnePlayers[i].DismissalMethod == "hw")
                    {
                        dismissalColumnText = "hit wicket";
                        bowlerColumnText = "b " + Teams.teamTwoPlayers[Teams.teamOnePlayers[i].DismissingBowler].FullName;
                    }
                    else if (Teams.teamOnePlayers[i].IsOut && Teams.teamOnePlayers[i].DismissalMethod == "ro")
                    {
                        dismissalColumnText = "run out (" + Teams.teamTwoPlayers[Teams.teamOnePlayers[i].DismissingFielder].FullName + ")";
                    }
                    else if (i == Teams.currentBatsmanOne || i == Teams.currentBatsmanTwo)
                    {
                        dismissalColumnText = "not out";
                    }
                    else if (!Teams.teamOnePlayers[i].IsOut && i != Teams.currentBatsmanOne && i != Teams.currentBatsmanTwo)
                    {
                        dismissalColumnText = "did not bat";
                    }
                    // Not sure how to represent "to", "ht", "ob" on a scorecard?

                    // TODO Extract WriteBatsmanData() method
                    // TODO add Captain * and Keeper +
                    file.WriteLine("<TR><td>" + (i + 1) + "</td><td width=25%>{0}</td><TD>{1}</TD><TD>{2}</td><TD>{3}</td><TD>{4}</td><TD>{5}</td><TD>{6}</td></TR>",
                                                           Teams.teamOnePlayers[i].FullName, dismissalColumnText, bowlerColumnText,
                                                           Teams.teamOnePlayers[i].RunsScored, Teams.teamOnePlayers[i].NumberOfFoursScored,
                                                           Teams.teamOnePlayers[i].NumberOfSixesScored, Teams.teamOnePlayers[i].DeliveriesFaced);
                }

                string extras = "b(" + Innings.Byes + "),lb(" + Innings.LegByes + "),w(" + Innings.Wides + "),nb(" + Innings.NoBalls + ")";
                file.WriteLine("<TR><td>Extras: </td><td>{0}</td><TD></td>,<TD></td><TD></td>,<TD>{1}</td></TR>", extras, (Innings.Byes + Innings.LegByes + Innings.Wides + Innings.NoBalls));
                file.WriteLine("<TR><TD>Total: </TD><td></td><TD></td>,<TD></td><TD></td>,<TD>{0}</td>", Innings.Runs, "/TR>");
                file.WriteLine("<TR><TD>Wickets: </TD><td></td><TD></td>,<TD></td><TD></td>,<TD>{0}</td>", Innings.Wickets, "/TR>");
                file.WriteLine("<TR><TD>Overs: </TD><td></td><TD></td>,<TD></td><TD></td>,<TD>{0}</td>", Innings.Overs, "/TR>");
                file.WriteLine("</table>");

                // Need a fall of wicket section (Innings class needs new members to store that data)
                file.WriteLine("<H4>Fall of wicket</H4>"); // Needs not out batsman score

                file.WriteLine("<table>");
                file.WriteLine("<TR><th>&nbsp;</th><th width = 25%>Bowler</th><th>Overs</th><th>Maidens</th><th>Runs</th><th>Wickets</th><th>Wides</th><th>No Balls</th></TR>");

                // TODO Bowling analysis - need another member on player to determine the bowling order
                for (int i = 0; i < Teams.teamTwoPlayers.Count; i++)
                {
                    if (Teams.teamTwoPlayers[i].NumberOfOversBowled > 0)
                    {
                        // TODO Extract WriteBowlerData() method
                        file.WriteLine("<TR><TD>" + (i + 1) + "</TD><td>{0}</td><TD>{1}</td><TD>{2}</td><TD>{3}</td><TD>{4}</td><TD>{5}</td><TD>{6}</td></TR>",
                            Teams.teamTwoPlayers[i].FullName, Teams.teamTwoPlayers[i].NumberOfOversBowled,
                            Teams.teamTwoPlayers[i].NumberOfMaidensBowled, Teams.teamTwoPlayers[i].RunsConceded,
                            Teams.teamTwoPlayers[i].NumberOfWicketsTaken, Teams.teamTwoPlayers[i].WidesConceded,
                            Teams.teamTwoPlayers[i].NoBallsDelivered);
                    }
                }
                file.WriteLine("</table>");
                file.WriteLine("</body>");
                file.WriteLine("</html>");

                file.Flush();
                file.Close();
            }
            return;
        }

        public static void WriteHTMLScorecard()
        {
            // This will ultimately replace WriteScorecard(), but will be developed in parallel, leaving WriteScorecard() working in the meantime
           // StringWriter stringWriter = new StringWriter();
           // using (XmlTextWriter writer = new XmlTextWriter(stringWriter))

            var settings = new System.Xml.XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            var writer = System.Xml.XmlWriter.Create("scorecard.html", settings);

            writer.WriteStartDocument();
            writer.WriteDocType("html", null, null, null);
            writer.WriteStartElement("html");
            writer.WriteStartElement("head");
            writer.WriteEndElement(); // </head>
            writer.WriteStartElement("body");
            writer.WriteStartElement("p");
            writer.WriteString("Hello World");
            writer.WriteEndElement(); // </p>
            writer.WriteEndElement(); // </body>
            writer.WriteEndElement(); // </html>
            writer.WriteEndDocument();
            writer.Close();
            
        }
    }
}