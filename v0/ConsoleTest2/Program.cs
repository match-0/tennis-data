using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TennisData;

using System.ComponentModel;
using System.Data;
//using System.Drawing;
//using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.IO;
using System.IO.Compression;
using System.Web;





namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Button3 clicked");
            var P = new Program();

            // Keep the program running
            while (true)
            {
                // Execute the function
                P.ExecuteFunction();

                // Wait for 24 hours before executing the function again
                Thread.Sleep(TimeSpan.FromDays(1));
            }

            Console.WriteLine("button3 ended");
        }

        private void ExecuteFunction()
        {
            string matchListURL = "http://tennisabstract.com/charting/";

            // This function will be executed once every day
            Console.WriteLine("Function executed at: " + DateTime.Now);
            int count;
            count = WebFunction.DownloadAllPlayerInfoToLocal();
            Console.WriteLine(count);

            count = 0;
            List<string> all_players = new List<string> { };
            foreach (var x in WebFunction.GetMatchList())
            {
                string matchURL = x.Value;
                int lastDot = matchURL.LastIndexOf(".");
                if (lastDot == -1) lastDot = matchURL.Length; // some webpages do not end with .html
                int lastDash = matchURL.LastIndexOf("-");
                int secondLastDash = matchURL.LastIndexOf("-", lastDash - 1);
                string ply1Name = matchURL.Substring(secondLastDash + 1, lastDash - secondLastDash - 1).Replace("_", " ");
                string ply2Name = matchURL.Substring(lastDash + 1, lastDot - lastDash - 1).Replace("_", " ");
                // string ply1FileName = LocalMatchListFolder + ply1Name.Replace(" ", "") + ".js";
                // string ply2FileName = LocalMatchListFolder + ply2Name.Replace(" ", "") + ".js";
                if (!all_players.Contains(ply1Name))
                {
                    all_players.Add(ply1Name);
                }
                if (!all_players.Contains(ply2Name))
                {
                    all_players.Add(ply2Name);
                }

            }
            Console.WriteLine(all_players.Count);
            int count2 = 0;
            foreach (var x in all_players)
            {
                var y = WebFunction.GetPlayerInfo(x);
                if (y.Hand == 99)
                {
                    if (!WebFunction.UpdatePlayerInfo(x))
                    {
                        Console.WriteLine("Failed " + x);
                        count++;
                    }
                }
                else
                {
                    count2++;
                }
            }
            Console.WriteLine(count);
            Console.WriteLine(count2);

            string LocalMatchListFolder = ConfigurationManager.AppSettings["LocalDataDirectory"];
            string localMatchListTxt = LocalMatchListFolder + "MatchList.html";
            WebClient wc = new WebClient();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            string pattern = @"\d{4}-\d{2}-\d{2}";
            doc.LoadHtml(wc.DownloadString(matchListURL));
            doc.Save(localMatchListTxt);

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                if (!Regex.IsMatch(link.InnerText, pattern))
                    continue;
                //Console.WriteLine(link.Attributes["href"].Value);
                string[] onelink = new string[1];
                //right//onelink[0] = "20240126-M-Australian_Open-SF-Jannik_Sinner-Novak_Djokovic.html";
                onelink[0] = link.Attributes["href"].Value;
                WebFunction.DownloadMatchListToLocal(onelink);
                // int returnValue = 123;
                // returnValue = WebFunction.DownloadMatchListToLocal(onelink);
                // Console.WriteLine("DownloadMatchListToLocal returnValue = " + returnValue.ToString());
                // if (returnValue == 0)
                // {
                //     continue;
                // }

                Dictionary<string, int> nums = null;
                List<TennisData.Point> points = new List<TennisData.Point>();

                string matchURL = onelink[0];
                //string matchURL = link.Attributes["href"].Value;
                int lastDot = matchURL.LastIndexOf(".");
                if (lastDot == -1)
                    continue;

                points = WebFunction.GetPointLog(matchURL);
                Console.WriteLine(matchURL);
                Console.WriteLine(points.Count);

                string CSVName = matchURL.Replace(".html", ".csv");
                if (points.Count > 0 && points[0].Match.Player1.Hand == WebFunction.RIGHT_HAND
                && points[0].Match.Player2.Hand == WebFunction.RIGHT_HAND)
                {
                    List<TennisData.Stroke> strokes = new List<Stroke>();
                    foreach (TennisData.Point p in points)
                    {
                        List<Stroke> temp = p.ExtractStrokes();
                        List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                        strokes.AddRange(converted);
                    }
                    TennisDataLib.ML.StrokeByStroke(CSVName, strokes);
                    Dictionary<string, int> Nums = new Dictionary<string, int>();
                    Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                    nums = Nums;
                }

                else if (points.Count > 0 && points[0].Match.Player1.Hand == WebFunction.LEFT_HAND
                && points[0].Match.Player2.Hand == WebFunction.LEFT_HAND)
                {
                    List<TennisData.Stroke> strokes = new List<Stroke>();
                    foreach (TennisData.Point p in points)
                    {
                        List<Stroke> temp = p.ExtractStrokes();
                        List<Stroke> converted = Model_LH_LH_6Regions.Convert(temp);
                        strokes.AddRange(converted);
                    }
                    TennisDataLib.ML.StrokeByStroke(CSVName, strokes);
                    Dictionary<string, int> Nums = new Dictionary<string, int>();
                    Nums = Model_LH_LH_6Regions.CountEvents(strokes);
                    nums = Nums;
                }

                else if (points.Count > 0 && points[0].Match.Player1.Hand == WebFunction.RIGHT_HAND
                && points[0].Match.Player2.Hand == WebFunction.LEFT_HAND)
                {
                    List<TennisData.Stroke> strokes = new List<Stroke>();
                    foreach (TennisData.Point p in points)
                    {
                        List<Stroke> temp = p.ExtractStrokes();
                        List<Stroke> converted = Model_RH_LH_6Regions.Convert(temp);
                        strokes.AddRange(converted);
                    }
                    TennisDataLib.ML.StrokeByStroke(CSVName, strokes);
                    Dictionary<string, int> Nums = new Dictionary<string, int>();
                    Nums = Model_RH_LH_6Regions.CountEvents(strokes);
                    nums = Nums;
                }

                else if (points.Count > 0 && points[0].Match.Player1.Hand == WebFunction.LEFT_HAND
                && points[0].Match.Player2.Hand == WebFunction.RIGHT_HAND)
                {
                    List<TennisData.Stroke> strokes = new List<Stroke>();
                    foreach (TennisData.Point p in points)
                    {
                        List<Stroke> temp = p.ExtractStrokes();
                        List<Stroke> converted = Model_LH_RH_6Regions.Convert(temp);
                        strokes.AddRange(converted);
                    }
                    TennisDataLib.ML.StrokeByStroke(CSVName, strokes);
                    Dictionary<string, int> Nums = new Dictionary<string, int>();
                    Nums = Model_RH_LH_6Regions.CountEvents(strokes);
                    nums = Nums;
                }


                if (nums.Count > 0 && points[0].Match.Player1.Hand == WebFunction.RIGHT_HAND
                && points[0].Match.Player2.Hand == WebFunction.RIGHT_HAND)
                {
                    string modelDir = System.Configuration.ConfigurationManager.AppSettings["ModelTemplateDirectory"];
                    string pcspDir = System.Configuration.ConfigurationManager.AppSettings["PcspOutputDirectory"];
                    string tempFile = pcspDir + "Temp-" + System.Guid.NewGuid().ToString() + ".pcsp";
                    PATFunction.ReplaceWeight(modelDir + @"Right-Right Model 6 Regions.pcsp",
                        tempFile, nums, points[0].Match.Player1.Name, points[0].Match.Player2.Name);
                    double[] result = { 0, 0 };
                    result = PATFunction.GetPATResult(tempFile);
                    Console.WriteLine(points[0].Match.Player1.Name + " vs " + points[0].Match.Player2.Name);
                    Console.WriteLine(result[0].ToString() + ", " + result[1].ToString());
                    Console.WriteLine(points[0].Match.ActualResult);

                    //create a record of the results
                    double[] unknown = { 0, 0, 0 };
                    unknown = UnknownPercentage(CSVName);
                    WriteResult2CSV(result[0], result[1], points[0].Match, unknown);

                    double difference = Math.Abs(result[0] - result[1]);
                    //max and min results has a big difference 
                    if (difference >= 0.15)
                    {
                        Console.WriteLine("Move for principle difference >= 0.15 .");
                        MoveFile(CSVName);
                    }
                    //there is too much unknown description in the points
                    if (unknown[0] >= 0.05)
                    {
                        Console.WriteLine("Move for principle unknown >= 0.05 .");
                        MoveFile(CSVName);
                    }
                    ////there is a big difference between prediction and actual result
                    if (!isPredictionConsistent(result[0], result[1], points[0].Match))
                    {
                        Console.WriteLine("Move for principle min result < actual function.");
                        MoveFile(CSVName);
                    }

                }

                else if (nums.Count > 0 && points[0].Match.Player1.Hand == WebFunction.LEFT_HAND
                && points[0].Match.Player2.Hand == WebFunction.LEFT_HAND)
                {
                    string modelDir = System.Configuration.ConfigurationManager.AppSettings["ModelTemplateDirectory"];
                    string pcspDir = System.Configuration.ConfigurationManager.AppSettings["PcspOutputDirectory"];
                    string tempFile = pcspDir + "Temp-" + System.Guid.NewGuid().ToString() + ".pcsp";
                    PATFunction.ReplaceWeight(modelDir + @"Left-Left Model 6 Regions.pcsp",
                        tempFile, nums, points[0].Match.Player1.Name, points[0].Match.Player2.Name);
                    double[] result = { 0, 0 };
                    result = PATFunction.GetPATResult(tempFile);
                    Console.WriteLine(points[0].Match.Player1.Name + " vs " + points[0].Match.Player2.Name);
                    Console.WriteLine(result[0].ToString() + ", " + result[1].ToString());
                    Console.WriteLine(points[0].Match.ActualResult);

                    //create a record of the results
                    double[] unknown = { 0, 0, 0 };
                    unknown = UnknownPercentage(CSVName);
                    WriteResult2CSV(result[0], result[1], points[0].Match, unknown);

                    double difference = Math.Abs(result[0] - result[1]);
                    //max and min results has a big difference 
                    if (difference >= 0.15)
                    {
                        Console.WriteLine("Move for principle difference >= 0.15 .");
                        MoveFile(CSVName);
                    }
                    //there is too much unknown description in the points
                    if (unknown[0] >= 0.05)
                    {
                        Console.WriteLine("Move for principle unknown >= 0.05 .");
                        MoveFile(CSVName);
                    }
                    ////there is a big difference between prediction and actual result
                    if (!isPredictionConsistent(result[0], result[1], points[0].Match))
                    {
                        Console.WriteLine("Move for principle min result < actual function.");
                        MoveFile(CSVName);
                    }
                }

                else if (nums.Count > 0 && points[0].Match.Player1.Hand == WebFunction.RIGHT_HAND
                && points[0].Match.Player2.Hand == WebFunction.LEFT_HAND)
                {
                    string modelDir = System.Configuration.ConfigurationManager.AppSettings["ModelTemplateDirectory"];
                    string pcspDir = System.Configuration.ConfigurationManager.AppSettings["PcspOutputDirectory"];
                    string tempFile = pcspDir + "Temp-" + System.Guid.NewGuid().ToString() + ".pcsp";
                    PATFunction.ReplaceWeight(modelDir + @"Right-Left Model 6 Regions.pcsp",
                        tempFile, nums, points[0].Match.Player1.Name, points[0].Match.Player2.Name);
                    double[] result = { 0, 0 };
                    result = PATFunction.GetPATResult(tempFile);
                    Console.WriteLine(points[0].Match.Player1.Name + " vs " + points[0].Match.Player2.Name);
                    Console.WriteLine(result[0].ToString() + ", " + result[1].ToString());
                    Console.WriteLine(points[0].Match.ActualResult);

                    //create a record of the results
                    double[] unknown = { 0, 0, 0 };
                    unknown = UnknownPercentage(CSVName);
                    WriteResult2CSV(result[0], result[1], points[0].Match, unknown);

                    double difference = Math.Abs(result[0] - result[1]);
                    //max and min results has a big difference 
                    if (difference >= 0.15)
                    {
                        Console.WriteLine("Move for principle difference >= 0.15 .");
                        MoveFile(CSVName);
                    }
                    //there is too much unknown description in the points
                    if (unknown[0] >= 0.05)
                    {
                        Console.WriteLine("Move for principle unknown >= 0.05 .");
                        MoveFile(CSVName);
                    }
                    ////there is a big difference between prediction and actual result
                    if (!isPredictionConsistent(result[0], result[1], points[0].Match))
                    {
                        Console.WriteLine("Move for principle min result < actual function.");
                        MoveFile(CSVName);
                    }
                }

                else if (nums.Count > 0 && points[0].Match.Player1.Hand == WebFunction.LEFT_HAND
                && points[0].Match.Player2.Hand == WebFunction.RIGHT_HAND)
                {
                    string modelDir = System.Configuration.ConfigurationManager.AppSettings["ModelTemplateDirectory"];
                    string pcspDir = System.Configuration.ConfigurationManager.AppSettings["PcspOutputDirectory"];
                    string tempFile = pcspDir + "Temp-" + System.Guid.NewGuid().ToString() + ".pcsp";
                    PATFunction.ReplaceWeight(modelDir + @"Right-Left Model 6 Regions.pcsp",
                        tempFile, nums, points[0].Match.Player1.Name, points[0].Match.Player2.Name);
                    double[] result = { 0, 0 };
                    result = PATFunction.GetPATResult(tempFile);
                    Console.WriteLine(points[0].Match.Player1.Name + " vs " + points[0].Match.Player2.Name);
                    Console.WriteLine(result[0].ToString() + ", " + result[1].ToString());
                    Console.WriteLine(points[0].Match.ActualResult);

                    //create a record of the results
                    double[] unknown = { 0, 0, 0 };
                    unknown = UnknownPercentage(CSVName);
                    WriteResult2CSV(result[0], result[1], points[0].Match, unknown);

                    double difference = Math.Abs(result[0] - result[1]);
                    //max and min results has a big difference 
                    if (difference >= 0.15)
                    {
                        Console.WriteLine("Move for principle difference >= 0.15 .");
                        MoveFile(CSVName);
                    }
                    //there is too much unknown description in the points
                    if (unknown[0] >= 0.05)
                    {
                        Console.WriteLine("Move for principle unknown >= 0.05 .");
                        MoveFile(CSVName);
                    }
                    ////there is a big difference between prediction and actual result
                    if (!isPredictionConsistent(result[0], result[1], points[0].Match))
                    {
                        Console.WriteLine("Move for principle min result < actual function.");
                        MoveFile(CSVName);
                    }
                }


            }

        }

        private double[] UnknownPercentage(string CSVName)
        {
            double[] per = { 0, 0, 0};
            double num = 0;
            double total = 0;
            string[] lines = File.ReadAllLines(CSVName);
            foreach(string line in lines)
            {
                string[] columns = line.Split(',');
                string lastcol = columns[columns.Length - 1];
                if (lastcol.ToLower().Equals("unknown"))
                    num += 1;
                total += 1;
            }
            per[0] = num / total;
            per[1] = num;
            per[2] = total;
            return per;
        }

        private double[] computePlayersMatchResult(string ActualResult)
        {
            int dash = ActualResult.LastIndexOf("-");
            string ply1GameResult, ply2GameResult;
            double ply1MatchResult = 0;
            double ply2MatchResult = 0;
            double[] MatchResult = { 0, 0 };
            while (dash != -1)
            {
                ply1GameResult = ActualResult.Substring(dash - 1, 1);
                ply2GameResult = ActualResult.Substring(dash + 1, 1);
                ply1MatchResult += Convert.ToDouble(ply1GameResult);
                ply2MatchResult += Convert.ToDouble(ply2GameResult);
                dash = ActualResult.LastIndexOf('-', dash - 1);
            }
            MatchResult[0] = ply1MatchResult;
            MatchResult[1] = ply2MatchResult;
            return MatchResult;
        }

        private bool isPredictionConsistent(double minResult, double maxResult, MatchInfo match)
        {
            int index = match.ActualResult.IndexOf(match.Player1.Name);
            int whoWin = 0;
            if (index == 0)
            {
                whoWin = 1;
            }
            else
            {
                whoWin = 2;
            }
            if (whoWin == 2)
            {
                double temp = minResult;
                minResult = 1 - maxResult;
                maxResult = 1 - temp;
            }
            
            double[] MatchResult = computePlayersMatchResult(match.ActualResult);
            //averesult >= x - max(|x - 0.5| * 0.7, 0.05)
            double x = MatchResult[0] / (MatchResult[0] + MatchResult[1]);
            double y = x - Math.Max(Math.Abs(x - 0.5) * 0.7, 0.05);
            if ((minResult + maxResult) / 2 >= y)
            {
                return true;
            }
            return false;
        }

        private void WriteResult2CSV(double minResult, double maxResult, MatchInfo match, double[] per)
        {
            string minres = minResult.ToString();
            string maxres = maxResult.ToString();
            double[] MatchResult = computePlayersMatchResult(match.ActualResult);
            string ply1MatchResult = MatchResult[0].ToString();
            string ply2MatchResult = MatchResult[1].ToString();
            string ply1ActualRate = (MatchResult[0] / (MatchResult[0] + MatchResult[1])).ToString();
            string unknownPer = per[0].ToString();
            string unknownNum = per[1].ToString();
            string linesTotal = per[2].ToString();
            string ResultFolder = @"./ResultFolder/";
            string ResultName = "ResultRecord" + "-" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            
            if (!Directory.Exists(ResultFolder))
            {
                Directory.CreateDirectory(ResultFolder);
            }

            using (StreamWriter sw = File.AppendText(ResultFolder + ResultName))
            {
                string line = "";
                line += match.MatchURL; line += ',';
                line += match.Player1.Name; line += ',';
                line += match.Player2.Name; line += ',';
                line += minres; line += ',';
                line += maxres; line += ',';
                line += ply1MatchResult; line += ',';
                line += ply2MatchResult; line += ',';
                line += ply1ActualRate; line += ',';
                line += unknownPer; line += ',';
                line += unknownNum; line += ',';
                line += linesTotal; line += ',';
                line += DateTime.Now.ToString();
                sw.WriteLine(line);
            }
        }

        private void MoveFile(string CSVName)
        {
            string targetFolder = @"./ErrorFolder/";

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            if (File.Exists(targetFolder + CSVName))
            {
                File.Delete(targetFolder + CSVName);
            }

            try
            {
                File.Move(CSVName, targetFolder + CSVName);
                Console.WriteLine("File moved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

        }
        

    }
}

