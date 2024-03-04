using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TennisData;

namespace TennisDataLib
{
    public class ML
    {
        public static void AppendCSV(string CSVFileName, MatchInfo match, Dictionary<string, int> nums)
        {
            if (!File.Exists(CSVFileName))
            {
                using (StreamWriter sw = File.CreateText(CSVFileName))
                {
                    
                }
            }
            using (StreamWriter sw = File.AppendText(CSVFileName))
            {
                Player ply1 = null;
                Player ply2 = null;
                int ply1points = 0;
                int ply2points = 0;
                if (match.Player1.Hand == Stroke.LEFT_HAND && match.Player2.Hand == Stroke.RIGHT_HAND)
                {
                    ply1 = match.Player2;
                    ply2 = match.Player1;
                    ply1points = match.Player2ActualPoints;
                    ply2points = match.Player1ActualPoints;
                }
                else
                {
                    ply1 = match.Player1;
                    ply2 = match.Player2;
                    ply1points = match.Player1ActualPoints;
                    ply2points = match.Player2ActualPoints;
                }
                DateTime matchDate = DateTime.ParseExact(new Regex(@"\d{8}").Match(match.MatchURL).Value,
                        "yyyyMMdd", CultureInfo.InvariantCulture);
                string line = "";
                line += '"' + ply1.Name.Replace(",", "") + '"' +',';
                line += '"' + ply2.Name.Replace(",", "") + '"' + ',';
                line += ply1.Hand == Stroke.RIGHT_HAND ? "RH" : "LH"; line += ',';
                line += ply2.Hand == Stroke.RIGHT_HAND ? "RH" : "LH"; line += ',';
                Int16 rank = DatabaseFunction.GetPlayerRankingOnDate(ply1.Name, matchDate);
                line += (rank != -1) ? rank.ToString() : ""; line += ',';
                rank = DatabaseFunction.GetPlayerRankingOnDate(ply2.Name, matchDate);
                line += (rank != -1) ? rank.ToString() : ""; line += ',';
                line += ply1points == 0 ? "0" : (ply1points * 1.0 / (ply1points + ply2points)).ToString(); line += ',';
                line += ply2points == 0 ? "0" : (ply2points * 1.0 / (ply1points + ply2points)).ToString(); line += ',';
                line += (matchDate - DateTime.Parse("1 Jan 1900")).TotalDays; line += ',';
                line += new Regex(@"\d{8}-[WM]-([^-]*)").Match(match.MatchURL).Groups[1].Value; line += ',';
                int sum = 0;

                sum = nums["De_Ply1Serve.ServeT_in"] + nums["De_Ply1Serve.ServeWide_in"] + nums["De_Ply1Serve.ServeBody_in"]
                    + nums["De_Ply1Serve.Serve_err"];
                line += sum == 0 ? "0" : (nums["De_Ply1Serve.ServeT_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply1Serve.ServeWide_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply1Serve.ServeBody_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply1Serve.Serve_err"] * 1.0 / sum).ToString(); line += ',';

                sum = nums["De_Ply1Serve_2nd.ServeT_in"] + nums["De_Ply1Serve_2nd.ServeWide_in"] + nums["De_Ply1Serve_2nd.ServeBody_in"]
                    + nums["De_Ply1Serve_2nd.Serve_err"];
                line += sum == 0 ? "0" : (nums["De_Ply1Serve_2nd.ServeT_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply1Serve_2nd.ServeWide_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply1Serve_2nd.ServeBody_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply1Serve_2nd.Serve_err"] * 1.0 / sum).ToString(); line += ',';

                sum = nums["Ad_Ply1Serve.ServeT_in"] + nums["Ad_Ply1Serve.ServeWide_in"] + nums["Ad_Ply1Serve.ServeBody_in"]
                    + nums["Ad_Ply1Serve.Serve_err"];
                line += sum == 0 ? "0" : (nums["Ad_Ply1Serve.ServeT_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply1Serve.ServeWide_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply1Serve.ServeBody_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply1Serve.Serve_err"] * 1.0 / sum).ToString(); line += ',';

                sum = nums["Ad_Ply1Serve_2nd.ServeT_in"] + nums["Ad_Ply1Serve_2nd.ServeWide_in"] + nums["Ad_Ply1Serve_2nd.ServeBody_in"]
                    + nums["Ad_Ply1Serve_2nd.Serve_err"];
                line += sum == 0 ? "0" : (nums["Ad_Ply1Serve_2nd.ServeT_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply1Serve_2nd.ServeWide_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply1Serve_2nd.ServeBody_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply1Serve_2nd.Serve_err"] * 1.0 / sum).ToString(); line += ',';

                sum = nums["De_Ply2Serve.ServeT_in"] + nums["De_Ply2Serve.ServeWide_in"] + nums["De_Ply2Serve.ServeBody_in"]
                    + nums["De_Ply2Serve.Serve_err"];
                line += sum == 0 ? "0" : (nums["De_Ply2Serve.ServeT_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply2Serve.ServeWide_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply2Serve.ServeBody_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply2Serve.Serve_err"] * 1.0 / sum).ToString(); line += ',';

                sum = nums["De_Ply2Serve_2nd.ServeT_in"] + nums["De_Ply2Serve_2nd.ServeWide_in"] + nums["De_Ply2Serve_2nd.ServeBody_in"]
                    + nums["De_Ply2Serve_2nd.Serve_err"];
                line += sum == 0 ? "0" : (nums["De_Ply2Serve_2nd.ServeT_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply2Serve_2nd.ServeWide_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply2Serve_2nd.ServeBody_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["De_Ply2Serve_2nd.Serve_err"] * 1.0 / sum).ToString(); line += ',';

                sum = nums["Ad_Ply2Serve.ServeT_in"] + nums["Ad_Ply2Serve.ServeWide_in"] + nums["Ad_Ply2Serve.ServeBody_in"]
                    + nums["Ad_Ply2Serve.Serve_err"];
                line += sum == 0 ? "0" : (nums["Ad_Ply2Serve.ServeT_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply2Serve.ServeWide_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply2Serve.ServeBody_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply2Serve.Serve_err"] * 1.0 / sum).ToString(); line += ',';

                sum = nums["Ad_Ply2Serve_2nd.ServeT_in"] + nums["Ad_Ply2Serve_2nd.ServeWide_in"] + nums["Ad_Ply2Serve_2nd.ServeBody_in"]
                    + nums["Ad_Ply2Serve_2nd.Serve_err"];
                line += sum == 0 ? "0" : (nums["Ad_Ply2Serve_2nd.ServeT_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply2Serve_2nd.ServeWide_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply2Serve_2nd.ServeBody_in"] * 1.0 / sum).ToString(); line += ',';
                line += sum == 0 ? "0" : (nums["Ad_Ply2Serve_2nd.Serve_err"] * 1.0 / sum).ToString(); line += ',';

                if (ply1.Hand == Stroke.RIGHT_HAND)
                {
                    sum = nums["Ply1_ForeHandR.FH_CrossCourt_DE"] + nums["Ply1_ForeHandR.FH_DownLine"] + nums["Ply1_ForeHandR.FH_DownMid_DE"]
                    + nums["Ply1_ForeHandR.FH_Error_DE"];
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_CrossCourt_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_DownLine"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_DownMid_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_Error_DE"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_ForeHandR.FH_InsideOut"] + nums["Ply1_ForeHandR.FH_CrossCourt_AD"] + nums["Ply1_ForeHandR.FH_InsideIn"]
                    + nums["Ply1_ForeHandR.FH_DownMid_AD"] + nums["Ply1_ForeHandR.FH_Error_AD"];
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_CrossCourt_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_DownMid_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_Error_AD"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_BackHandR.BH_CrossCourt_AD"] + nums["Ply1_BackHandR.BH_DownLine"] + nums["Ply1_BackHandR.BH_DownMid_AD"]
                    + nums["Ply1_BackHandR.BH_Error_AD"];
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_CrossCourt_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_DownLine"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_DownMid_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_Error_AD"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_BackHandR.BH_InsideOut"] + nums["Ply1_BackHandR.BH_CrossCourt_DE"] + nums["Ply1_BackHandR.BH_InsideIn"]
                    + nums["Ply1_BackHandR.BH_DownMid_DE"] + nums["Ply1_BackHandR.BH_Error_DE"];
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_CrossCourt_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_DownMid_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_Error_DE"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_de_stroke.FH_Crosscourt"] + nums["Ply1_de_stroke.FH_Downline"] + nums["Ply1_de_stroke.FH_DownMid"]
                    + nums["Ply1_de_stroke.BH_InsideIn"] + nums["Ply1_de_stroke.BH_InsideOut"] + nums["Ply1_de_stroke.BH_DownMid"] 
                    + nums["Ply1_de_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.FH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.FH_Downline"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.BH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.Error"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_mid_stroke.FH_InsideOut"] + nums["Ply1_mid_stroke.FH_Crosscourt"] + nums["Ply1_mid_stroke.FH_DownMid"]
                    + nums["Ply1_mid_stroke.BH_InsideOut"] + nums["Ply1_mid_stroke.BH_DownMid"] + nums["Ply1_mid_stroke.BH_Crosscourt"]
                    + nums["Ply1_mid_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.FH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.BH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.Error"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_ad_stroke.BH_Crosscourt"] + nums["Ply1_ad_stroke.BH_Downline"] + nums["Ply1_ad_stroke.BH_DownMid"]
                    + nums["Ply1_ad_stroke.FH_InsideIn"] + nums["Ply1_ad_stroke.FH_InsideOut"] + nums["Ply1_ad_stroke.FH_DownMid"]
                    + nums["Ply1_ad_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.BH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.BH_Downline"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.FH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.Error"] * 1.0 / sum).ToString(); line += ',';
                }
                else
                {
                    sum = nums["Ply1_ForeHandR.FH_CrossCourt_AD"] + nums["Ply1_ForeHandR.FH_DownLine"] + nums["Ply1_ForeHandR.FH_DownMid_AD"]
                                        + nums["Ply1_ForeHandR.FH_Error_AD"];
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_CrossCourt_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_DownLine"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_DownMid_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_Error_AD"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_ForeHandR.FH_InsideOut"] + nums["Ply1_ForeHandR.FH_CrossCourt_DE"] + nums["Ply1_ForeHandR.FH_InsideIn"]
                    + nums["Ply1_ForeHandR.FH_DownMid_DE"] + nums["Ply1_ForeHandR.FH_Error_DE"];
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_CrossCourt_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_DownMid_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ForeHandR.FH_Error_DE"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_BackHandR.BH_CrossCourt_DE"] + nums["Ply1_BackHandR.BH_DownLine"] + nums["Ply1_BackHandR.BH_DownMid_DE"]
                    + nums["Ply1_BackHandR.BH_Error_DE"];
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_CrossCourt_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_DownLine"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_DownMid_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_Error_DE"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_BackHandR.BH_InsideOut"] + nums["Ply1_BackHandR.BH_CrossCourt_AD"] + nums["Ply1_BackHandR.BH_InsideIn"]
                    + nums["Ply1_BackHandR.BH_DownMid_AD"] + nums["Ply1_BackHandR.BH_Error_AD"];
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_CrossCourt_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_DownMid_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_BackHandR.BH_Error_AD"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_ad_stroke.FH_Crosscourt"] + nums["Ply1_ad_stroke.FH_Downline"] + nums["Ply1_ad_stroke.FH_DownMid"]
                    + nums["Ply1_ad_stroke.BH_InsideIn"] + nums["Ply1_ad_stroke.BH_InsideOut"] + nums["Ply1_ad_stroke.BH_DownMid"]
                    + nums["Ply1_ad_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.FH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.FH_Downline"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.BH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_ad_stroke.Error"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_mid_stroke.FH_InsideOut"] + nums["Ply1_mid_stroke.FH_Crosscourt"] + nums["Ply1_mid_stroke.FH_DownMid"]
                    + nums["Ply1_mid_stroke.BH_InsideOut"] + nums["Ply1_mid_stroke.BH_DownMid"] + nums["Ply1_mid_stroke.BH_Crosscourt"]
                    + nums["Ply1_mid_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.FH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.BH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_mid_stroke.Error"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply1_de_stroke.BH_Crosscourt"] + nums["Ply1_de_stroke.BH_Downline"] + nums["Ply1_de_stroke.BH_DownMid"]
                   + nums["Ply1_de_stroke.FH_InsideIn"] + nums["Ply1_de_stroke.FH_InsideOut"] + nums["Ply1_de_stroke.FH_DownMid"]
                   + nums["Ply1_de_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.BH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.BH_Downline"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.FH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply1_de_stroke.Error"] * 1.0 / sum).ToString(); line += ',';
                }

                if (ply2.Hand == Stroke.RIGHT_HAND)
                {
                    sum = nums["Ply2_ForeHandR.FH_CrossCourt_DE"] + nums["Ply2_ForeHandR.FH_DownLine"] + nums["Ply2_ForeHandR.FH_DownMid_DE"]
                    + nums["Ply2_ForeHandR.FH_Error_DE"];
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_CrossCourt_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_DownLine"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_DownMid_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_Error_DE"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_ForeHandR.FH_InsideOut"] + nums["Ply2_ForeHandR.FH_CrossCourt_AD"] + nums["Ply2_ForeHandR.FH_InsideIn"]
                    + nums["Ply2_ForeHandR.FH_DownMid_AD"] + nums["Ply2_ForeHandR.FH_Error_AD"];
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_CrossCourt_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_DownMid_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_Error_AD"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_BackHandR.BH_CrossCourt_AD"] + nums["Ply2_BackHandR.BH_DownLine"] + nums["Ply2_BackHandR.BH_DownMid_AD"]
                    + nums["Ply2_BackHandR.BH_Error_AD"];
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_CrossCourt_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_DownLine"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_DownMid_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_Error_AD"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_BackHandR.BH_InsideOut"] + nums["Ply2_BackHandR.BH_CrossCourt_DE"] + nums["Ply2_BackHandR.BH_InsideIn"]
                    + nums["Ply2_BackHandR.BH_DownMid_DE"] + nums["Ply2_BackHandR.BH_Error_DE"];
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_CrossCourt_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_DownMid_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_Error_DE"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_de_stroke.FH_Crosscourt"] + nums["Ply2_de_stroke.FH_Downline"] + nums["Ply2_de_stroke.FH_DownMid"]
                    + nums["Ply2_de_stroke.BH_InsideIn"] + nums["Ply2_de_stroke.BH_InsideOut"] + nums["Ply2_de_stroke.BH_DownMid"]
                    + nums["Ply2_de_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.FH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.FH_Downline"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.BH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.Error"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_mid_stroke.FH_InsideOut"] + nums["Ply2_mid_stroke.FH_Crosscourt"] + nums["Ply2_mid_stroke.FH_DownMid"]
                    + nums["Ply2_mid_stroke.BH_InsideOut"] + nums["Ply2_mid_stroke.BH_DownMid"] + nums["Ply2_mid_stroke.BH_Crosscourt"]
                    + nums["Ply2_mid_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.FH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.BH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.Error"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_ad_stroke.BH_Crosscourt"] + nums["Ply2_ad_stroke.BH_Downline"] + nums["Ply2_ad_stroke.BH_DownMid"]
                    + nums["Ply2_ad_stroke.FH_InsideIn"] + nums["Ply2_ad_stroke.FH_InsideOut"] + nums["Ply2_ad_stroke.FH_DownMid"]
                    + nums["Ply2_ad_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.BH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.BH_Downline"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.FH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.Error"] * 1.0 / sum).ToString(); line += ',';
                }
                else
                {
                    sum = nums["Ply2_ForeHandR.FH_CrossCourt_AD"] + nums["Ply2_ForeHandR.FH_DownLine"] + nums["Ply2_ForeHandR.FH_DownMid_AD"]
                                        + nums["Ply2_ForeHandR.FH_Error_AD"];
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_CrossCourt_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_DownLine"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_DownMid_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_Error_AD"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_ForeHandR.FH_InsideOut"] + nums["Ply2_ForeHandR.FH_CrossCourt_DE"] + nums["Ply2_ForeHandR.FH_InsideIn"]
                    + nums["Ply2_ForeHandR.FH_DownMid_DE"] + nums["Ply2_ForeHandR.FH_Error_DE"];
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_CrossCourt_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_DownMid_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ForeHandR.FH_Error_DE"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_BackHandR.BH_CrossCourt_DE"] + nums["Ply2_BackHandR.BH_DownLine"] + nums["Ply2_BackHandR.BH_DownMid_DE"]
                    + nums["Ply2_BackHandR.BH_Error_DE"];
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_CrossCourt_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_DownLine"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_DownMid_DE"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_Error_DE"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_BackHandR.BH_InsideOut"] + nums["Ply2_BackHandR.BH_CrossCourt_AD"] + nums["Ply2_BackHandR.BH_InsideIn"]
                    + nums["Ply2_BackHandR.BH_DownMid_AD"] + nums["Ply2_BackHandR.BH_Error_AD"];
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_CrossCourt_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_DownMid_AD"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_BackHandR.BH_Error_AD"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_ad_stroke.FH_Crosscourt"] + nums["Ply2_ad_stroke.FH_Downline"] + nums["Ply2_ad_stroke.FH_DownMid"]
                    + nums["Ply2_ad_stroke.BH_InsideIn"] + nums["Ply2_ad_stroke.BH_InsideOut"] + nums["Ply2_ad_stroke.BH_DownMid"]
                    + nums["Ply2_ad_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.FH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.FH_Downline"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.BH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_ad_stroke.Error"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_mid_stroke.FH_InsideOut"] + nums["Ply2_mid_stroke.FH_Crosscourt"] + nums["Ply2_mid_stroke.FH_DownMid"]
                    + nums["Ply2_mid_stroke.BH_InsideOut"] + nums["Ply2_mid_stroke.BH_DownMid"] + nums["Ply2_mid_stroke.BH_Crosscourt"]
                    + nums["Ply2_mid_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.FH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.BH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.BH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_mid_stroke.Error"] * 1.0 / sum).ToString(); line += ',';

                    sum = nums["Ply2_de_stroke.BH_Crosscourt"] + nums["Ply2_de_stroke.BH_Downline"] + nums["Ply2_de_stroke.BH_DownMid"]
                   + nums["Ply2_de_stroke.FH_InsideIn"] + nums["Ply2_de_stroke.FH_InsideOut"] + nums["Ply2_de_stroke.FH_DownMid"]
                   + nums["Ply2_de_stroke.Error"];
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.BH_Crosscourt"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.BH_Downline"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.BH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.FH_InsideIn"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.FH_InsideOut"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.FH_DownMid"] * 1.0 / sum).ToString(); line += ',';
                    line += sum == 0 ? "0" : (nums["Ply2_de_stroke.Error"] * 1.0 / sum).ToString(); line += ',';
                }
                line += match.ActualResult; line += ',';
                line += match.MatchURL; line += ',';
                sw.WriteLine(line);
            }
        }

        public static void PointByPoint(string CSVFileName, MatchInfo match, List<TennisData.Point> points)
        {
            if (!File.Exists(CSVFileName))
            {
                using (StreamWriter sw = File.CreateText(CSVFileName))
                {

                }
            }
            using (StreamWriter sw = File.AppendText(CSVFileName))
            {
                Player ply1 = null;
                Player ply2 = null;
                int ply1points = 0;
                int ply2points = 0;
                if (match.Player1.Hand == Stroke.LEFT_HAND && match.Player2.Hand == Stroke.RIGHT_HAND)
                {
                    ply1 = match.Player2;
                    ply2 = match.Player1;
                    ply1points = match.Player2ActualPoints;
                    ply2points = match.Player1ActualPoints;
                }
                else
                {
                    ply1 = match.Player1;
                    ply2 = match.Player2;
                    ply1points = match.Player1ActualPoints;
                    ply2points = match.Player2ActualPoints;
                }
                DateTime matchDate = DateTime.ParseExact(new Regex(@"\d{8}").Match(match.MatchURL).Value,
                        "yyyyMMdd", CultureInfo.InvariantCulture);
                string line = "";
                line += '"' + ply1.Name.Replace(",", "") + '"' + ',';
                line += '"' + ply2.Name.Replace(",", "") + '"' + ',';
                line += ply1.Hand == Stroke.RIGHT_HAND ? "RH" : "LH"; line += ',';
                line += ply2.Hand == Stroke.RIGHT_HAND ? "RH" : "LH"; line += ',';
                Int16 rank = DatabaseFunction.GetPlayerRankingOnDate(ply1.Name, matchDate);
                line += (rank != -1) ? rank.ToString() : ""; line += ',';
                rank = DatabaseFunction.GetPlayerRankingOnDate(ply2.Name, matchDate);
                line += (rank != -1) ? rank.ToString() : ""; line += ',';
                line += ply1points == 0 ? "0" : (ply1points * 1.0 / (ply1points + ply2points)).ToString(); line += ',';
                line += ply2points == 0 ? "0" : (ply2points * 1.0 / (ply1points + ply2points)).ToString(); line += ',';
                line += (matchDate - DateTime.Parse("1 Jan 1900")).TotalDays; line += ',';
                line += new Regex(@"\d{8}-[WM]-([^-]*)").Match(match.MatchURL).Groups[1].Value; line += ',';

                string prefix = String.Copy(line);
                if (points[0].Match.Player1.Hand == Stroke.RIGHT_HAND && points[0].Match.Player2.Hand == Stroke.RIGHT_HAND)
                {
                    List<TennisData.Stroke> strokes = new List<Stroke>();
                    foreach (TennisData.Point p in points)
                    {
                        line = String.Copy(prefix);
                        if (p.Server == 1)
                        {
                            line += p.Set1.ToString(); line += ',';
                            line += p.Set2.ToString(); line += ',';
                            line += p.Game1.ToString(); line += ',';
                            line += p.Game2.ToString(); line += ',';
                            line += p.Pts1.ToString(); line += ',';
                            line += p.Pts2.ToString(); line += ',';
                        }
                        else
                        {
                            line += p.Set2.ToString(); line += ',';
                            line += p.Set1.ToString(); line += ',';
                            line += p.Game2.ToString(); line += ',';
                            line += p.Game1.ToString(); line += ',';
                            line += p.Pts2.ToString(); line += ',';
                            line += p.Pts1.ToString(); line += ',';
                        }
                        line += p.Server.ToString(); line += ',';
                        List<Stroke> temp = p.ExtractStrokes();
                        List<Stroke> converted = Model_RH_RH_6Regions.Convert(temp);
                        foreach(TennisData.Stroke s in converted)
                        {
                            if (s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_T)
                            {
                                line += "De_Ply1Serve.ServeT_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_WIDE)
                            {
                                line += "De_Ply1Serve.ServeWide_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_BODY)
                            {
                                line += "De_Ply1Serve.ServeBody_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "De_Ply1Serve.Serve_err"; line += ',';
                            }

                            if (s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_T)
                            {
                                line += "De_Ply1Serve_2nd.ServeT_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_WIDE)
                            {
                                line += "De_Ply1Serve_2nd.ServeWide_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_BODY)
                            {
                                line += "De_Ply1Serve_2nd.ServeBody_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "De_Ply1Serve_2nd.Serve_err"; line += ',';
                            }
                            
                            if (s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_T)
                            {
                                line += "Ad_Ply1Serve.ServeT_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_WIDE)
                            {
                                line += "Ad_Ply1Serve.ServeWide_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_BODY)
                            {
                                line += "Ad_Ply1Serve.ServeBody_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ad_Ply1Serve.Serve_err"; line += ',';
                            }

                            if (s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_T)
                            {
                                line +="Ad_Ply1Serve_2nd.ServeT_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_WIDE)
                            {
                                line +="Ad_Ply1Serve_2nd.ServeWide_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_BODY)
                            {
                                line +="Ad_Ply1Serve_2nd.ServeBody_in"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT)
                            {
                                line +="Ad_Ply1Serve_2nd.Serve_err"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_T)
                            {
                                line += "De_Ply2Serve.ServeT_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_WIDE)
                            {
                                line += "De_Ply2Serve.ServeWide_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_BODY)
                            {
                                line += "De_Ply2Serve.ServeBody_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "De_Ply2Serve.Serve_err"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_T)
                            {
                                line += "De_Ply2Serve_2nd.ServeT_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_WIDE)
                            {
                                line += "De_Ply2Serve_2nd.ServeWide_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_BODY)
                            {
                                line += "De_Ply2Serve_2nd.ServeBody_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "De_Ply2Serve_2nd.Serve_err"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_T)
                            {
                                line += "Ad_Ply2Serve.ServeT_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_WIDE)
                            {
                                line += "Ad_Ply2Serve.ServeWide_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_BODY)
                            {
                                line += "Ad_Ply2Serve.ServeBody_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ad_Ply2Serve.Serve_err"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_T)
                            {
                                line += "Ad_Ply2Serve_2nd.ServeT_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_WIDE)
                            {
                                line += "Ad_Ply2Serve_2nd.ServeWide_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                                && s.Direction == Stroke.DIR_BODY)
                            {
                                line += "Ad_Ply2Serve_2nd.ServeBody_in"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ad_Ply2Serve_2nd.Serve_err"; line += ',';
                            }

                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_CROSSCOURT
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ForeHandR.FH_CrossCourt_DE"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_DOWN_LINE
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ForeHandR.FH_DownLine"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_MIDDLE
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ForeHandR.FH_DownMid_DE"; line += ',';
                            }
                            // Return error should include ace and service win
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ply1_ForeHandR.FH_Error_DE"; line += ',';
                            }
                            if (s.Player == 2
                                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && (s.Direction == Stroke.DIR_WIDE || s.Direction == Stroke.DIR_BODY)
                                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                            {
                                line += "Ply1_ForeHandR.FH_Error_DE"; line += ',';
                            }

                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_INSIDE_OUT
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ForeHandR.FH_InsideOut"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_CROSSCOURT
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ForeHandR.FH_CrossCourt_AD"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_INSIDE_IN
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ForeHandR.FH_InsideIn"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_MIDDLE
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ForeHandR.FH_DownMid_AD"; line += ',';
                            }
                            // Return error should include ace and service win
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ply1_ForeHandR.FH_Error_AD"; line += ',';
                            }
                            if (s.Player == 2
                                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && (s.Direction == Stroke.DIR_T || s.Direction == Stroke.DIR_BODY)
                                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                            {
                                line += "Ply1_ForeHandR.FH_Error_AD"; line += ',';
                            }

                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_CROSSCOURT
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_BackHandR.BH_CrossCourt_DE"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_INSIDE_IN
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_BackHandR.BH_InsideIn"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_INSIDE_OUT
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_BackHandR.BH_InsideOut"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_MIDDLE
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_BackHandR.BH_DownMid_DE"; line += ',';
                            }
                            // Return error should include ace and service win
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ply1_BackHandR.BH_Error_DE"; line += ',';
                            }
                            if (s.Player == 2
                                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && (s.Direction == Stroke.DIR_T)
                                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                            {
                                line += "Ply1_BackHandR.BH_Error_DE"; line += ',';
                            }

                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_CROSSCOURT
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_BackHandR.BH_CrossCourt_AD"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_DOWN_LINE
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_BackHandR.BH_DownLine"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_MIDDLE
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_BackHandR.BH_DownMid_AD"; line += ',';
                            }
                            // Return error should include ace and service win
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ply1_BackHandR.BH_Error_AD"; line += ',';
                            }
                            if (s.Player == 2
                                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && (s.Direction == Stroke.DIR_WIDE)
                                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                            {
                                line += "Ply1_BackHandR.BH_Error_AD"; line += ',';
                            }

                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_de_stroke.FH_Crosscourt"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_DOWN_LINE
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_de_stroke.FH_Downline"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_de_stroke.FH_DownMid"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_IN
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_de_stroke.BH_InsideIn"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_de_stroke.BH_InsideOut"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_de_stroke.BH_DownMid"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ply1_de_stroke.Error"; line += ',';
                            }

                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_mid_stroke.FH_Crosscourt"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_mid_stroke.FH_InsideOut"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_mid_stroke.FH_DownMid"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_mid_stroke.BH_Crosscourt"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_mid_stroke.BH_InsideOut"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_mid_stroke.BH_DownMid"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ply1_mid_stroke.Error"; line += ',';
                            }

                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_CROSSCOURT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ad_stroke.BH_Crosscourt"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_DOWN_LINE
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ad_stroke.BH_Downline"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ad_stroke.BH_DownMid"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_IN
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ad_stroke.FH_InsideIn"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ad_stroke.FH_InsideOut"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply1_ad_stroke.FH_DownMid"; line += ',';
                            }
                            if (s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ply1_ad_stroke.Error"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_CROSSCOURT
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_ForeHandR.FH_CrossCourt_DE"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_DOWN_LINE
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_ForeHandR.FH_DownLine"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_MIDDLE
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_ForeHandR.FH_DownMid_DE"; line += ',';
                            }
                            // Return error should include ace and service win
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ply2_ForeHandR.FH_Error_DE"; line += ',';
                            }
                            if (s.Player == 1
                                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && (s.Direction == Stroke.DIR_WIDE || s.Direction == Stroke.DIR_BODY)
                                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                            {
                                line += "Ply2_ForeHandR.FH_Error_DE"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_INSIDE_IN
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_ForeHandR.FH_InsideIn"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_CROSSCOURT
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_ForeHandR.FH_CrossCourt_AD"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_INSIDE_OUT
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_ForeHandR.FH_InsideOut"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_MIDDLE
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_ForeHandR.FH_DownMid_AD"; line += ',';
                            }
                            // Return error should include ace and service win
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ply2_ForeHandR.FH_Error_AD"; line += ',';
                            }
                            if (s.Player == 1
                                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && (s.Direction == Stroke.DIR_T || s.Direction == Stroke.DIR_BODY)
                                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                            {
                                line += "Ply2_ForeHandR.FH_Error_AD"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_INSIDE_OUT
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_BackHandR.BH_InsideOut"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_CROSSCOURT
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_BackHandR.BH_CrossCourt_DE"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_INSIDE_IN
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_BackHandR.BH_InsideIn"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_MIDDLE
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_BackHandR.BH_DownMid_DE"; line += ',';
                            }
                            // Return error should include ace and service win
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ply2_BackHandR.BH_Error_DE"; line += ',';
                            }
                            if (s.Player == 1
                                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                                && s.Point.ServiceCourt == Stroke.DE_CT
                                && (s.Direction == Stroke.DIR_T)
                                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                            {
                                line += "Ply2_BackHandR.BH_Error_DE"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_CROSSCOURT
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_BackHandR.BH_CrossCourt_AD"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_DOWN_LINE
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_BackHandR.BH_DownLine"; line += ',';
                            }
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Direction == Stroke.DIR_MIDDLE
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line += "Ply2_BackHandR.BH_DownMid_AD"; line += ',';
                            }
                            // Return error should include ace and service win
                            if (s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line += "Ply2_BackHandR.BH_Error_AD"; line += ',';
                            }
                            if (s.Player == 1
                                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                                && s.Point.ServiceCourt == Stroke.AD_CT
                                && (s.Direction == Stroke.DIR_WIDE)
                                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                            {
                                line += "Ply2_BackHandR.BH_Error_AD"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_de_stroke.FH_Crosscourt"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_DOWN_LINE
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_de_stroke.FH_Downline"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_de_stroke.FH_DownMid"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_IN
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_de_stroke.BH_InsideIn"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_de_stroke.BH_InsideOut"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_de_stroke.BH_DownMid"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.DE_CT
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line +="Ply2_de_stroke.Error"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_mid_stroke.FH_InsideOut"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_mid_stroke.FH_Crosscourt"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_mid_stroke.FH_DownMid"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_mid_stroke.BH_InsideOut"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_mid_stroke.BH_Crosscourt"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_mid_stroke.BH_DownMid"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.MIDDLE_CT
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line +="Ply2_mid_stroke.Error"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_CROSSCOURT
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_ad_stroke.BH_Crosscourt"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_DOWN_LINE
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_ad_stroke.BH_Downline"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_ad_stroke.BH_DownMid"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_IN
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_ad_stroke.FH_InsideIn"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_ad_stroke.FH_InsideOut"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_MIDDLE
                                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                                && s.Fault == Stroke.NO_FAULT)
                            {
                                line +="Ply2_ad_stroke.FH_DownMid"; line += ',';
                            }

                            if (s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                                && s.HitCourt == Stroke.AD_CT
                                && s.Fault != Stroke.NO_FAULT)
                            {
                                line +="Ply2_ad_stroke.Error"; line += ',';
                            }

                        




















                        }
                        sw.WriteLine(line);


                    }
                }
                //sw.WriteLine(line);
            }
        }

        public static void StrokeByStroke(string CSVFileName, List<TennisData.Stroke> strokes)
        {
            if (!File.Exists(CSVFileName))
            {
                using (StreamWriter sw = File.CreateText(CSVFileName))
                {

                }
            }
            using (StreamWriter sw = File.AppendText(CSVFileName))
            {
                foreach (TennisData.Stroke s in strokes)
                {
                    string ply1Name;
                    int ply1Hand;
                    string ply2Name;
                    int ply2Hand;
                    int ply1Pts;
                    int ply1Gms;
                    int ply1Sets;
                    int ply2Pts;
                    int ply2Gms;
                    int ply2Sets;
                    DateTime date;
                    string tour;

                    byte type; // 1st server, 2nd server, 3-return, 4-rally
                    byte shot;
                    // 1-forehand, 22-backhand, 3-forehand chip/slice, 24-backhand slice, 5-forehand volley, 26-backhand volley, 7-smash, 28-backhand smash
                    // 9-forehand drop shot, 30-backhand drop shot, 11-forehand lob, 32-backhand lob, 13-forehand half-volley, 34-backhand half-volley
                    // 15-forhand swinging volley, 36-backhand swinging volley, 41-trick shot, 99-unknown shot
                    byte hitPosition;    //1-at net, 2-at baseline, 99-other
                    byte hitCourt;   //1-de_ct, 2-middle_ct, 3-ad_ct, 99-other
                    byte toCourt;  //1-de_ct, 2-middle_ct, 3-ad_ct, 99-other
                    byte direction;  //1-de_ct, 2-down the middle, 3-ad_ct, 4-wide, 5-to body, 6-down the T, 99-other
                    byte depth;  //1-(shallow), 2-(deep), 3-(very deep), 99-other
                    byte netCord;    //1-(net cord), 2-not
                    byte fault;  //1-(net), 2-(wide), 3-(long), 4-(wide and long), 5-(foot fault), 6-(shank), 7-no fault, 99-other
                    byte outcome;    //1-ace, 2-fault, 3-forced error, 4-unforced error, 5-winner, 6-service winner, 7-no outcome
                    string description;
                    byte approachShot;    //1-approach shot, 2-not

                    byte type1; // 1st server, 2nd server, 3-return, 4-rally
                    byte shot1;
                    // 1-forehand, 22-backhand, 3-forehand chip/slice, 24-backhand slice, 5-forehand volley, 26-backhand volley, 7-smash, 28-backhand smash
                    // 9-forehand drop shot, 30-backhand drop shot, 11-forehand lob, 32-backhand lob, 13-forehand half-volley, 34-backhand half-volley
                    // 15-forhand swinging volley, 36-backhand swinging volley, 41-trick shot, 99-unknown shot
                    byte hitPosition1;    //1-at net, 2-at baseline, 99-other
                    byte hitCourt1;   //1-de_ct, 2-middle_ct, 3-ad_ct, 99-other
                    byte toCourt1;  //1-de_ct, 2-middle_ct, 3-ad_ct, 99-other
                    byte direction1;  //1-de_ct, 2-down the middle, 3-ad_ct, 4-wide, 5-to body, 6-down the T, 99-other
                    byte depth1;  //1-(shallow), 2-(deep), 3-(very deep), 99-other
                    byte netCord1;    //1-(net cord), 2-not
                    byte fault1;  //1-(net), 2-(wide), 3-(long), 4-(wide and long), 5-(foot fault), 6-(shank), 7-no fault, 99-other
                    byte outcome1;    //1-ace, 2-fault, 3-forced error, 4-unforced error, 5-winner, 6-service winner, 7-no outcome
                    string description1;
                    byte approachShot1;    //1-approach shot, 2-not

                    byte type2; // 1st server, 2nd server, 3-return, 4-rally
                    byte shot2;
                    // 1-forehand, 22-backhand, 3-forehand chip/slice, 24-backhand slice, 5-forehand volley, 26-backhand volley, 7-smash, 28-backhand smash
                    // 9-forehand drop shot, 30-backhand drop shot, 11-forehand lob, 32-backhand lob, 13-forehand half-volley, 34-backhand half-volley
                    // 15-forhand swinging volley, 36-backhand swinging volley, 41-trick shot, 99-unknown shot
                    byte hitPosition2;    //1-at net, 2-at baseline, 99-other
                    byte hitCourt2;   //1-de_ct, 2-middle_ct, 3-ad_ct, 99-other
                    byte toCourt2;  //1-de_ct, 2-middle_ct, 3-ad_ct, 99-other
                    byte direction2;  //1-de_ct, 2-down the middle, 3-ad_ct, 4-wide, 5-to body, 6-down the T, 99-other
                    byte depth2;  //1-(shallow), 2-(deep), 3-(very deep), 99-other
                    byte netCord2;    //1-(net cord), 2-not
                    byte fault2;  //1-(net), 2-(wide), 3-(long), 4-(wide and long), 5-(foot fault), 6-(shank), 7-no fault, 99-other
                    byte outcome2;    //1-ace, 2-fault, 3-forced error, 4-unforced error, 5-winner, 6-service winner, 7-no outcome
                    string description2;
                    byte approachShot2;    //1-approach shot, 2-not

                    string url;

                    if (s.Point == null) continue;
                    try
                    {
                        if (s.Player == 1)
                        {
                            ply1Name = s.Point.Match.Player1.Name;
                            ply1Hand = s.Point.Match.Player1.Hand;
                            ply2Name = s.Point.Match.Player2.Name;
                            ply2Hand = s.Point.Match.Player2.Hand;
                            ply1Pts = PointStrToByte(s.Point.Pts1);
                            ply1Gms = s.Point.Game1;
                            ply1Sets = s.Point.Set1;
                            ply2Pts = PointStrToByte(s.Point.Pts2);
                            ply2Gms = s.Point.Game2;
                            ply2Sets = s.Point.Set2;
                        }
                        else
                        {
                            ply1Name = s.Point.Match.Player2.Name;
                            ply1Hand = s.Point.Match.Player2.Hand;
                            ply2Name = s.Point.Match.Player1.Name;
                            ply2Hand = s.Point.Match.Player1.Hand;
                            ply1Pts = PointStrToByte(s.Point.Pts2);
                            ply1Gms = s.Point.Game2;
                            ply1Sets = s.Point.Set2;
                            ply2Pts = PointStrToByte(s.Point.Pts1);
                            ply2Gms = s.Point.Game1;
                            ply2Sets = s.Point.Set1;
                        }

                        date = DateTime.ParseExact(new Regex(@"\d{8}").Match(s.Point.Match.MatchURL).Value,
                            "yyyyMMdd", CultureInfo.InvariantCulture);
                        tour = new Regex(@"\d{8}-[WM]-([^-]*)").Match(s.Point.Match.MatchURL).Groups[1].Value;

                        type = s.Type;
                        hitCourt = s.HitCourt;
                        shot = s.Shot;
                        direction = s.Direction;
                        toCourt = s.ToCourt;
                        depth = s.Depth;
                        netCord = s.NetCord;
                        fault = s.Fault;
                        hitPosition = s.HitPosition;
                        approachShot = s.ApproachShot;
                        description = s.Description;
                        outcome = s.Outcome;
                        if (s.Prev != null)
                        {
                            type1 = s.Prev.Type;
                            hitCourt1 = s.Prev.HitCourt;
                            shot1 = s.Prev.Shot;
                            direction1 = s.Prev.Direction;
                            toCourt1 = s.Prev.ToCourt;
                            depth1 = s.Prev.Depth;
                            netCord1 = s.Prev.NetCord;
                            fault1 = s.Prev.Fault;
                            hitPosition1 = s.Prev.HitPosition;
                            approachShot1 = s.Prev.ApproachShot;
                            description1 = s.Prev.Description;
                            outcome1 = s.Prev.Outcome;
                            if (s.Prev.Prev != null)
                            {
                                type2 = s.Prev.Prev.Type;
                                hitCourt2 = s.Prev.Prev.HitCourt;
                                shot2 = s.Prev.Prev.Shot;
                                direction2 = s.Prev.Prev.Direction;
                                toCourt2 = s.Prev.Prev.ToCourt;
                                depth2 = s.Prev.Prev.Depth;
                                netCord2 = s.Prev.Prev.NetCord;
                                fault2 = s.Prev.Prev.Fault;
                                hitPosition2 = s.Prev.Prev.HitPosition;
                                approachShot2 = s.Prev.Prev.ApproachShot;
                                description2 = s.Prev.Prev.Description;
                                outcome2 = s.Prev.Prev.Outcome;
                            }
                            else
                            {
                                type2 = 99;
                                hitCourt2 = 99;
                                shot2 = 99;
                                direction2 = 99;
                                toCourt2 = 99;
                                depth2 = 99;
                                netCord2 = 99;
                                fault2 = 99;
                                hitPosition2 = 99;
                                approachShot2 = 99;
                                description2 = "";
                                outcome2 = 99;
                            }
                        }
                        else
                        {
                            type1 = 99;
                            hitCourt1 = 99;
                            shot1 = 99;
                            direction1 = 99;
                            toCourt1 = 99;
                            depth1 = 99;
                            netCord1 = 99;
                            fault1 = 99;
                            hitPosition1 = 99;
                            approachShot1 = 99;
                            description1 = "";
                            outcome1 = 99;

                            type2 = 99;
                            hitCourt2 = 99;
                            shot2 = 99;
                            direction2 = 99;
                            toCourt2 = 99;
                            depth2 = 99;
                            netCord2 = 99;
                            fault2 = 99;
                            hitPosition2 = 99;
                            approachShot2 = 99;
                            description2 = "";
                            outcome2 = 99;
                        }
                        url = s.Point.Match.MatchURL;

                        string line = "";
                        line += '"' + ply1Name.Replace(",", "") + '"' + ',';
                        line += '"' + ply2Name.Replace(",", "") + '"' + ',';
                        line += ply1Hand == Stroke.RIGHT_HAND ? "RH" : "LH"; line += ',';
                        line += ply2Hand == Stroke.RIGHT_HAND ? "RH" : "LH"; line += ',';
                        //Int16 rank = DatabaseFunction.GetPlayerRankingOnDate(ply1Name, date);
                        //line += (rank != -1) ? rank.ToString() : ""; line += ',';
                        //rank = DatabaseFunction.GetPlayerRankingOnDate(ply2Name, date);
                        //line += (rank != -1) ? rank.ToString() : ""; line += ',';
                        line += ply1Pts; line += ',';
                        line += ply2Pts; line += ',';
                        line += ply1Gms; line += ',';
                        line += ply2Gms; line += ',';
                        line += ply1Sets; line += ',';
                        line += ply2Sets; line += ',';
                        line += date.ToString("yyyy-MM-dd"); line += ',';
                        line += tour; line += ',';

                        line += type; line += ',';
                        line += hitCourt; line += ',';
                        line += shot; line += ',';
                        line += direction; line += ',';
                        line += toCourt; line += ',';
                        line += depth; line += ',';
                        line += netCord; line += ',';
                        line += hitPosition; line += ',';
                        line += approachShot; line += ',';
                        line += outcome; line += ',';
                        line += fault; line += ',';

                        line += type1; line += ',';
                        line += hitCourt1; line += ',';
                        line += shot1; line += ',';
                        line += direction1; line += ',';
                        line += toCourt1; line += ',';
                        line += depth1; line += ',';
                        line += netCord1; line += ',';
                        line += hitPosition1; line += ',';
                        line += approachShot1; line += ',';
                        line += outcome1; line += ',';
                        line += fault1; line += ',';

                        line += type2; line += ',';
                        line += hitCourt2; line += ',';
                        line += shot2; line += ',';
                        line += direction2; line += ',';
                        line += toCourt2; line += ',';
                        line += depth2; line += ',';
                        line += netCord2; line += ',';
                        line += hitPosition2; line += ',';
                        line += approachShot2; line += ',';
                        line += outcome2; line += ',';
                        line += fault2; line += ',';

                        line += url;
                        line += ','; line += s.Description;
                        sw.WriteLine(line);
                    }catch(Exception e)
                    {

                    }
                }
            }
        }

        public static byte PointStrToByte(string pts)
        {
            switch (pts.ToUpper().Trim())
            {
                case "0":
                    return 0;
                case "15":
                    return 1;
                case "30":
                    return 2;
                case "40":
                    return 3;
                case "AD":
                    return 4;
                default:
                    try
                    {
                        return Byte.Parse(pts);
                    }catch (Exception e)
                    {
                        return 99;
                    }
            }
        }
    }
}
