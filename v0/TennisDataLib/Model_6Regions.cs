using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;

namespace TennisData
{
    public class Model_6Regions
    {
        public static string Log1(string ply1Name, string ply2Names)
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HHmmss_fff_") + ply1Name + ".log";
            string logDir = ConfigurationManager.AppSettings["LogDirectory"];
            using (StreamWriter logFile = new StreamWriter(logDir + fileName))
            {
                logFile.WriteLine(ply1Name);
                logFile.WriteLine(ply2Names);

            }
            return fileName;
        }

        public static void Log2(string logFileName, string match)
        {
            string logDir = ConfigurationManager.AppSettings["LogDirectory"];
            using (StreamWriter logFile = File.AppendText(logDir + logFileName))
            {
                logFile.WriteLine(match);
            }
        }

        public static void Log2(string logFileName, double[] result)
        {
            string logDir = ConfigurationManager.AppSettings["LogDirectory"];
            using (StreamWriter logFile = File.AppendText(logDir + logFileName))
            {
                logFile.WriteLine("[{0}, {1}]", result[0], result[1]);
            }
        }

        public static double[] Predict(string ply1Name, List<string> ply2Names, DateTime dateFrom, DateTime dateTo, out Dictionary<string, int> nums)
        {
            string logFileName = Log1(ply1Name, string.Join(", ", ply2Names));
            double[] result = { 0, 0 };
            nums = new Dictionary<string, int>();
            if (ply2Names.Count < 1) return result;
            Player ply1 = WebFunction.GetPlayerInfo(ply1Name);
            Player ply2_0 = WebFunction.GetPlayerInfo(ply2Names[0]);
            if (ply1.Hand == 99 || ply2_0.Hand == 99) return result;
            Dictionary<string, MatchInfo> matches = WebFunction.GetMatchInfoList();
            string modelDir = ConfigurationManager.AppSettings["ModelTemplateDirectory"];
            string pcspDir = ConfigurationManager.AppSettings["PcspOutputDirectory"];
            string tempFile = pcspDir + "Temp-" + System.Guid.NewGuid().ToString() + ".pcsp";
            #region RH-RH
            if (ply1.Hand == Stroke.RIGHT_HAND && ply2_0.Hand == Stroke.RIGHT_HAND)
            {
                foreach (string ply2Name in ply2Names)
                {
                    var filtered = from kvp in matches
                                   where kvp.Value.Player1.Name.Equals(ply1Name)
                                           && kvp.Value.Player2.Name.Equals(ply2Name)
                                           && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                           && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                                   select kvp;
                    foreach (var m in filtered)
                    {
                        Log2(logFileName, m.Key);
                        if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-RH-6.data")))
                        {
                            nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-RH-6.data")));
                        }
                        else
                        {
                            List<TennisData.Point> points = new List<TennisData.Point>();
                            points = WebFunction.GetPointLog(m.Key);
                            if (points.Count > 0)
                            {
                                List<TennisData.Stroke> strokes = new List<Stroke>();
                                foreach (TennisData.Point p in points)
                                {
                                    List<Stroke> temp = p.ExtractStrokes();
                                    List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                                    strokes.AddRange(converted);
                                }
                                if (strokes.Count(s =>
                                    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                    && !s.Description.Trim().ToLower().Equals("unknown")
                                    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                                {
                                    continue;
                                }
                                Dictionary<string, int> Nums = new Dictionary<string, int>();
                                Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                                PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-RH-6.data"), Nums);
                                nums = PATFunction.Sum(nums, Nums);
                            }
                        }
                    }
                    filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply2Name)
                                       && kvp.Value.Player2.Name.Equals(ply1Name)
                                       && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                       && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                    foreach (var m in filtered)
                    {
                        Log2(logFileName, m.Key);
                        if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-RH-6.data")))
                        {
                            nums = PATFunction.Sum(nums, SwapPlys(PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-RH-6.data"))));
                        }
                        else
                        {
                            List<TennisData.Point> points = new List<TennisData.Point>();
                            points = WebFunction.GetPointLog(m.Key);
                            if (points.Count > 0)
                            {
                                List<TennisData.Stroke> strokes = new List<Stroke>();
                                foreach (TennisData.Point p in points)
                                {
                                    List<Stroke> temp = p.ExtractStrokes();
                                    List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                                    strokes.AddRange(converted);
                                }
                                if (strokes.Count(s =>
                                    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                    && !s.Description.Trim().ToLower().Equals("unknown")
                                    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                                {
                                    continue;
                                }
                                Dictionary<string, int> Nums = new Dictionary<string, int>();
                                Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                                PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-RH-6.data"), Nums);
                                nums = PATFunction.Sum(nums, SwapPlys(Nums));
                                //foreach(var kvp in nums)
                                //{
                                //    if (!Nums.Keys.Contains(kvp.Key))
                                //    {
                                //        Console.WriteLine(kvp.Key);
                                //    }
                                //}
                            }
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Right Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Names[0] + " ...");
            }
            #endregion
            #region RH-LH
            else if (ply1.Hand == Stroke.RIGHT_HAND && ply2_0.Hand != Stroke.RIGHT_HAND)
            {
                foreach (string ply2Name in ply2Names)
                {
                    var filtered = from kvp in matches
                                   where kvp.Value.Player1.Name.Equals(ply1Name)
                                           && kvp.Value.Player2.Name.Equals(ply2Name)
                                           && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                           && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                                   select kvp;
                    foreach (var m in filtered)
                    {
                        Log2(logFileName, m.Key);
                        if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                        {
                            nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                        }
                        else
                        {
                            List<TennisData.Point> points = new List<TennisData.Point>();
                            points = WebFunction.GetPointLog(m.Key);
                            if (points.Count > 0)
                            {
                                List<TennisData.Stroke> strokes = new List<Stroke>();
                                foreach (TennisData.Point p in points)
                                {
                                    List<Stroke> temp = p.ExtractStrokes();
                                    List<Stroke> converted = Model_RH_LH_6Regions.Convert(temp);
                                    strokes.AddRange(converted);
                                }
                                if (strokes.Count(s =>
                                    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                    && !s.Description.Trim().ToLower().Equals("unknown")
                                    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                                {
                                    continue;
                                }
                                Dictionary<string, int> Nums = new Dictionary<string, int>();
                                Nums = Model_RH_LH_6Regions.CountEvents(strokes);
                                PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                                nums = PATFunction.Sum(nums, Nums);
                            }
                        }
                    }
                    filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply2Name)
                                       && kvp.Value.Player2.Name.Equals(ply1Name)
                                       && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                       && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                    foreach (var m in filtered)
                    {
                        Log2(logFileName, m.Key);
                        if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                        {
                            nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                        }
                        else
                        {
                            List<TennisData.Point> points = new List<TennisData.Point>();
                            points = WebFunction.GetPointLog(m.Key);
                            if (points.Count > 0)
                            {
                                List<TennisData.Stroke> strokes = new List<Stroke>();
                                foreach (TennisData.Point p in points)
                                {
                                    List<Stroke> temp = p.ExtractStrokes();
                                    List<Stroke> converted = Model_LH_RH_6Regions.Convert(temp);
                                    strokes.AddRange(converted);
                                }
                                if (strokes.Count(s =>
                                    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                    && !s.Description.Trim().ToLower().Equals("unknown")
                                    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                                {
                                    continue;
                                }
                                Dictionary<string, int> Nums = new Dictionary<string, int>();
                                Nums = Model_LH_RH_6Regions.CountEvents(strokes);
                                PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                                nums = PATFunction.Sum(nums, Nums);
                            }
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Names[0] + " ...");
            }
            #endregion
            #region LH-RH
            else if (ply1.Hand != Stroke.RIGHT_HAND && ply2_0.Hand == Stroke.RIGHT_HAND)
            {
                foreach (string ply2Name in ply2Names)
                {
                    var filtered = from kvp in matches
                                   where kvp.Value.Player1.Name.Equals(ply2Name)
                                           && kvp.Value.Player2.Name.Equals(ply1Name)
                                           && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                           && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                                   select kvp;
                    foreach (var m in filtered)
                    {
                        Log2(logFileName, m.Key);
                        if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                        {
                            nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                        }
                        else
                        {
                            List<TennisData.Point> points = new List<TennisData.Point>();
                            points = WebFunction.GetPointLog(m.Key);
                            if (points.Count > 0)
                            {
                                List<TennisData.Stroke> strokes = new List<Stroke>();
                                foreach (TennisData.Point p in points)
                                {
                                    List<Stroke> temp = p.ExtractStrokes();
                                    List<Stroke> converted = Model_RH_LH_6Regions.Convert(temp);
                                    strokes.AddRange(converted);
                                }
                                if (strokes.Count(s =>
                                    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                    && !s.Description.Trim().ToLower().Equals("unknown")
                                    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                                {
                                    continue;
                                }
                                Dictionary<string, int> Nums = new Dictionary<string, int>();
                                Nums = Model_RH_LH_6Regions.CountEvents(strokes);
                                PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                                nums = PATFunction.Sum(nums, Nums);
                            }
                        }
                    }
                    filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply1Name)
                                       && kvp.Value.Player2.Name.Equals(ply2Name)
                                       && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                       && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                    foreach (var m in filtered)
                    {
                        Log2(logFileName, m.Key);
                        if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                        {
                            nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                        }
                        else
                        {
                            List<TennisData.Point> points = new List<TennisData.Point>();
                            points = WebFunction.GetPointLog(m.Key);
                            if (points.Count > 0)
                            {
                                List<TennisData.Stroke> strokes = new List<Stroke>();
                                foreach (TennisData.Point p in points)
                                {
                                    List<Stroke> temp = p.ExtractStrokes();
                                    List<Stroke> converted = Model_LH_RH_6Regions.Convert(temp);
                                    strokes.AddRange(converted);
                                }
                                if (strokes.Count(s =>
                                    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                    && !s.Description.Trim().ToLower().Equals("unknown")
                                    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                                {
                                    continue;
                                }
                                Dictionary<string, int> Nums = new Dictionary<string, int>();
                                Nums = Model_LH_RH_6Regions.CountEvents(strokes);
                                PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                                nums = PATFunction.Sum(nums, Nums);
                            }
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply2Names[0] + " ...", ply1Name);
            }
            #endregion
            #region LH-LH
            else
            {
                foreach (string ply2Name in ply2Names)
                {
                    var filtered = from kvp in matches
                                   where kvp.Value.Player1.Name.Equals(ply1Name)
                                           && kvp.Value.Player2.Name.Equals(ply2Name)
                                           && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                           && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                                   select kvp;
                    foreach (var m in filtered)
                    {
                        Log2(logFileName, m.Key);
                        if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".LH-LH-6.data")))
                        {
                            nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".LH-LH-6.data")));
                        }
                        else
                        {
                            List<TennisData.Point> points = new List<TennisData.Point>();
                            points = WebFunction.GetPointLog(m.Key);
                            if (points.Count > 0)
                            {
                                List<TennisData.Stroke> strokes = new List<Stroke>();
                                foreach (TennisData.Point p in points)
                                {
                                    List<Stroke> temp = p.ExtractStrokes();
                                    List<Stroke> converted = Model_LH_LH_6Regions.Convert(temp);
                                    strokes.AddRange(converted);
                                }
                                if (strokes.Count(s =>
                                    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                    && !s.Description.Trim().ToLower().Equals("unknown")
                                    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                                {
                                    continue;
                                }
                                Dictionary<string, int> Nums = new Dictionary<string, int>();
                                Nums = Model_LH_LH_6Regions.CountEvents(strokes);
                                PATFunction.SaveWeight(m.Key.Replace(".html", ".LH-LH-6.data"), Nums);
                                nums = PATFunction.Sum(nums, Nums);
                            }
                        }
                    }
                    filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply2Name)
                                       && kvp.Value.Player2.Name.Equals(ply1Name)
                                       && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                       && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                    foreach (var m in filtered)
                    {
                        Log2(logFileName, m.Key);
                        if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".LH-LH-6.data")))
                        {
                            nums = PATFunction.Sum(nums, SwapPlys(PATFunction.LoadWeight(m.Key.Replace(".html", ".LH-LH-6.data"))));
                        }
                        else
                        {
                            List<TennisData.Point> points = new List<TennisData.Point>();
                            points = WebFunction.GetPointLog(m.Key);
                            if (points.Count > 0)
                            {
                                List<TennisData.Stroke> strokes = new List<Stroke>();
                                foreach (TennisData.Point p in points)
                                {
                                    List<Stroke> temp = p.ExtractStrokes();
                                    List<Stroke> converted = Model_LH_LH_6Regions.Convert(temp);
                                    strokes.AddRange(converted);
                                }
                                if (strokes.Count(s =>
                                    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                    && !s.Description.Trim().ToLower().Equals("unknown")
                                    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                                {
                                    continue;
                                }
                                Dictionary<string, int> Nums = new Dictionary<string, int>();
                                Nums = Model_LH_LH_6Regions.CountEvents(strokes);
                                PATFunction.SaveWeight(m.Key.Replace(".html", ".LH-LH-6.data"), Nums);
                                nums = PATFunction.Sum(nums, SwapPlys(Nums));
                                //foreach (var kvp in nums)
                                //{
                                //    if (!Nums.Keys.Contains(kvp.Key))
                                //    {
                                //        Console.WriteLine(kvp.Key);
                                //    }
                                //}
                            }
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Left-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Names[0] + " ...");
            }
            #endregion

            if (nums.Count > 0)
            {
                result = PATFunction.GetPATResult(tempFile);
            }
            Log2(logFileName, result);
            return result;
        }

        public static double[] PredictWithoutValidationByCompare(string ply1Name, string ply2Name, DateTime dateFrom, DateTime dateTo, out Dictionary<string, int> nums)
        {
            const string ALL_RIGHT = "all right";
            const string ALL_LEFT = "all left";
            const int N = 75;  // divide players into N bands based on ranking
            const int M = 5;  // Need at least M bands to compare the winning chance
            const int MAX_RANKING = 1500;
            string logFileName = Log1(ply1Name, ply2Name + " WITHOUT VALIDATION BY COMPARE");
            Log2(logFileName, string.Format("N={0}, M={1}, MAX_RANKING={2}", N, M, MAX_RANKING));
            double[] result = { 0, 0 };
            nums = new Dictionary<string, int>();
            if (ply1Name.ToLower().Equals(ply2Name.ToLower()))
            {
                result = new double[] { 0.5, 0.5 };
                return result;
            }
            Player ply1 = WebFunction.GetPlayerInfo(ply1Name);
            Player ply2 = WebFunction.GetPlayerInfo(ply2Name);
            if (ply1.Hand == 99 || ply2.Hand == 99) return result;
            Dictionary<string, MatchInfo> matches = WebFunction.GetMatchInfoList();
            string modelDir = ConfigurationManager.AppSettings["ModelTemplateDirectory"];
            string pcspDir = ConfigurationManager.AppSettings["PcspOutputDirectory"];
            string tempFile = pcspDir + "Temp-" + System.Guid.NewGuid().ToString() + ".pcsp";
            #region RH-RH
            if (ply1.Hand == Stroke.RIGHT_HAND && ply2.Hand == Stroke.RIGHT_HAND)
            {
                Dictionary<string, Int16> ranking = DatabaseFunction.GetPlayerRanking(Stroke.RIGHT_HAND);
                string[] ply1Matches = new string[N];
                for (int i = 0; i < N; i++)
                {
                    foreach (var e in matches.OrderByDescending(x => x.Key))
                    {
                        if (e.Value.Player1.Name.Equals(ply1Name)
                            && e.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                            && e.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                            && ranking.ContainsKey(e.Value.Player2.Name)
                            && ranking[e.Value.Player2.Name] >= (i * MAX_RANKING / N)
                            && ranking[e.Value.Player2.Name] < ((i + 1) * MAX_RANKING / N))
                        {
                            ply1Matches[i] = e.Key;
                            Log2(logFileName, string.Format("{0}, Ranking={1}", e.Key, ranking[e.Value.Player2.Name]));
                            break;
                        }
                        if (e.Value.Player2.Name.Equals(ply1Name)
                            && e.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                            && e.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                            && ranking.ContainsKey(e.Value.Player1.Name)
                            && ranking[e.Value.Player1.Name] >= (i * MAX_RANKING / N)
                            && ranking[e.Value.Player1.Name] < ((i + 1) * MAX_RANKING / N))
                        {
                            ply1Matches[i] = e.Key;
                            Log2(logFileName, string.Format("{0}, Ranking={1}", e.Key, ranking[e.Value.Player1.Name]));
                            break;
                        }
                    }
                }
                string[] ply2Matches = new string[N];
                for (int i = 0; i < N; i++)
                {
                    foreach (var e in matches.OrderByDescending(x => x.Key))
                    {
                        if (e.Value.Player1.Name.Equals(ply2Name)
                            && e.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                            && e.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                            && ranking.ContainsKey(e.Value.Player2.Name)
                            && ranking[e.Value.Player2.Name] >= (i * MAX_RANKING / N)
                            && ranking[e.Value.Player2.Name] < ((i + 1) * MAX_RANKING / N))
                        {
                            ply2Matches[i] = e.Key;
                            Log2(logFileName, string.Format("{0}, Ranking={1}", e.Key, ranking[e.Value.Player2.Name]));
                            break;
                        }
                        if (e.Value.Player2.Name.Equals(ply2Name)
                            && e.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                            && e.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                            && ranking.ContainsKey(e.Value.Player1.Name)
                            && ranking[e.Value.Player1.Name] >= (i * MAX_RANKING / N)
                            && ranking[e.Value.Player1.Name] < ((i + 1) * MAX_RANKING / N))
                        {
                            ply2Matches[i] = e.Key;
                            Log2(logFileName, string.Format("{0}, Ranking={1}", e.Key, ranking[e.Value.Player1.Name]));
                            break;
                        }
                    }
                }
                int c = 0;
                for (int i = 0; i < N; i++)
                {
                    if (ply1Matches[i] != null && ply2Matches[i] != null)
                    {
                        c++;
                    }
                    else if (ply1Matches[i] != null && ply2Matches[i] == null)
                    {
                        Log2(logFileName, ply1Matches[i] + " removed");
                        ply1Matches[i] = null;
                    }
                    else if (ply1Matches[i] == null && ply2Matches[i] != null)
                    {
                        Log2(logFileName, ply2Matches[i] + " removed");
                        ply2Matches[i] = null;
                    }
                }
                if (c < M) return result;
                var filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply1Name)
                                     && ply1Matches.Contains(kvp.Key)
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-RH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-RH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-RH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player2.Name.Equals(ply1Name)
                                 && ply1Matches.Contains(kvp.Key)
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-RH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, SwapPlys(PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-RH-6.data"))));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-RH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, SwapPlys(Nums));
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Right Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Name);
                double[] result1 = PATFunction.GetPATResult(tempFile);
                Log2(logFileName, result1);

                filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply2Name)
                                     && ply2Matches.Contains(kvp.Key)
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-RH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-RH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-RH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player2.Name.Equals(ply2Name)
                                 && ply2Matches.Contains(kvp.Key)
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-RH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, SwapPlys(PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-RH-6.data"))));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-RH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, SwapPlys(Nums));
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Right Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Name);
                double[] result2 = PATFunction.GetPATResult(tempFile);
                Log2(logFileName, result2);

                result[0] = result1[0] / (result1[0] + result2[0]);
                result[1] = result1[1] / (result1[1] + result2[1]);
                if (result[0] > result[1])
                {
                    double temp = result[1];
                    result[1] = result[0];
                    result[0] = temp;
                }
            }
            #endregion
            #region RH-LH
            else if (ply1.Hand == Stroke.RIGHT_HAND && ply2.Hand != Stroke.RIGHT_HAND)
            {
                var filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply1Name)
                                     && (kvp.Value.Player2.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player2.Hand == Stroke.LEFT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_LH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player2.Name.Equals(ply1Name)
                                 && (kvp.Value.Player1.Name.Equals(ply2Name) ||
                                    (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player1.Hand == Stroke.LEFT_HAND))
                                 && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                 && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_RH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Name);
            }
            #endregion
            #region LH-RH
            else if (ply1.Hand != Stroke.RIGHT_HAND && ply2.Hand == Stroke.RIGHT_HAND)
            {
                var filtered = from kvp in matches
                               where kvp.Value.Player2.Name.Equals(ply1Name)
                                     && (kvp.Value.Player1.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_RIGHT) && kvp.Value.Player1.Hand == Stroke.RIGHT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_LH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player1.Name.Equals(ply1Name)
                                 && (kvp.Value.Player2.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_RIGHT) && kvp.Value.Player2.Hand == Stroke.RIGHT_HAND))
                                 && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                 && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_RH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply2Name, ply1Name);
            }
            #endregion
            #region LH-LH
            else
            {
                var filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply1Name)
                                     && (kvp.Value.Player2.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player2.Hand == Stroke.LEFT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".LH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".LH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_LH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".LH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player2.Name.Equals(ply1Name)
                                 && (kvp.Value.Player1.Name.Equals(ply2Name) ||
                                    (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player1.Hand == Stroke.LEFT_HAND))
                                 && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                 && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".LH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, SwapPlys(PATFunction.LoadWeight(m.Key.Replace(".html", ".LH-LH-6.data"))));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_LH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".LH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, SwapPlys(Nums));
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Left-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Name);
            }
            #endregion
            Log2(logFileName, result);
            return result;
        }

        public static double[] PredictWithoutValidation(string ply1Name, string ply2Name, DateTime dateFrom, DateTime dateTo, out Dictionary<string, int> nums)
        {
            const string ALL_RIGHT = "all right";
            const string ALL_LEFT = "all left";
            string logFileName = Log1(ply1Name, ply2Name + " WITHOUT VALIDATION");
            double[] result = { 0, 0 };
            nums = new Dictionary<string, int>();
            if (ply1Name.ToLower().Equals(ply2Name.ToLower()))
            {
                result = new double[] { 0.5, 0.5 };
                return result;
            }
            Player ply1 = WebFunction.GetPlayerInfo(ply1Name);
            Player ply2 = WebFunction.GetPlayerInfo(ply2Name);
            if (ply1.Hand == 99 || ply2.Hand == 99) return result;
            Dictionary<string, MatchInfo> matches = WebFunction.GetMatchInfoList();
            string modelDir = ConfigurationManager.AppSettings["ModelTemplateDirectory"];
            string pcspDir = ConfigurationManager.AppSettings["PcspOutputDirectory"];
            string tempFile = pcspDir + "Temp-" + System.Guid.NewGuid().ToString() + ".pcsp";
            #region RH-RH
            if (ply1.Hand == Stroke.RIGHT_HAND && ply2.Hand == Stroke.RIGHT_HAND)
            {
                var filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply1Name)
                                     && (kvp.Value.Player2.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_RIGHT) && kvp.Value.Player2.Hand == Stroke.RIGHT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-RH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-RH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-RH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player2.Name.Equals(ply1Name)
                                 && (kvp.Value.Player1.Name.Equals(ply2Name) ||
                                    (ply2Name.ToLower().StartsWith(ALL_RIGHT) && kvp.Value.Player1.Hand == Stroke.RIGHT_HAND))
                                 && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                 && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-RH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, SwapPlys(PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-RH-6.data"))));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-RH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, SwapPlys(Nums));
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Right Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Name);
            }
            #endregion
            #region RH-LH
            else if (ply1.Hand == Stroke.RIGHT_HAND && ply2.Hand != Stroke.RIGHT_HAND)
            {
                var filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply1Name)
                                     && (kvp.Value.Player2.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player2.Hand == Stroke.LEFT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_LH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player2.Name.Equals(ply1Name)
                                 && (kvp.Value.Player1.Name.Equals(ply2Name) ||
                                    (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player1.Hand == Stroke.LEFT_HAND))
                                 && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                 && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_RH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Name);
            }
            #endregion
            #region LH-RH
            else if (ply1.Hand != Stroke.RIGHT_HAND && ply2.Hand == Stroke.RIGHT_HAND)
            {
                var filtered = from kvp in matches
                               where kvp.Value.Player2.Name.Equals(ply1Name)
                                     && (kvp.Value.Player1.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_RIGHT) && kvp.Value.Player1.Hand == Stroke.RIGHT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_LH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player1.Name.Equals(ply1Name)
                                 && (kvp.Value.Player2.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_RIGHT) && kvp.Value.Player2.Hand == Stroke.RIGHT_HAND))
                                 && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                 && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_RH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply2Name, ply1Name);
            }
            #endregion
            #region LH-LH
            else
            {
                var filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply1Name)
                                     && (kvp.Value.Player2.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player2.Hand == Stroke.LEFT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".LH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".LH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_LH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".LH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player2.Name.Equals(ply1Name)
                                 && (kvp.Value.Player1.Name.Equals(ply2Name) ||
                                    (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player1.Hand == Stroke.LEFT_HAND))
                                 && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                 && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".LH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, SwapPlys(PATFunction.LoadWeight(m.Key.Replace(".html", ".LH-LH-6.data"))));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            //if (strokes.Count(s =>
                            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            //    && !s.Description.Trim().ToLower().Equals("unknown")
                            //    && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            //{
                            //    continue;
                            //}
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_LH_6Regions.CountEvents(strokes);
                            //PATFunction.SaveWeight(m.Key.Replace(".html", ".LH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, SwapPlys(Nums));
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Left-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Name);
            }
            #endregion
            if (nums.Count > 0)
            {
                result = PATFunction.GetPATResult(tempFile);
            }
            Log2(logFileName, result);
            return result;
        }

        public static double[] Predict(string ply1Name, string ply2Name, DateTime dateFrom, DateTime dateTo, out Dictionary<string, int> nums)
        {
            const string ALL_RIGHT = "all right";
            const string ALL_LEFT = "all left";
            string logFileName = Log1(ply1Name, ply2Name);
            double[] result = { 0, 0 };
            nums = new Dictionary<string, int>();
            if (ply1Name.ToLower().Equals(ply2Name.ToLower()))
            {
                result = new double[]{ 0.5, 0.5};
                return result;
            }
            Player ply1 = WebFunction.GetPlayerInfo(ply1Name);
            Player ply2 = WebFunction.GetPlayerInfo(ply2Name);
            if (ply1.Hand == 99 || ply2.Hand == 99) return result;
            Dictionary<string, MatchInfo> matches = WebFunction.GetMatchInfoList();
            string modelDir = ConfigurationManager.AppSettings["ModelTemplateDirectory"];
            string pcspDir = ConfigurationManager.AppSettings["PcspOutputDirectory"];
            string tempFile = pcspDir + "Temp-" + System.Guid.NewGuid().ToString() + ".pcsp";
            #region RH-RH
            if (ply1.Hand == Stroke.RIGHT_HAND && ply2.Hand == Stroke.RIGHT_HAND)
            {
                var filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply1Name)
                                     && (kvp.Value.Player2.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_RIGHT) && kvp.Value.Player2.Hand == Stroke.RIGHT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-RH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-RH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            if (strokes.Count(s =>
                                (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                && !s.Description.Trim().ToLower().Equals("unknown")
                                && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            {
                                continue;
                            }
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                            PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-RH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player2.Name.Equals(ply1Name)
                                 && (kvp.Value.Player1.Name.Equals(ply2Name) ||
                                    (ply2Name.ToLower().StartsWith(ALL_RIGHT) && kvp.Value.Player1.Hand == Stroke.RIGHT_HAND))
                                 && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                 && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-RH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, SwapPlys(PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-RH-6.data"))));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            if (strokes.Count(s =>
                                (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                && !s.Description.Trim().ToLower().Equals("unknown")
                                && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            {
                                continue;
                            }
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                            PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-RH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, SwapPlys(Nums));
                            //foreach(var kvp in nums)
                            //{
                            //    if (!Nums.Keys.Contains(kvp.Key))
                            //    {
                            //        Console.WriteLine(kvp.Key);
                            //    }
                            //}
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Right Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Name);
            }
            #endregion
            #region RH-LH
            else if (ply1.Hand == Stroke.RIGHT_HAND && ply2.Hand != Stroke.RIGHT_HAND)
            {
                var filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply1Name) 
                                     && (kvp.Value.Player2.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player2.Hand == Stroke.LEFT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            if (strokes.Count(s =>
                                (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                && !s.Description.Trim().ToLower().Equals("unknown")
                                && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            {
                                continue;
                            }
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_LH_6Regions.CountEvents(strokes);
                            PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                               where kvp.Value.Player2.Name.Equals(ply1Name)
                                     && (kvp.Value.Player1.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player1.Hand == Stroke.LEFT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            if (strokes.Count(s =>
                                (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                && !s.Description.Trim().ToLower().Equals("unknown")
                                && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            {
                                continue;
                            }
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_RH_6Regions.CountEvents(strokes);
                            PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Name);
            }
            #endregion
            #region LH-RH
            else if (ply1.Hand != Stroke.RIGHT_HAND && ply2.Hand == Stroke.RIGHT_HAND)
            {
                var filtered = from kvp in matches
                               where kvp.Value.Player2.Name.Equals(ply1Name)
                                     && (kvp.Value.Player1.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_RIGHT) && kvp.Value.Player1.Hand == Stroke.RIGHT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_RH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            if (strokes.Count(s =>
                                (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                && !s.Description.Trim().ToLower().Equals("unknown")
                                && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            {
                                continue;
                            }
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_RH_LH_6Regions.CountEvents(strokes);
                            PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player1.Name.Equals(ply1Name)
                                 && (kvp.Value.Player2.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_RIGHT) && kvp.Value.Player2.Hand == Stroke.RIGHT_HAND))
                                 && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                 && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".RH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".RH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_RH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            if (strokes.Count(s =>
                                (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                && !s.Description.Trim().ToLower().Equals("unknown")
                                && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            {
                                continue;
                            }
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_RH_6Regions.CountEvents(strokes);
                            PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Right-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply2Name, ply1Name);
            }
            #endregion
            #region LH-LH
            else
            {
                var filtered = from kvp in matches
                               where kvp.Value.Player1.Name.Equals(ply1Name)
                                     && (kvp.Value.Player2.Name.Equals(ply2Name) ||
                                        (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player2.Hand == Stroke.LEFT_HAND))
                                     && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                     && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                               select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".LH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, PATFunction.LoadWeight(m.Key.Replace(".html", ".LH-LH-6.data")));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            if (strokes.Count(s =>
                                (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                && !s.Description.Trim().ToLower().Equals("unknown")
                                && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            {
                                continue;
                            }
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_LH_6Regions.CountEvents(strokes);
                            PATFunction.SaveWeight(m.Key.Replace(".html", ".LH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, Nums);
                        }
                    }
                }
                filtered = from kvp in matches
                           where kvp.Value.Player2.Name.Equals(ply1Name)
                                 && (kvp.Value.Player1.Name.Equals(ply2Name) ||
                                    (ply2Name.ToLower().StartsWith(ALL_LEFT) && kvp.Value.Player1.Hand == Stroke.LEFT_HAND))
                                 && kvp.Key.CompareTo(dateFrom.ToString("yyyyMMdd")) > 0
                                 && kvp.Key.CompareTo(dateTo.AddDays(1).ToString("yyyyMMdd")) < 0
                           select kvp;
                foreach (var m in filtered)
                {
                    Log2(logFileName, m.Key);
                    if (File.Exists(ConfigurationManager.AppSettings["LocalDataDirectory"] + m.Key.Replace(".html", ".LH-LH-6.data")))
                    {
                        nums = PATFunction.Sum(nums, SwapPlys(PATFunction.LoadWeight(m.Key.Replace(".html", ".LH-LH-6.data"))));
                    }
                    else
                    {
                        List<TennisData.Point> points = new List<TennisData.Point>();
                        points = WebFunction.GetPointLog(m.Key);
                        if (points.Count > 0)
                        {
                            List<TennisData.Stroke> strokes = new List<Stroke>();
                            foreach (TennisData.Point p in points)
                            {
                                List<Stroke> temp = p.ExtractStrokes();
                                List<Stroke> converted = Model_LH_LH_6Regions.Convert(temp);
                                strokes.AddRange(converted);
                            }
                            if (strokes.Count(s =>
                                (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                                && !s.Description.Trim().ToLower().Equals("unknown")
                                && !s.Description.Trim().ToLower().Contains("penalty"))) > 0)
                            {
                                continue;
                            }
                            Dictionary<string, int> Nums = new Dictionary<string, int>();
                            Nums = Model_LH_LH_6Regions.CountEvents(strokes);
                            PATFunction.SaveWeight(m.Key.Replace(".html", ".LH-LH-6.data"), Nums);
                            nums = PATFunction.Sum(nums, SwapPlys(Nums));
                            //foreach (var kvp in nums)
                            //{
                            //    if (!Nums.Keys.Contains(kvp.Key))
                            //    {
                            //        Console.WriteLine(kvp.Key);
                            //    }
                            //}
                        }
                    }
                }
                PATFunction.ReplaceWeight(modelDir + @"Left-Left Model 6 Regions.pcsp",
                    tempFile, nums, ply1Name, ply2Name);
            }
            #endregion
            if (nums.Count > 0)
            {
                result = PATFunction.GetPATResult(tempFile);
            }
            Log2(logFileName, result);
            return result;
        }

        public static Dictionary<string, int> SwapPlys(Dictionary<string, int> nums)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach(KeyValuePair<string, int> kvp in nums)
            {
                if (kvp.Key.Contains("Ply1"))
                {
                    result.Add(kvp.Key.Replace("Ply1", "Ply2"), kvp.Value);
                }
                else if (kvp.Key.Contains("Ply2"))
                {
                    result.Add(kvp.Key.Replace("Ply2", "Ply1"), kvp.Value);
                }
                else
                {
                    result.Add(kvp.Key, kvp.Value);
                }
            }
            return result;
        }
    }
}
