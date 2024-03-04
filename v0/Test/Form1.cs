using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TennisData;

using System.Timers;
using System.Threading;
using System.Threading.Tasks;

using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Configuration;



namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count;
            //count = WebFunction.DownloadAllPlayerInfoToLocal();
            //count = WebFunction.DownloadMatchListToLocal();
            //MessageBox.Show(count.ToString());
            //Dictionary<string, Int16> a = DatabaseFunction.GetPlayerRanking(1);
            Dictionary<string, int> nums = null;
            //double[] test = Model_6Regions.PredictWithoutValidation("Roger Federer", "Rafael Nadal", new DateTime(2012, 03, 13), new DateTime(2012, 03, 20), out nums);
            //double[] test = Model_6Regions.PredictWithoutValidation("Marcos Giron", "Dominic Thiem", new DateTime(2021, 05, 04), new DateTime(2021, 05, 04), out nums);

            List<TennisData.Point> points = new List<TennisData.Point>();
            points = WebFunction.GetPointLog("20230403-M-Marrakech-R32-Younes_Lalami_Laaroussi-Alexei_Popyrin.html");
            if (points.Count > 0)
            {
                List<TennisData.Stroke> strokes = new List<Stroke>();
                foreach (TennisData.Point p in points)
                {
                    List<Stroke> temp = p.ExtractStrokes();
                    List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                    strokes.AddRange(converted);
                }
                TennisDataLib.ML.StrokeByStroke(@"d:\temp\20230403-M-Marrakech-R32-Younes_Lalami_Laaroussi-Alexei_Popyrin.csv", strokes);
                Dictionary<string, int> Nums = new Dictionary<string, int>();
                Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                nums = Nums;
            }
            if (nums.Count > 0)
            {
                string modelDir = System.Configuration.ConfigurationManager.AppSettings["ModelTemplateDirectory"];
                string pcspDir = System.Configuration.ConfigurationManager.AppSettings["PcspOutputDirectory"];
                string tempFile = pcspDir + "Temp-" + System.Guid.NewGuid().ToString() + ".pcsp";
                PATFunction.ReplaceWeight(modelDir + @"Right-Right Model 6 Regions.pcsp",
                    tempFile, nums, "Roger_Federer", "Dominic_Thiem");
                double[] result = { 0, 0 };
                result = PATFunction.GetPATResult(tempFile);
            }
            //double[] test = Model_6Regions.PredictWithoutValidation("Roger Federer", "Dominic Thiem", new DateTime(2019, 03, 17), new DateTime(2019, 03, 17), out nums);
            
            //MessageBox.Show(string.Format("{0} matches have been downloaded.", WebFunction.DownloadAllPlayerInfoToLocal()));
            //string[] names =
            //{
            //    "Harriet Dart",
            //    "Belinda Bencic",
            //    "Anastasija Sevastova"
            //};
            //WebFunction.DownloadAllPlayerInfoToLocal(names);
            //string[] links =
            //{
            //    "20180828-M-US_Open-R128-Yoshihito_Nishioka-Roger_Federer.html",
            //    "20180830-M-US_Open-R64-Benoit_Paire-Roger_Federer.html",
            //    "20180901-M-US_Open-R32-Roger_Federer-Nick_Kyrgios.html",
            //    "20180903-M-US_Open-R16-Roger_Federer-John_Millman.html"
            //};
            //WebFunction.DownloadMatchListToLocal(links);
            Dictionary<string, MatchInfo> matches = WebFunction.GetMatchInfoList();
            var filtered = from kvp in matches
                               //where ((kvp.Value.Player1.Name.Equals("Angelique Kerber") && kvp.Value.Player2.Name.Equals("Ekaterina Makarova"))
                               // || (kvp.Value.Player1.Name.Equals("Ekaterina Makarova") && kvp.Value.Player2.Name.Equals("Angelique Kerber")))
                           where
                                    (kvp.Value.Player1.Hand == 1 && kvp.Value.Player2.Name.Equals("Harriet Dart"))
                           //         && (kvp.Key.StartsWith("2018"))
                           //                                  || (kvp.Value.Player2.Hand == 1 && kvp.Value.Player1.Name.Equals("Mikhail Kukushkin"))
                           select kvp;

            //foreach (var ttt in filtered) Console.WriteLine("http://www.tennisabstract.com/charting/" + ttt.Key);
            int total_FH_R = 0;
            int total_FH_R_L = 0;
            int total_FH_R_R = 0;
            int total_FH_R_M = 0;
            int total_FH_R_S = 0;
            int total_FH_R_D = 0;
            int total_FH_R_VD = 0;
            int total_FH_Error = 0;
            int total_ACE_FH = 0;
            int total_ACE_Body = 0;
            int total_ACE_BH = 0;
            int total_FH = 0;
            int total_Body = 0;
            int total_BH = 0;
            //int total_AFAP = 0;
            //int total_AFAP_FH_CR = 0;
            //int total_AFAP_FH_M = 0;
            //int total_AFAP_FH_DL = 0;
            //int total_AFAP_FH_CR_Err = 0;
            //int total_AFAP_FH_M_Err = 0;
            //int total_AFAP_FH_DL_Err = 0;
            //int total_AFAP_FH_IO = 0;
            //int total_AFAP_FH_II = 0;
            //int total_AFAP_FH_IO_Err = 0;
            //int total_AFAP_FH_II_Err = 0;
            //int total_AFAP_BH_CR = 0;
            //int total_AFAP_BH_M = 0;
            //int total_AFAP_BH_DL = 0;
            //int total_AFAP_BH_CR_Err = 0;
            //int total_AFAP_BH_M_Err = 0;
            //int total_AFAP_BH_DL_Err = 0;
            //int total_AFAP_BH_IO = 0;
            //int total_AFAP_BH_II = 0;
            //int total_AFAP_BH_IO_Err = 0;
            //int total_AFAP_BH_II_Err = 0;
            foreach (var m in filtered)
            //            List<string> matches = @"20170617-M-Lisbon_CH-SF-Oscar_Otte-Attila_Balazs.html
            //20170616-M-Lyon_CH-QF-Casper_Ruud-Felix_Auger_Aliassime.html
            //20170618-W-Nottingham-F-Donna_Vekic-Johanna_Konta.html
            //20150215-M-Rotterdam-F-Stanislas_Wawrinka-Tomas_Berdych.html
            //20150614-W-s_Hertogenbosch-F-Camila_Giorgi-Belinda_Bencic.html
            //20141026-W-Singapore-F-Serena_Williams-Simona_Halep.html
            //20170315-M-Indian_Wells_Masters-R16-Rafael_Nadal-Roger_Federer.html
            //20170626-W-Birmingham-F-Petra_Kvitova-Ashleigh_Barty.html
            //20170528-W-Roland_Garros-R128-Angelique_Kerber-Ekaterina_Makarova.html
            //            20170514-M-Madrid_Masters-F-Rafael_Nadal-Dominic_Thiem.html
            //                        20140124-M-Australian_Open-SF-Roger_Federer-Rafael_Nadal.html
            //                                        20170513-M-Madrid_Masters-SF-Novak_Djokovic-Rafael_Nadal.html".Split().ToList().FindAll(x => !x.Equals(""));
            //            foreach (string m in matches)
            {
                //List<TennisData.Point> points = WebFunction.GetPointLog("20170402-M-Miami_Masters-F-Roger_Federer-Rafael_Nadal.html");
                //List<TennisData.Stroke> strokes = new List<Stroke>();
                //foreach (TennisData.Point p in points)
                //{
                //    List<Stroke> temp = p.ExtractStrokes();
                //    List<Stroke> converted = Model_RH_LH_6Regions.Convert(temp);
                //    strokes.AddRange(converted);
                //}
                //List<Stroke> errors = strokes.FindAll(s =>
                //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                //    //&& !s.Description.Trim().ToLower().Equals("unknown")
                //    //&& !s.Description.Trim().ToLower().Contains("penalty")
                //    )
                //);
                //List<Stroke> ply1_wins1 = strokes.FindAll(s =>
                //    s.Player == 1
                //    && (s.Outcome == 1 || s.Outcome == 5 || s.Outcome ==6)
                //);

                //List<Stroke> ply1_wins2 = strokes.FindAll(s =>
                //    s.Player == 2
                //    && (s.Type == Stroke.TYPE_2ND_SERVE || s.Type == Stroke.TYPE_RALLY || s.Type == Stroke.TYPE_RETURN)
                //    && (s.Outcome == 2 || s.Outcome == 3 || s.Outcome == 4)
                //);

                //List<Stroke> ply2_wins1 = strokes.FindAll(s =>
                //    s.Player == 2
                //    && (s.Outcome == 1 || s.Outcome == 5 || s.Outcome == 6)
                //);

                //List<Stroke> ply2_wins2 = strokes.FindAll(s =>
                //    s.Player == 1
                //    && (s.Type == Stroke.TYPE_2ND_SERVE || s.Type == Stroke.TYPE_RALLY || s.Type == Stroke.TYPE_RETURN)
                //    && (s.Outcome == 2 || s.Outcome == 3 || s.Outcome == 4)
                //);

                //List<Stroke> ply1_returns = strokes.FindAll(s =>
                //    s.Player == 1
                //    && s.Type == Stroke.TYPE_RETURN
                //);

                //List<Stroke> ply2_returns = strokes.FindAll(s =>
                //    s.Player == 2
                //    && s.Type == Stroke.TYPE_RETURN
                //);

                //List<Stroke> ply1_rally = strokes.FindAll(s =>
                //    s.Player == 1
                //    && s.Type == Stroke.TYPE_RALLY
                //);

                //List<Stroke> ply2_rally = strokes.FindAll(s =>
                //    s.Player == 2
                //    && s.Type == Stroke.TYPE_RALLY
                //);

                //Dictionary<string, int> Nums1 = Model_RH_LH_6Regions.CountEvents(strokes);
                //PATFunction.ReplaceWeight(@"d:\temp\Right-Left Model 6 Regions.pcsp",
                //            @"d:\temp\test.pcsp", Nums1, points[0].Match.Player1.Name, points[0].Match.Player2.Name, points[0].Match.Name);
                //double[] result = PATFunction.GetPATResult(@"d:\temp\test.pcsp");
                //MessageBox.Show(string.Format("{0} - {1}", result[0], result[1]));
                //points = WebFunction.GetPointLog("20170315-M-Indian_Wells_Masters-R16-Rafael_Nadal-Roger_Federer.html");
                //strokes = new List<Stroke>();
                //foreach (TennisData.Point p in points)
                //{
                //    List<Stroke> temp = p.ExtractStrokes();
                //    List<Stroke> converted = Model_LH_RH_6Regions.Convert(temp);
                //    strokes.AddRange(converted);
                //}
                //Dictionary<string, int> Nums2 = Model_LH_RH_6Regions.CountEvents(strokes);
                //PATFunction.ReplaceWeight(@"d:\temp\Right-Left Model 6 Regions.pcsp",
                //            @"d:\temp\test.pcsp", Nums2, points[0].Match.Player2.Name, points[0].Match.Player1.Name, points[0].Match.Name);
                //result = PATFunction.GetPATResult(@"d:\temp\test.pcsp");
                //MessageBox.Show(string.Format("{0} - {1}", result[0], result[1]));
                //Dictionary<string, int> Total_Nums = PATFunction.Sum(Nums1, Nums2);
                //PATFunction.ReplaceWeight(@"d:\temp\Right-Left Model 6 Regions.pcsp",
                //            @"d:\temp\test.pcsp", Total_Nums);
                //result = PATFunction.GetPATResult(@"d:\temp\test.pcsp");
                //MessageBox.Show(string.Format("{0} - {1}", result[0], result[1]));
                //List<TennisData.Point> points = new List<TennisData.Point>();
                points = new List<TennisData.Point>();
                points = WebFunction.GetPointLog(m.Key);
                if (points.Count > 0)
                {
                    Console.WriteLine("http://www.tennisabstract.com/charting/" + m.Key);
                    if (points[0].Match.Player1.Hand == Stroke.RIGHT_HAND && points[0].Match.Player2.Hand == Stroke.RIGHT_HAND)
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
                            var z = strokes.FindAll(s =>
                            (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
                            && !s.Description.Trim().ToLower().Equals("unknown")
                            && !s.Description.Trim().ToLower().Contains("penalty")));
                            Console.WriteLine(z.Count);
                            //continue;
                        }
                        //TennisDataLib.ML.StrokeByStroke(@"d:\temp\tennis-4.csv", strokes);
                        //var x = strokes.FindAll(s => s.Player == 2 && s.Type == Stroke.TYPE_RETURN && ((s.Prev.HitCourt == Stroke.DE_CT && s.Prev.Direction == Stroke.DIR_WIDE) || (s.Prev.HitCourt == Stroke.AD_CT && s.Prev.Direction == Stroke.DIR_T)));
                        var x = strokes.FindAll(s => s.Player == 2 && s.Type == Stroke.TYPE_RETURN && ((s.Prev.HitCourt == Stroke.DE_CT && s.Prev.Direction == Stroke.DIR_T) || (s.Prev.HitCourt == Stroke.AD_CT && s.Prev.Direction == Stroke.DIR_WIDE)));
                        //var x = strokes.FindAll(s => s.Player == 2 && s.Type == Stroke.TYPE_RETURN && ((s.Prev.HitCourt == Stroke.DE_CT && s.Prev.Direction == Stroke.DIR_BODY) || (s.Prev.HitCourt == Stroke.AD_CT && s.Prev.Direction == Stroke.DIR_BODY)));
                        total_FH_R += x.Count;
                        total_FH_R_L += x.Count(s => s.ToCourt == 1);
                        total_FH_R_M += x.Count(s => s.ToCourt == 2);
                        total_FH_R_R += x.Count(s => s.ToCourt == 3);
                        total_FH_R_S += x.Count(s => s.Depth == 1);
                        total_FH_R_D += x.Count(s => s.Depth == 2);
                        total_FH_R_VD += x.Count(s => s.Depth == 3);
                        total_FH_Error += x.Count(s => (s.Outcome == 2 || s.Outcome == 3 || s.Outcome == 4));

                        x = strokes.FindAll(s => s.Player == 1 && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE) && (s.Outcome == 1 || s.Outcome == 6)
                                                    && ((s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_WIDE) || (s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_T)));
                        total_ACE_FH += x.Count;
                        x = strokes.FindAll(s => s.Player == 1 && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE) && (s.Outcome == 1 || s.Outcome == 6)
                                                    && ((s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_T) || (s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_WIDE)));
                        total_ACE_BH += x.Count;
                        x = strokes.FindAll(s => s.Player == 1 && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE) && (s.Outcome == 1 || s.Outcome == 6)
                                                    && ((s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_BODY) || (s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_BODY)));
                        total_ACE_Body += x.Count;

                        x = strokes.FindAll(s => s.Player == 1 && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                                                    && ((s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_WIDE) || (s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_T)));
                        total_FH += x.Count;
                        x = strokes.FindAll(s => s.Player == 1 && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                                                    && ((s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_T) || (s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_WIDE)));
                        total_BH += x.Count;
                        x = strokes.FindAll(s => s.Player == 1 && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                                                    && ((s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_BODY) || (s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_BODY)));
                        total_Body += x.Count;

                        //    x = strokes.FindAll(s => s.Player == 2 && s.Prev != null && s.Prev.ApproachShot == 1);
                        //    total_AFAP += x.Count;
                        //    x = strokes.FindAll(s => s.Player == 2 && s.Prev != null && s.Prev.ApproachShot == 1
                        //                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS && s.Direction == Stroke.DIR_CROSSCOURT);
                        //    total_AFAP_FH_CR += x.Count;
                        //    total_AFAP_FH_CR_Err += x.Count(s => (s.Outcome == 3) || (s.Outcome == 4));
                        //    x = strokes.FindAll(s => s.Player == 2 && s.Prev != null && s.Prev.ApproachShot == 1
                        //                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS && s.Direction == Stroke.DIR_MIDDLE);
                        //    total_AFAP_FH_M += x.Count;
                        //    total_AFAP_FH_M_Err += x.Count(s => (s.Outcome == 3) || (s.Outcome == 4));
                        //    x = strokes.FindAll(s => s.Player == 2 && s.Prev != null && s.Prev.ApproachShot == 1
                        //                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS && s.Direction == Stroke.DIR_DOWN_LINE);
                        //    total_AFAP_FH_DL += x.Count;
                        //    total_AFAP_FH_DL_Err += x.Count(s => (s.Outcome == 3) || (s.Outcome == 4));
                        //    x = strokes.FindAll(s => s.Player == 2 && s.Prev != null && s.Prev.ApproachShot == 1
                        //                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS && s.Direction == Stroke.DIR_INSIDE_OUT);
                        //    total_AFAP_FH_IO += x.Count;
                        //    total_AFAP_FH_IO_Err += x.Count(s => (s.Outcome == 3) || (s.Outcome == 4));
                        //    x = strokes.FindAll(s => s.Player == 2 && s.Prev != null && s.Prev.ApproachShot == 1
                        //                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS && s.Direction == Stroke.DIR_INSIDE_IN);
                        //    total_AFAP_FH_II += x.Count;
                        //    total_AFAP_FH_II_Err += x.Count(s => (s.Outcome == 3) || (s.Outcome == 4));

                        //    x = strokes.FindAll(s => s.Player == 2 && s.Prev != null && s.Prev.ApproachShot == 1
                        //                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS && s.Direction == Stroke.DIR_CROSSCOURT);
                        //    total_AFAP_BH_CR += x.Count;
                        //    total_AFAP_BH_CR_Err += x.Count(s => (s.Outcome == 3) || (s.Outcome == 4));
                        //    x = strokes.FindAll(s => s.Player == 2 && s.Prev != null && s.Prev.ApproachShot == 1
                        //                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS && s.Direction == Stroke.DIR_MIDDLE);
                        //    total_AFAP_BH_M += x.Count;
                        //    total_AFAP_BH_M_Err += x.Count(s => (s.Outcome == 3) || (s.Outcome == 4));
                        //    x = strokes.FindAll(s => s.Player == 2 && s.Prev != null && s.Prev.ApproachShot == 1
                        //                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS && s.Direction == Stroke.DIR_DOWN_LINE);
                        //    total_AFAP_BH_DL += x.Count;
                        //    total_AFAP_BH_DL_Err += x.Count(s => (s.Outcome == 3) || (s.Outcome == 4));
                        //    x = strokes.FindAll(s => s.Player == 2 && s.Prev != null && s.Prev.ApproachShot == 1
                        //                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS && s.Direction == Stroke.DIR_INSIDE_OUT);
                        //    total_AFAP_BH_IO += x.Count;
                        //    total_AFAP_BH_IO_Err += x.Count(s => (s.Outcome == 3) || (s.Outcome == 4));
                        //    x = strokes.FindAll(s => s.Player == 2 && s.Prev != null && s.Prev.ApproachShot == 1
                        //                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS && s.Direction == Stroke.DIR_INSIDE_IN);
                        //    total_AFAP_BH_II += x.Count;
                        //    total_AFAP_BH_II_Err += x.Count(s => (s.Outcome == 3) || (s.Outcome == 4));

                        //Dictionary < string, int> Nums = new Dictionary<string, int>();
                        //Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                        ////PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-RH-6.data"), Nums);
                        //try
                        //{
                        //    TennisDataLib.ML.PointByPoint(@"d:\temp\tennis-2.csv", points[0].Match, points);
                        //}
                        //catch { }
                        //PATFunction.ReplaceWeight(@"d:\temp\Right-Right Model 6 Regions.pcsp",
                        //    @"d:\temp\test.pcsp", Nums, points[0].Match.Player1.Name, points[0].Match.Player2.Name, points[0].Match.Name);
                        //double[] result = PATFunction.GetPATResult(@"d:\temp\test.pcsp");
                        //MessageBox.Show(string.Format("{0:P2} - {1:P2}, {2} winning avg={3:P2} \n {4}", result[0], result[1], points[0].Match.Player1.Name,
                        //    (result[0] + result[1]) / 2, points[0].Match.ActualResult));
                    }
                    else if (points[0].Match.Player1.Hand == Stroke.RIGHT_HAND && points[0].Match.Player2.Hand != Stroke.RIGHT_HAND)
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
                            //continue;
                        }
                        TennisDataLib.ML.StrokeByStroke(@"d:\temp\tennis-4.csv", strokes);
                        //Dictionary<string, int> Nums = new Dictionary<string, int>();
                        //Nums = Model_RH_LH_6Regions.CountEvents(strokes);
                        ////PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                        //try
                        //{
                        //    TennisDataLib.ML.AppendCSV(@"d:\temp\tennis-1.csv", points[0].Match, Nums);
                        //}
                        //catch { }
                        ////PATFunction.ReplaceWeight(@"d:\temp\Right-Left Model 6 Regions.pcsp",
                        ////    @"d:\temp\test.pcsp", Nums, points[0].Match.Player1.Name, points[0].Match.Player2.Name, points[0].Match.Name);
                        ////double[] result = PATFunction.GetPATResult(@"d:\temp\test.pcsp");
                        ////MessageBox.Show(string.Format("{0:P2} - {1:P2}, {2} winning avg={3:P2} \n {4}", result[0], result[1], points[0].Match.Player1.Name,
                        ////    (result[0] + result[1]) / 2, points[0].Match.ActualResult));
                    }
                    else if (points[0].Match.Player1.Hand != Stroke.RIGHT_HAND && points[0].Match.Player2.Hand == Stroke.RIGHT_HAND)
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
                            //continue;
                        }
                        TennisDataLib.ML.StrokeByStroke(@"d:\temp\tennis-4.csv", strokes);
                        //Dictionary<string, int> Nums = new Dictionary<string, int>();
                        //Nums = Model_LH_RH_6Regions.CountEvents(strokes);
                        ////PATFunction.SaveWeight(m.Key.Replace(".html", ".RH-LH-6.data"), Nums);
                        //try
                        //{
                        //    TennisDataLib.ML.AppendCSV(@"d:\temp\tennis-1.csv", points[0].Match, Nums);
                        //}
                        //catch { }
                        ////PATFunction.ReplaceWeight(@"d:\temp\Right-Left Model 6 Regions.pcsp",
                        ////@"d:\temp\test.pcsp", Nums, points[0].Match.Player2.Name, points[0].Match.Player1.Name, points[0].Match.Name);
                        ////double[] result = PATFunction.GetPATResult(@"d:\temp\test.pcsp");
                        ////MessageBox.Show(string.Format("{0:P2} - {1:P2}, {2} winning avg={3:P2} \n {4}", result[0], result[1], points[0].Match.Player2.Name,
                        ////    (result[0] + result[1]) / 2, points[0].Match.ActualResult));
                    }
                    else if (points[0].Match.Player1.Hand != Stroke.RIGHT_HAND && points[0].Match.Player2.Hand != Stroke.RIGHT_HAND)
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
                            //continue;
                        }
                        TennisDataLib.ML.StrokeByStroke(@"d:\temp\tennis-4.csv", strokes);
                        //Dictionary<string, int> Nums = new Dictionary<string, int>();
                        //Nums = Model_LH_LH_6Regions.CountEvents(strokes);
                        ////PATFunction.SaveWeight(m.Key.Replace(".html", ".LH-LH-6.data"), Nums);
                        //try
                        //{
                        //    TennisDataLib.ML.AppendCSV(@"d:\temp\tennis-1.csv", points[0].Match, Nums);
                        //}
                        //catch { }
                        ////PATFunction.ReplaceWeight(@"d:\temp\Left-Left Model 6 Regions.pcsp",
                        ////    @"d:\temp\test.pcsp", Nums, points[0].Match.Player1.Name, points[0].Match.Player2.Name, points[0].Match.Name);
                        ////double[] result = PATFunction.GetPATResult(@"d:\temp\test.pcsp");
                        ////MessageBox.Show(string.Format("{0:P2} - {1:P2}, {2} winning avg={3:P2} \n {4}", result[0], result[1], points[0].Match.Player1.Name,
                        ////    (result[0] + result[1]) / 2, points[0].Match.ActualResult));
                    }
                }
            //List<Stroke> subset = strokes.FindAll(s =>
            //    s.Player == 1
            //    //&& 
            //    //(
            //    //s.Type == Stroke.TYPE_1ST_SERVE
            //    //||
            //    //s.Type == Stroke.TYPE_2ND_SERVE
            //    //)
            //    //&& s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_T
            //    //&& s.Fault == Stroke.NO_FAULT
            //    //&& s.Next != null
            //    && s.Outcome != Stroke.FORCED_ERROR
            //    //&& s.Outcome != Stroke.FAULT
            //    //&& Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
            //    && s.Direction == Stroke.DIR_CROSSCOURT
            //    && s.Shot == 22
            //    && 
            //    ( s.Type == Stroke.TYPE_RALLY)
            //    //&& s.HitCourt == Stroke.DE_CT
            //    //&& s.Direction == Stroke.DIR_BODY
            //    );
            //List<Stroke> subset = strokes.FindAll(s =>
            //    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
            //    && !s.Description.Trim().ToLower().Equals("unknown")
            //    && !s.Description.Trim().ToLower().Contains("penalty"))
            //);
            //if (subset.Count > 0)
            //{
            //    //MessageBox.Show(subset.Count.ToString());
            //    //listBox1.Items.Clear();
            //    //foreach (var x in subset)
            //    //{
            //    //    listBox1.Items.Add(string.Format("{0}-{1} {2}-{3} {4}-{5} {6}",
            //    //        x.Point.Set1, x.Point.Set2, x.Point.Game1, x.Point.Game2, x.Point.Pts1, x.Point.Pts2, x.Description));
            //    //}
            //    listBox1.Items.Add(m);
            //}
            //List<Stroke> subset = strokes.FindAll(s =>
            //    s.Player == 1
            //    && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
            //    && (s.Next == null)
            //    //&& s.Direction == Stroke.DIR_CROSSCOURT
            //    //&& s.Point.ServiceCourt == Stroke.AD_CT
            //    //&& Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
            //    //&& s.Fault == Stroke.NO_FAULT
            //    );


            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var x = WebFunction.GetMatchList();
            var stars = new string[]{ "Roger Federer", "Rafael Nadal", "Novak Djokovic", "Andy Murray" };
            int count = 0;
            int count2 = 0;
            foreach (var m in x)
            {
                if ((m.Value.CompareTo("2010") > 0) && (m.Value[9]=='M')) {
                    count2++;
                    foreach (string s in stars)
                    {
                        if (m.Key.Contains(s))
                        {
                            Console.WriteLine(m.Key);
                            count++;
                            break;
                        }
                    }
                }
            }
            Console.WriteLine(count);
            Console.WriteLine(count2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Button3 clicked");

            // Keep the program running
            while (true)
            {
                // Execute the function
                ExecuteFunction();

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
            List<string> all_players = new List<string>{};
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
                if (!all_players.Contains(ply1Name)){
                    all_players.Add(ply1Name);
                }
                if (!all_players.Contains(ply2Name)){
                    all_players.Add(ply2Name);
                }
                
            }
            Console.WriteLine(all_players.Count);
            int count2=0;
            foreach(var x in all_players){
                var y = WebFunction.GetPlayerInfo(x);
                if (y.Hand == 99){
                    if (!WebFunction.UpdatePlayerInfo(x)){
                        Console.WriteLine("Failed "+x);
                        count++;
                    }
                }
                else{
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
                    Console.WriteLine(points[0].Match.Player1.Name+" vs " +points[0].Match.Player2.Name);
                    Console.WriteLine(result[0].ToString() + ", " + result[1].ToString());
                    Console.WriteLine(points[0].Match.ActualResult);

                    double difference = Math.Abs(result[0] - result[1]);
                    if (difference >= 0.15)
                    {
                        try
                        {
                            File.Delete(CSVName);
                            Console.WriteLine("File deleted successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred: " + ex.Message);
                        }
                    }

                }


            }


            
        }
      



 /*       
        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Button3 clicked");
            int count;
            count = WebFunction.DownloadAllPlayerInfoToLocal();
            Console.WriteLine(count);
            string[] links =
            {
                "20240212-M-Rotterdam-R32-Dino_Prizmic-David_Goffin.html"
               //right//"20240126-M-Australian_Open-SF-Jannik_Sinner-Novak_Djokovic.html"
            };
            WebFunction.DownloadMatchListToLocal(links);
            Dictionary<string, int> nums = null;
            List<TennisData.Point> points = new List<TennisData.Point>();
            points = WebFunction.GetPointLog("20240212-M-Rotterdam-R32-Dino_Prizmic-David_Goffin.html");
            //right//points = WebFunction.GetPointLog("20240126-M-Australian_Open-SF-Jannik_Sinner-Novak_Djokovic.html");
            Console.WriteLine(points.Count);
            if (points.Count > 0)
            {
                List<TennisData.Stroke> strokes = new List<Stroke>();
                foreach (TennisData.Point p in points)
                {
                    List<Stroke> temp = p.ExtractStrokes();
                    List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                    strokes.AddRange(converted);
                }
                TennisDataLib.ML.StrokeByStroke(@"20240212-M-Rotterdam-R32-Dino_Prizmic-David_Goffin.csv", strokes);
                //right//TennisDataLib.ML.StrokeByStroke(@"20240126-M-Australian_Open-SF-Jannik_Sinner-Novak_Djokovic.csv", strokes);
                Dictionary<string, int> Nums = new Dictionary<string, int>();
                Nums = Model_RH_RH_6Regions.CountEvents(strokes);
                nums = Nums;
            }
            if (nums.Count > 0)
            {
                string modelDir = System.Configuration.ConfigurationManager.AppSettings["ModelTemplateDirectory"];
                string pcspDir = System.Configuration.ConfigurationManager.AppSettings["PcspOutputDirectory"];
                string tempFile = pcspDir + "Temp-" + System.Guid.NewGuid().ToString() + ".pcsp";
                PATFunction.ReplaceWeight(modelDir + @"Right-Right Model 6 Regions.pcsp",
                    tempFile, nums, "Roger_Federer", "Dominic_Thiem");
                double[] result = { 0, 0 };
                result = PATFunction.GetPATResult(tempFile);
                Console.WriteLine(result[0].ToString() + ", " + result[1].ToString());
            }
            Console.WriteLine("button3 ended");
        }
        
    */





    }
}

