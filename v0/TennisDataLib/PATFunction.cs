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
    public class PATFunction
    {
        public static string ReplaceWeight(string SrcPATFile, string DestPATFile, Dictionary<string, int> Nums,
            string Player1Name="Player1Name", string Player2Name="Player2Name", string MatchName = "MatchName")
        {
            string weightPattern = "";
            string PATSource = "";
            PATSource = File.ReadAllText(SrcPATFile);

            foreach (KeyValuePair<string, int> entry in Nums)
            {
                string k = entry.Key;
                string[] x = k.Split('.');
                if (x.Length != 2) continue;
                string process = x[0];
                string eventName = x[1];
                weightPattern = string.Format(@"^{0}.*?(\d+)\s*:\s*{1}", process, eventName);
                Match m = new Regex(weightPattern,
                    RegexOptions.Singleline | RegexOptions.Multiline | 
                    RegexOptions.IgnoreCase).Match(PATSource);
                if (m.Success)
                {
                    PATSource = PATSource.Remove(m.Groups[1].Index, m.Groups[1].Length)
                        .Insert(m.Groups[1].Index, Nums[k].ToString());
                }
            }
            PATSource = PATSource.Replace("***Player1Name***", Player1Name);
            PATSource = PATSource.Replace("***Player2Name***", Player2Name);
            PATSource = PATSource.Replace("***MatchName***", MatchName);
            File.WriteAllText(DestPATFile, PATSource);
            return PATSource;
        }

        public static double[] GetPATResult(string PATSourceFileName)
        {
            double[] result = new double[2];

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = ConfigurationManager.AppSettings["PatConsolePath"];
            startInfo.Arguments = "-pcsp " + PATSourceFileName + " " + PATSourceFileName + ".txt";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string PATResult = System.IO.File.ReadAllText(PATSourceFileName + ".txt");
            string ResultPattern = @"Probability\s*\[(0\.\d+),\s+(0\.\d+)";
            System.Text.RegularExpressions.Match m
                = System.Text.RegularExpressions.Regex.Match(PATResult, ResultPattern);
            if (m.Success)
            {
                result[0] = double.Parse(m.Groups[1].Value);
                result[1] = double.Parse(m.Groups[2].Value);
            }
            else
            {
                result[0] = 0.0;
                result[1] = 0.0;
            }

            return result;
        }

        public static Dictionary<string, int> Sum(Dictionary<string, int> nums1, Dictionary<string, int> nums2)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach(KeyValuePair<string, int> entry in nums1)
            {
                result.Add(entry.Key, entry.Value);
            }
            foreach (KeyValuePair<string, int> entry in nums2)
            {
                if (nums1.ContainsKey(entry.Key))
                {
                    int temp = result[entry.Key];
                    temp += entry.Value;
                    result[entry.Key] = temp;
                }
                else
                {
                    result.Add(entry.Key, entry.Value);
                }
            }
            return result;
        }

        public static void SaveWeight(string DestDataFile, Dictionary<string, int> Nums)
        {
            var query = from kvp in Nums
                        select kvp.Key + " : " + kvp.Value.ToString();
            File.WriteAllLines(ConfigurationManager.AppSettings["LocalDataDirectory"]+ DestDataFile, query);
        }

        public static Dictionary<string, int> LoadWeight(string SrcDataFile)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            string[] lines = File.ReadAllLines(ConfigurationManager.AppSettings["LocalDataDirectory"] + SrcDataFile);
            foreach(string s in lines)
            {
                string[] kvp = s.Split(':');
                result.Add(kvp[0].Trim(), int.Parse(kvp[1]));
            }
            return result;
        }

    }
}
