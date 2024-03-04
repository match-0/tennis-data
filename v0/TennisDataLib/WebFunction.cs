using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Configuration;

namespace TennisData
{
    public class WebFunction
    {
        static private string matchListURL = "http://tennisabstract.com/charting/";
        static private string matchListURL2 = "http://tennisabstract.com/charting/";
        static private string LocalMatchListURL = "file://C/FYP/modified_jk/charting/";
        //static private string LocalMatchListFolder = @"D:\FYP\modified_jk\charting\";
        //static private string playerInfoURL = "http://www.minorleaguesplits.com/tennisabstract/cgi-bin/jsmatches/";
        static private string playerInfoURL = "http://www.tennisabstract.com/cgi-bin/player.cgi?p=";
        static private string matchRawCSVURL = "https://github.com/JeffSackmann/tennis_MatchChartingProject/blob/master/charting-m-points.csv?raw=true";
        static private string matchRawLocalFileName = @"D:\FYP\modified_jk\TennisData\TennisData\charting-m-points.csv";
        static private string POINT_LOG_START = "var pointlog = '";
        static private string POINT_LOG_END = "'";
        static private string RALLYOUTCOMES_START = "var rallyoutcomes = '";
        static private string RALLYOUTCOMES_END = "'";
        static private string ATPRankingURL = "http://www.atpworldtour.com/en/rankings/singles";
        static private string ATPPlayerSearchURL = "http://www.atpworldtour.com/en/search-results/players?searchTerm=";
        static private string ATPResultURL = "http://www.atpworldtour.com/en/scores/results-archive";
        static private string ATPBaseURL = "http://www.atpworldtour.com";
        static private string ATPPlayerBaseURL = "http://www.atpworldtour.com/en/players/";
        static private string RANKING_START = "<tbody>";
        static private string RANKING_END = "</tbody>";
        static private string WHITE_SPACE = "&nbsp;";
        static private string DASH = "&#8209;";
        static private string SECOND_SERVE = "2nd serve";
        static public byte RIGHT_HAND = 1;
        static public byte LEFT_HAND = 2;
        static private string TennisExplorerBaseURL = "https://www.tennisexplorer.com/player/";
        static private string TennisExplorerSearchBaseURL = "https://www.tennisexplorer.com/res/ajax/search.php?s=";
        static public Dictionary<string, string> GetMatchList()
        {
            WebClient wc = new WebClient();
            HtmlDocument doc = new HtmlDocument();
            Dictionary<string, string> matches = new Dictionary<string, string>();
            string pattern = @"\d{4}-\d{2}-\d{2}";

            if (ConfigurationManager.AppSettings["UseLocalData"].ToLower().Equals("true")
                && Directory.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"]))
            {
                matchListURL = ConfigurationManager.AppSettings["LocalDataDirectory"] + "MatchList.html";
            }
            doc.LoadHtml(wc.DownloadString(matchListURL));
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                if (Regex.IsMatch(link.InnerText, pattern)) //Assume all match links start with dddd-dd-dd
                {
                    matches.Add(link.InnerText, link.Attributes["href"].Value);
                } 
            }
            return matches;
        }

        static public Dictionary<string, MatchInfo> GetMatchInfoList()
        {
            WebClient wc = new WebClient();
            HtmlDocument doc = new HtmlDocument();
            Dictionary<string, MatchInfo> matches = new Dictionary<string, MatchInfo>();
            string pattern = @"\d{4}-\d{2}-\d{2}";

            if (ConfigurationManager.AppSettings["UseLocalData"].ToLower().Equals("true")
                && Directory.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"]))
            {
                matchListURL = ConfigurationManager.AppSettings["LocalDataDirectory"] + "MatchList.html";
            }
            doc.LoadHtml(wc.DownloadString(matchListURL));
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                if (Regex.IsMatch(link.InnerText, pattern)) //Assume all match links start with dddd-dd-dd
                {
                    matches.Add(link.Attributes["href"].Value, GetMatchInfo(link.Attributes["href"].Value));
                }
            }
            return matches;
        }

        static public int DownloadMatchListToLocal()
        {
            string LocalMatchListFolder = ConfigurationManager.AppSettings["LocalDataDirectory"];
            string localMatchListTxt = LocalMatchListFolder + "MatchList.html";
            WebClient wc = new WebClient();
            HtmlDocument doc = new HtmlDocument();
            string pattern = @"\d{4}-\d{2}-\d{2}";
            int count = 0;
            doc.LoadHtml(wc.DownloadString(matchListURL));
            doc.Save(localMatchListTxt);
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                if (Regex.IsMatch(link.InnerText, pattern)) //Assume all match links start with dddd-dd-dd
                {
                    if (!File.Exists(LocalMatchListFolder + link.Attributes["href"].Value))
                    {
                        try
                        {
                            if (GZipDownload(matchListURL2 + link.Attributes["href"].Value, true,
                                LocalMatchListFolder + link.Attributes["href"].Value) > 0)
                            {
                                count++;
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return count;
        }

        static public int DownloadMatchListToLocal(string[] links)
        {
            string LocalMatchListFolder = ConfigurationManager.AppSettings["LocalDataDirectory"];
            string localMatchListTxt = LocalMatchListFolder + "MatchList.html";
            WebClient wc = new WebClient();
            HtmlDocument doc = new HtmlDocument();
            int count = 0;
            try
            {
                doc.LoadHtml(wc.DownloadString(matchListURL2));
            }
            catch (Exception ex) {}
            doc.Save(localMatchListTxt);
            foreach (string s in links)
            {
                if (!File.Exists(LocalMatchListFolder + s))
                {
                    try
                    {
                        if (GZipDownload(matchListURL2 + s, true,
                            LocalMatchListFolder + s) > 0)
                        {
                            count++;
                        }
                    }
                    catch (Exception ex) {}
                }
            }
            return count;
        }

        static public int DownloadAllPlayerInfoToLocal()
        {
            string LocalMatchListFolder = ConfigurationManager.AppSettings["LocalDataDirectory"];
            WebClient wc = new WebClient();
            HtmlDocument doc = new HtmlDocument();
            string pattern = @"\d{4}-\d{2}-\d{2}";
            int count = 0;
            doc.LoadHtml(wc.DownloadString(matchListURL));
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                if (Regex.IsMatch(link.InnerText, pattern)) //Assume all match links start with dddd-dd-dd
                {
                    string matchURL = link.Attributes["href"].Value;
                    int lastDot = matchURL.LastIndexOf(".");
                    if (lastDot == -1) lastDot = matchURL.Length; // some webpages do not end with .html
                    int lastDash = matchURL.LastIndexOf("-");
                    int secondLastDash = matchURL.LastIndexOf("-", lastDash - 1);
                    string ply1Name = matchURL.Substring(secondLastDash + 1, lastDash - secondLastDash - 1).Replace("_", " ");
                    string ply2Name = matchURL.Substring(lastDash + 1, lastDot - lastDash - 1).Replace("_", " ");
                    string ply1FileName = LocalMatchListFolder + ply1Name.Replace(" ", "") + ".js";
                    string ply2FileName = LocalMatchListFolder + ply2Name.Replace(" ", "") + ".js";
                    if (!File.Exists(ply1FileName))
                    {
                        try
                        {
                            //string s = GetGZipHTML(playerInfoURL + ply1Name.Replace(" ", "") + ".js", true);
                            string s = GetGZipHTML(playerInfoURL + ply1Name.Replace(" ", ""), true);
                            if (s.Length > 0)
                            {
                                File.WriteAllText(ply1FileName, s);
                                count++;
                            }
                        }
                        catch (Exception ex) { }
                    }
                    if (!File.Exists(ply2FileName))
                    {
                        try
                        {
                            //string s = GetGZipHTML(playerInfoURL + ply2Name.Replace(" ", "") + ".js", true);
                            string s = GetGZipHTML(playerInfoURL + ply2Name.Replace(" ", ""), true);
                            if (s.Length > 0)
                            {
                                File.WriteAllText(ply2FileName, s);
                                count++;
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return count;
        }

        static public int DownloadAllPlayerInfoToLocal(string[] names)
        {
            string LocalMatchListFolder = ConfigurationManager.AppSettings["LocalDataDirectory"];
            WebClient wc = new WebClient();
            int count = 0;
            foreach (string n in names)
            {
                string ply1FileName = LocalMatchListFolder + n.Replace(" ", "") + ".js";
                if (!File.Exists(ply1FileName))
                {
                    try
                    {
                        string s = GetGZipHTML(playerInfoURL + n.Replace(" ", "") + ".js", true);
                        if (s.Length > 0)
                        {
                            File.WriteAllText(ply1FileName, s);
                            count++;
                        }
                    }
                    catch (Exception ex) { }
                }
            }
            return count;
        }

        static public bool UpdatePlayerInfo(string name)
        {
            string LocalMatchListFolder = ConfigurationManager.AppSettings["LocalDataDirectory"];

            string ply1FileName = LocalMatchListFolder + name.Replace(" ", "") + ".js";
            if (File.Exists(ply1FileName))
            {
                var p = GetPlayerInfoFromTennisExplorer(name);
                if (p.Hand ==99){
                    return false;
                }
                try
                {
                    var s = File.ReadAllText(ply1FileName);
                    if (p.Hand == RIGHT_HAND){
                        s = "var hand = 'R';\n" + s;
                    }
                    else{
                        s = "var hand = 'L';\n" + s;
                    }
                    
                    File.WriteAllText(ply1FileName, s);
                    return true;
                }
                catch (Exception ex) { return false;}
            }
            else{
                return false;
            }
        }

        static private string GetGZipHTML(string url, bool gzip){
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            if (gzip){
                req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            }
            string html = "";
            HttpWebResponse resp = null;
            Stream respStream = null;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
                respStream = resp.GetResponseStream();
                if (resp.ContentEncoding.ToLower().Contains("gzip"))
                    respStream = new GZipStream(respStream, CompressionMode.Decompress);
                else if (resp.ContentEncoding.ToLower().Contains("deflate"))
                    respStream = new DeflateStream(respStream, CompressionMode.Decompress);

                StreamReader reader = new StreamReader(respStream, Encoding.Default);
                html = reader.ReadToEnd();
            }
            catch (WebException wex)
            {
                // log the exception?
            }
            finally
            {
                if (resp != null) { resp.Close(); }
                if (respStream != null) { respStream.Close(); }
            }
            return html;
        }

        static private int GZipDownload(string url, bool gzip, string localFileName)
        {
            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            //if (gzip)
            //{
            //    req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            //}
            //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            //Stream respStream = resp.GetResponseStream();
            //if (resp.ContentEncoding.ToLower().Contains("gzip"))
            //    respStream = new GZipStream(respStream, CompressionMode.Decompress);
            //else if (resp.ContentEncoding.ToLower().Contains("deflate"))
            //    respStream = new DeflateStream(respStream, CompressionMode.Decompress);

            //StreamReader reader = new StreamReader(respStream, Encoding.Default);
            //char[] buffer = new char[512];
            //using (StreamWriter file = new StreamWriter(localFileName))
            //{
            //    while (reader.Peek() >= 0)
            //    {
            //        reader.Read(buffer, 0, buffer.Length);
            //        file.Write(buffer);
            //    }
            //}
            string html = GetGZipHTML(url, gzip);
            if (html.Length > 0)
            {
                File.WriteAllText(localFileName, html);
            }
            return html.Length;
        }

        static public void DownloadMatchRawCSV()
        {
            GZipDownload(matchRawCSVURL, true, matchRawLocalFileName);
        }

        static public Player GetPlayerInfo(string name)
        {
            string doc = null;
            Player p = new Player();
            p.Name = name;
            string pattern = @"var hand\s?=\s?'(?<hand>[RL])";

            if (name.ToLower().StartsWith("all right"))
            {
                p.Hand = RIGHT_HAND;
            }
            else if (name.ToLower().StartsWith("all left"))
            {
                p.Hand = LEFT_HAND;
            }
            else
            {
                try
                {
                    if (ConfigurationManager.AppSettings["UseLocalData"].ToLower().Equals("true")
                        && Directory.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"]))
                    {
                        doc = File.ReadAllText(ConfigurationManager.AppSettings["LocalDataDirectory"] + name.Replace(" ", "").Replace("\u00A0", "") + ".js");
                    }
                    else
                    {
                        doc = GetGZipHTML(playerInfoURL + name.Replace(" ", "") + ".js", true);
                    }
                    if (Regex.IsMatch(doc, pattern))
                    {
                        foreach (Match match in Regex.Matches(doc, pattern))
                        {
                            if (match.Groups["hand"].Value.Equals("R"))
                            {
                                p.Hand = RIGHT_HAND;
                            }
                            else
                            {
                                p.Hand = LEFT_HAND;
                            }
                        };
                    }
                    else
                    {
                        p.Hand = 99; //Unknown hand
                    }
                }
                catch (Exception e)
                {
                    p.Hand = 99;
                }
            }
            return p;
        }

        static public Player GetPlayerInfoFromTennisExplorer(string name)
        {
            string doc = null;
            Player p = new Player();
            p.Name = name;
            string pattern = @"Plays:\s?[rl]";
            string pattern2 = @"url"":""([a-z0-9-]*)";
            

            if (name.ToLower().StartsWith("all right"))
            {
                p.Hand = RIGHT_HAND;
            }
            else if (name.ToLower().StartsWith("all left"))
            {
                p.Hand = LEFT_HAND;
            }
            else
            {
                try
                {
                    var name_converted = name.Replace(" ", "%20");
                    string found_name = "";
                    doc = GetGZipHTML(TennisExplorerSearchBaseURL + name_converted.ToLower(), true);
                    if (Regex.IsMatch(doc, pattern2)){
                        found_name = Regex.Matches(doc, pattern2)[0].Groups[1].Value;
                    }
                    else{
                        p.Hand = 99;
                        return p;
                    }
                    // Console.WriteLine(TennisExplorerBaseURL + found_name + "/");
                    doc = GetGZipHTML(TennisExplorerBaseURL + found_name + "/", true);
                    // Console.WriteLine(doc);
                    if (Regex.IsMatch(doc, pattern))
                    {
                        foreach (Match match in Regex.Matches(doc, pattern))
                        {
                            // Console.WriteLine(match.Value);
                            if (match.Value.EndsWith("r"))
                            {
                                p.Hand = RIGHT_HAND;
                            }
                            else
                            {
                                p.Hand = LEFT_HAND;
                            }
                        };
                    }
                    else
                    {
                        p.Hand = 99; //Unknown hand
                    }
                }
                catch (Exception e)
                {
                    p.Hand = 99;
                }
            }
            return p;
        }

        static public MatchInfo GetMatchInfo(string matchURL)
        {
            if (!matchURL.EndsWith(".html")) matchURL = matchURL + ".html";
            MatchInfo match = new MatchInfo();
            match.Name = matchURL;
            match.MatchURL = matchURL;
            int lastDot = matchURL.LastIndexOf(".");
            if (lastDot == -1) lastDot = matchURL.Length;
            int lastDash = matchURL.LastIndexOf("-");
            int secondLastDash = matchURL.LastIndexOf("-", lastDash - 1);
            string ply1Name = matchURL.Substring(secondLastDash + 1, lastDash - secondLastDash - 1).Replace("_", " ");
            string ply2Name = matchURL.Substring(lastDash + 1, lastDot - lastDash - 1).Replace("_", " ");
            Player ply1 = GetPlayerInfo(ply1Name);
            Player ply2 = GetPlayerInfo(ply2Name);
            match.Player1 = ply1;
            match.Player2 = ply2;
            return match;
        }

        static public List<Point> GetPointLog(string matchURL)
        {
            if (ConfigurationManager.AppSettings["UseLocalData"].ToLower().Equals("true")
                && Directory.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"]))
            {
                matchURL = ConfigurationManager.AppSettings["LocalDataDirectory"] + matchURL;
            }
            else
            {
                if (matchURL.IndexOf(matchListURL) == -1) { matchURL = matchListURL + matchURL; }
            }
            MatchInfo match = GetMatchInfo(matchURL);
            List<Point> pointList = new List<Point>();
            if (match.Player1.Hand == 99 || match.Player2.Hand == 99)
            {
                //match.Player1.Hand = 1;
                //match.Player2.Hand = 1;
                return pointList;
            }
            WebClient wc = new WebClient();
            try
            {
                Point prevPt = null;
                string fullContent = wc.DownloadString(matchURL);
                string ply1winPattern = string.Format("<b>({0}\\s+d\\..+?)</b>", match.Player1.Name);
                System.Text.RegularExpressions.Match mat1 =
                    new System.Text.RegularExpressions.Regex(ply1winPattern).Match(fullContent);
                if (mat1.Success)
                {
                    match.ActualResult = mat1.Groups[1].Value;
                }
                else
                {
                    string ply2winPattern = string.Format("<b>({0}\\s+d\\..+?)</b>",
                                                    match.Player2.Name);
                    System.Text.RegularExpressions.Match mat2 =
                        new System.Text.RegularExpressions.Regex(ply2winPattern).Match(fullContent);
                    if (mat2.Success)
                    {
                        match.ActualResult = mat2.Groups[1].Value;
                    }
                    else
                    {
                        match.ActualResult = "";
                    }
                }
                int rallyoutcomesStart = fullContent.IndexOf(RALLYOUTCOMES_START) + RALLYOUTCOMES_START.Length;
                if (rallyoutcomesStart >= RALLYOUTCOMES_START.Length)
                {
                    int rallyoutcomesEnd = fullContent.IndexOf(RALLYOUTCOMES_END, rallyoutcomesStart);
                    string rallyOutcomes = fullContent.Substring(rallyoutcomesStart, rallyoutcomesEnd - rallyoutcomesStart);
                    HtmlDocument doc2 = new HtmlDocument();
                    doc2.LoadHtml(rallyOutcomes);
                    var totalPointsNode = doc2.DocumentNode.Descendants("td").First().NextSibling;
                    int totalPoints = int.Parse(totalPointsNode.InnerText);
                    match.Player1ActualPoints = int.Parse(totalPointsNode.NextSibling.InnerText.Split()[0]);
                    match.Player2ActualPoints = totalPoints - match.Player1ActualPoints;
                }
                int pointLogStart = fullContent.IndexOf(POINT_LOG_START) + POINT_LOG_START.Length;
                if (pointLogStart < POINT_LOG_START.Length) { return pointList; }
                int pointLogEnd = fullContent.IndexOf(POINT_LOG_END, pointLogStart);
                string pointLog = fullContent.Substring(pointLogStart, pointLogEnd - pointLogStart);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pointLog);
                Point p = new Point();
                int colNo = 1;
                foreach (HtmlNode col in doc.DocumentNode.SelectNodes("//td"))
                {
                    int dash = 0;
                    switch (colNo)
                    {
                        case 1:
                            if (col.InnerText.Replace(WHITE_SPACE, " ").Trim().Equals(match.Player1.Name, StringComparison.CurrentCultureIgnoreCase))
                            {
                                p.Server = 1;
                            }
                            else if (col.InnerText.Replace(WHITE_SPACE, " ").Trim().Equals(match.Player2.Name, StringComparison.CurrentCultureIgnoreCase))
                            {
                                p.Server = 2;
                            }
                            else
                            {
                                p.Server = 99;
                            };
                            break;
                        case 2:
                            if (p.Server != 99)
                            {
                                dash = col.InnerText.IndexOf(DASH);
                                p.Set1 = Convert.ToByte(col.InnerText.Substring(0, dash));
                                p.Set2 = Convert.ToByte(col.InnerText.Substring(dash + DASH.Length));
                            }
                            break;
                        case 3:
                            if (p.Server != 99)
                            {
                                dash = col.InnerText.IndexOf(DASH);
                                p.Game1 = Convert.ToByte(col.InnerText.Substring(0, dash));
                                p.Game2 = Convert.ToByte(col.InnerText.Substring(dash + DASH.Length));
                            }
                            break;
                        case 4:
                            if (p.Server != 99)
                            {
                                dash = col.InnerText.IndexOf(DASH);
                                p.Pts1 = col.InnerText.Substring(0, dash);
                                switch (p.Pts1)
                                {
                                    case "Jan":
                                        p.Pts1 = "1";
                                        break;
                                    case "Feb":
                                        p.Pts1 = "2";
                                        break;
                                    case "Mar":
                                        p.Pts1 = "3";
                                        break;
                                    case "Apr":
                                        p.Pts1 = "4";
                                        break;
                                    case "May":
                                        p.Pts1 = "5";
                                        break;
                                    case "Jun":
                                        p.Pts1 = "6";
                                        break;
                                    case "Jul":
                                        p.Pts1 = "7";
                                        break;
                                    case "Aug":
                                        p.Pts1 = "8";
                                        break;
                                    case "Sep":
                                        p.Pts1 = "9";
                                        break;
                                    case "Oct":
                                        p.Pts1 = "10";
                                        break;
                                    case "Nov":
                                        p.Pts1 = "11";
                                        break;
                                    case "Dec":
                                        p.Pts1 = "12";
                                        break;
                                    default:
                                        break;
                                };
                                p.Pts2 = col.InnerText.Substring(dash + DASH.Length);
                                switch (p.Pts2)
                                {
                                    case "Jan":
                                        p.Pts2 = "1";
                                        break;
                                    case "Feb":
                                        p.Pts2 = "2";
                                        break;
                                    case "Mar":
                                        p.Pts2 = "3";
                                        break;
                                    case "Apr":
                                        p.Pts2 = "4";
                                        break;
                                    case "May":
                                        p.Pts2 = "5";
                                        break;
                                    case "Jun":
                                        p.Pts2 = "6";
                                        break;
                                    case "Jul":
                                        p.Pts2 = "7";
                                        break;
                                    case "Aug":
                                        p.Pts2 = "8";
                                        break;
                                    case "Sep":
                                        p.Pts2 = "9";
                                        break;
                                    case "Oct":
                                        p.Pts2 = "10";
                                        break;
                                    case "Nov":
                                        p.Pts2 = "11";
                                        break;
                                    case "Dec":
                                        p.Pts2 = "12";
                                        break;
                                    default:
                                        break;
                                };
                            }
                            break;
                        default:
                            if (p.Server != 99)
                            {
                                p.Strokes = col.InnerText;
                            }
                            break;
                    }
                    colNo++;
                    if (colNo == 6)
                    {
                        if (p.Server != 99)
                        {
                            p.Match = match;
                            p.PrevPt = prevPt;
                            if (prevPt != null) p.PrevPt.NextPt = p;
                            pointList.Add(p);
                            prevPt = p;
                        }
                        p = new Point();
                        colNo = 1;
                    }
                }
            }
            catch (Exception e)
            {
                // Possible reasons:
                //  404, File not found
                //Console.WriteLine(e);
                // do nothing, maybe, log the exception
            }

            return pointList;
        }

        static public List<RankHt> GetRankHt(DateTime RankDate)
        {
            string html = GetGZipHTML(
                string.Format("{0}?rankDate={1}&rankRange=1-500", ATPRankingURL, RankDate.ToString("yyyy-MM-dd")),
                true);

            int rankBegin = html.IndexOf(RANKING_START) + RANKING_START.Length;
            int rankEnd = html.IndexOf(RANKING_END);
            if (rankBegin == -1 || rankEnd == -1) { return null; }
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html.Substring(rankBegin, rankEnd - rankBegin));
            List<RankHt> rankList = new List<RankHt>();
            RankHt r = new RankHt();
            string raw = "";
            int colNo = 1;
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//td");
            if (nodes == null) { return null; }
            foreach (HtmlNode col in nodes)
            {
                switch (colNo)
                {
                    case 1:
                        raw = col.InnerText.Replace(WHITE_SPACE, " ").Trim();
                        r.Rank = Int16.Parse(Regex.Replace(raw, @"[^0-9]", ""));
                        break;
                    case 4:
                        r.Name = col.InnerText.Replace(WHITE_SPACE, " ").Trim();
                        break;
                    case 5:
                        raw = col.InnerText.Replace(WHITE_SPACE, " ").Trim();
                        if (!raw.Equals("")) r.Age = Int16.Parse(raw);
                        break;
                    case 6:
                        raw = col.InnerText.Replace(WHITE_SPACE, " ").Trim();
                        r.Points = Int32.Parse(Regex.Replace(raw, @"[^0-9]", ""));
                        break;
                    case 7:
                        r.TourPlayed = Int16.Parse(col.InnerText.Replace(WHITE_SPACE, " ").Trim());
                        break;
                    case 8:
                        raw = col.InnerText.Replace(WHITE_SPACE, " ").Trim();
                        r.PointsDropping = Int32.Parse(Regex.Replace(raw, @"[^0-9]", ""));
                        break;
                    case 9:
                        raw = col.InnerText.Replace(WHITE_SPACE, " ").Trim();
                        r.NextBest = Int32.Parse(Regex.Replace(raw, @"[^0-9]", ""));
                        break;
                    default:
                        break;
                }
                colNo++;
                if (colNo == 10)
                {
                    r.Date = RankDate;
                    rankList.Add(r);
                    r = new RankHt();
                    colNo = 1;
                }
            }

            return rankList;
        }

        //static public ATPPlayer GetATPPlayer(string name)
        //{
        //    string html = GetGZipHTML(
        //        string.Format("{0}{1}", ATPPlayerSearchURL, HttpUtility.HtmlEncode(name.ToLower())),
        //        true);

        //    int rankBegin = html.IndexOf(RANKING_START) + RANKING_START.Length;
        //    int rankEnd = html.IndexOf(RANKING_END);
        //    if (rankBegin == -1 || rankEnd == -1) { return null; }
        //    HtmlDocument doc = new HtmlDocument();
        //    doc.LoadHtml(html.Substring(rankBegin, rankEnd - rankBegin));


        //    string pattern = @"/en/players/(.+)/(.+)/overview";
        //    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
        //    {
        //        Match m = new Regex(pattern).Match(link.Attributes[0].Value);
        //        if (m.Success)
        //        {
        //            ATPPlayer player = new ATPPlayer();
        //            player.Code1 = m.Groups[1].Value;
        //            player.Code2 = m.Groups[2].Value;
        //            player.Name = name;
        //            return player;
        //        }
        //    }

        //    return null;
        //}

        static public int GetATPPlayerHand(string code12)
        {
            try
            {
                string html = GetGZipHTML(
                    string.Format("{0}{1}/overview", ATPPlayerBaseURL, code12),
                    true);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                var root = doc.DocumentNode;
                var node = root.Descendants("div")
                    .Where(n => n.InnerText.Contains("-Handed") &&
                        n.GetAttributeValue("class", "").Equals("table-value"))
                    .First();
                string s = node.InnerText.Trim().ToUpper();
                if (s.StartsWith("RIGHT-HANDED"))
                {
                    return 1;
                }
                else if (s.StartsWith("LEFT-HANDED"))
                {
                    return 2;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        static public List<ATPTour> GetATPTours(int year)
        {
            string html = GetGZipHTML(
                string.Format("{0}?year={1}", ATPResultURL, year.ToString()),
                true);

            int rankBegin = html.IndexOf(RANKING_START) + RANKING_START.Length;
            int rankEnd = html.IndexOf(RANKING_END);
            if (rankBegin == -1 || rankEnd == -1) { return null; }
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html.Substring(rankBegin, rankEnd - rankBegin));
            var xList = doc.DocumentNode.Descendants("a")
                .Where(n => n.InnerText.Contains("Results"));
            List<string> urlList = new List<string>();
            foreach (var x in xList)
            {
                urlList.Add(ATPBaseURL + x.Attributes["href"].Value);
            }
            List<ATPTour> tourList = new List<ATPTour>();
            foreach (string url in urlList)
            {
                try
                {
                    ATPTour r = new ATPTour();
                    r.ResultURL = url;
                    string[] path = url.Split('/');
                    r.Code1 = Int32.Parse(path[7]);
                    r.Year = Int32.Parse(path[8]);
                    html = GetGZipHTML(url, true);
                    doc.LoadHtml(html);
                    var root = doc.DocumentNode;
                    r.Name = root.Descendants()
                        .Where(n => n.GetAttributeValue("class", "").Equals("dropdown-label autofillHolder"))
                        .Single().InnerText.Trim();
                    var datesNode = root.Descendants()
                        .Where(n => n.GetAttributeValue("class", "").Equals("tourney-dates"))
                        .Single();
                    string[] date = datesNode.InnerText.Trim().Split('-');
                    r.DateFrom = DateTime.Parse(date[0]);
                    r.DateTill = DateTime.Parse(date[1]);
                    var courtNode = root.Descendants()
                        .Where(n => n.GetAttributeValue("class", "").Equals("icon-court image-icon"))
                        .Single();
                    courtNode = courtNode.ParentNode.NextSibling.NextSibling.FirstChild.NextSibling.FirstChild.NextSibling;
                    r.Surface = courtNode.InnerText.Trim();
                    tourList.Add(r);
                }
                catch (Exception)
                {

                }
            }
            return tourList;
        }

        static public ATPResult GetATPResult(string url)
        {
            try
            {
                string html = GetGZipHTML(url, true);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                ATPResult result = new ATPResult();
                var root = doc.DocumentNode;

                #region winner set1-5
                var nodes = root.Descendants("span")
                    .Where(n => n.Id.Contains("_MS") && n.Id.Contains("_TeamOne_"));
                int set = 1;
                foreach (var node in nodes)
                {
                    switch (set)
                    {
                        case 1:
                            result.Set1WinScore = Int32.Parse(node.InnerText);
                            if (node.NextSibling != null && node.NextSibling.NextSibling != null)
                            {
                                var x = node.NextSibling.NextSibling;
                                if (x.Id.Contains("_Tiebreak")
                                    && (!x.InnerText.Trim().Equals("")))
                                {
                                    result.Set1TBScore = Int32.Parse(x.InnerText);
                                }
                            }
                            break;
                        case 2:
                            result.Set2WinScore = Int32.Parse(node.InnerText);
                            if (node.NextSibling != null && node.NextSibling.NextSibling != null)
                            {
                                var x = node.NextSibling.NextSibling;
                                if (x.Id.Contains("_Tiebreak")
                                    && (!x.InnerText.Trim().Equals("")))
                                {
                                    result.Set2TBScore = Int32.Parse(x.InnerText);
                                }
                            }
                            break;
                        case 3:
                            result.Set3WinScore = Int32.Parse(node.InnerText);
                            if (node.NextSibling != null && node.NextSibling.NextSibling != null)
                            {
                                var x = node.NextSibling.NextSibling;
                                if (x.Id.Contains("_Tiebreak")
                                    && (!x.InnerText.Trim().Equals("")))
                                {
                                    result.Set3TBScore = Int32.Parse(x.InnerText);
                                }
                            }
                            break;
                        case 4:
                            result.Set4WinScore = Int32.Parse(node.InnerText);
                            if (node.NextSibling != null && node.NextSibling.NextSibling != null)
                            {
                                var x = node.NextSibling.NextSibling;
                                if (x.Id.Contains("_Tiebreak")
                                    && (!x.InnerText.Trim().Equals("")))
                                {
                                    result.Set4TBScore = Int32.Parse(x.InnerText);
                                }
                            }
                            break;
                        default:
                            result.Set5WinScore = Int32.Parse(node.InnerText);
                            if (node.NextSibling != null && node.NextSibling.NextSibling != null)
                            {
                                var x = node.NextSibling.NextSibling;
                                if (x.Id.Contains("_Tiebreak")
                                    && (!x.InnerText.Trim().Equals("")))
                                {
                                    result.Set5TBScore = Int32.Parse(x.InnerText);
                                }
                            }
                            break;
                    }
                    set++;
                }
                #endregion

                #region loser set1-5
                nodes = root.Descendants("span")
                    .Where(n => n.Id.Contains("_MS") && n.Id.Contains("_TeamTwo_"));
                set = 1;
                foreach (var node in nodes)
                {
                    switch (set)
                    {
                        case 1:
                            result.Set1LoseScore = Int32.Parse(node.InnerText);
                            if (node.NextSibling != null && node.NextSibling.NextSibling != null)
                            {
                                var x = node.NextSibling.NextSibling;
                                if (x.Id.Contains("_Tiebreak")
                                    && (!x.InnerText.Trim().Equals("")))
                                {
                                    result.Set1TBScore = Int32.Parse(x.InnerText);
                                }
                            }
                            break;
                        case 2:
                            result.Set2LoseScore = Int32.Parse(node.InnerText);
                            if (node.NextSibling != null && node.NextSibling.NextSibling != null)
                            {
                                var x = node.NextSibling.NextSibling;
                                if (x.Id.Contains("_Tiebreak")
                                    && (!x.InnerText.Trim().Equals("")))
                                {
                                    result.Set2TBScore = Int32.Parse(x.InnerText);
                                }
                            }
                            break;
                        case 3:
                            result.Set3LoseScore = Int32.Parse(node.InnerText);
                            if (node.NextSibling != null && node.NextSibling.NextSibling != null)
                            {
                                var x = node.NextSibling.NextSibling;
                                if (x.Id.Contains("_Tiebreak")
                                    && (!x.InnerText.Trim().Equals("")))
                                {
                                    result.Set3TBScore = Int32.Parse(x.InnerText);
                                }
                            }
                            break;
                        case 4:
                            result.Set4LoseScore = Int32.Parse(node.InnerText);
                            if (node.NextSibling != null && node.NextSibling.NextSibling != null)
                            {
                                var x = node.NextSibling.NextSibling;
                                if (x.Id.Contains("_Tiebreak")
                                    && (!x.InnerText.Trim().Equals("")))
                                {
                                    result.Set4TBScore = Int32.Parse(x.InnerText);
                                }
                            }
                            break;
                        default:
                            result.Set5LoseScore = Int32.Parse(node.InnerText);
                            if (node.NextSibling != null && node.NextSibling.NextSibling != null)
                            {
                                var x = node.NextSibling.NextSibling;
                                if (x.Id.Contains("_Tiebreak")
                                    && (!x.InnerText.Trim().Equals("")))
                                {
                                    result.Set5TBScore = Int32.Parse(x.InnerText);
                                }
                            }
                            break;
                    }
                    set++;
                }
                #endregion

                MatchCollection m = Regex.Matches(html, @"FirstServeDivisor.*\s(\d+),");
                result.WinTotalServe = Int32.Parse(m[0].Groups[1].Value);
                result.LoseTotalServe = Int32.Parse(m[1].Groups[1].Value);

                m = Regex.Matches(html, @"FirstServeDividend.*\s(\d+),");
                result.Win1stIn = Int32.Parse(m[0].Groups[1].Value);
                result.Lose1stIn = Int32.Parse(m[1].Groups[1].Value);

                m = Regex.Matches(html, @"FirstServePointsWonDividend.*\s(\d+),");
                result.Win1stWon = Int32.Parse(m[0].Groups[1].Value);
                result.Lose1stWon = Int32.Parse(m[1].Groups[1].Value);

                m = Regex.Matches(html, @"SecondServePointsWonDividend.*\s(\d+),");
                result.Win2ndWon = Int32.Parse(m[0].Groups[1].Value);
                result.Lose2ndWon = Int32.Parse(m[1].Groups[1].Value);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<PlayersResult> GetATPPlayersResult(string url)
        {
            List<PlayersResult> result = new List<PlayersResult>();
            string html = GetGZipHTML(url, true);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var root = doc.DocumentNode;
            var nodes = root.Descendants("span")
                .Where(n => n.InnerText.Trim().Equals("Defeats"));
            foreach (var node in nodes)
            {
                try
                {
                    PlayersResult r = new PlayersResult();
                    var x = node.ParentNode.PreviousSibling.PreviousSibling.FirstChild.NextSibling;
                    r.WinnerName = x.InnerText;
                    r.WinnerCode2 = x.GetAttributeValue("href", "").Split('/')[4];
                    x = node.ParentNode.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.FirstChild.NextSibling;
                    r.LoserName = x.InnerText;
                    r.LoserCode2 = x.GetAttributeValue("href", "").Split('/')[4];
                    x = node.ParentNode.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.FirstChild.NextSibling;
                    r.MatchURL = ATPBaseURL + x.GetAttributeValue("href", "");
                    string score = x.InnerText;
                    if (!score.ToUpper().Contains("RET"))
                    {
                        result.Add(r);
                    }
                }
                catch (Exception)
                {

                }
            }
            return result;
        }
    }
}
