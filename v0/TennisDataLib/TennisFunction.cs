using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TennisData
{
    public class TennisFunction
    {
        public const byte DE_CT = 1;
        public const byte AD_CT = 3;
        public const byte OTHER = 99;
        public const string DASH_PATTERN = "[-]";

        static public byte ServiceCourtByPoint(string Pt1, string Pt2)
        {

            if (Pt1.Trim().Equals("AD", StringComparison.CurrentCultureIgnoreCase) || 
                Pt2.Trim().Equals("AD", StringComparison.CurrentCultureIgnoreCase))
            {
                return AD_CT;
            }

            if (Pt1.Trim().Equals(Pt2.Trim(), StringComparison.CurrentCultureIgnoreCase))
            {
                return DE_CT;
            }

            byte bPt1;
            byte bPt2;

            try
            {
                bPt1 = Byte.Parse(Pt1);
                bPt2 = Byte.Parse(Pt2);
                if ((bPt1 - bPt2) == 1 || (bPt1 - bPt2) == -1)
                {
                    return AD_CT;
                }
                if (bPt1 == 40 && bPt2 <= 30) bPt1 = 41;
                if (bPt2 == 40 && bPt1 <= 30) bPt2 = 41;
                if ((bPt1 + bPt2) % 2 == 0)
                {
                    return DE_CT;
                }
                else
                {
                    return AD_CT;
                }
            }
            catch (Exception ex)
            {
                return OTHER;
            }
        }
        static public byte PointWonByPlayer(byte curServer, string curPts1, string curPts2, byte nextServer, string nextPts1, string nextPts2)
        {
            if (curServer == 99) return 99;
            if (nextServer!=99 && nextServer != curServer)
            {
                string temp = nextPts2;
                nextPts2 = nextPts1;
                nextPts1 = temp;
                nextServer = curServer;
            }
            if (nextServer == 99)
            {
                if (curPts1.Trim().Equals("AD", StringComparison.CurrentCultureIgnoreCase)) return curServer;
                if (curPts2.Trim().Equals("AD", StringComparison.CurrentCultureIgnoreCase)) return (byte)(3 - curServer);
                byte bPt1;
                byte bPt2;
                try
                {
                    bPt1 = Byte.Parse(curPts1);
                    bPt2 = Byte.Parse(curPts2);
                    if (bPt1 > bPt2) return curServer;
                    else if (bPt2 > bPt1) return (byte)(3 - curServer);
                    else return 99;
                } catch(Exception ex)
                {
                    return 99;
                }
            }
            else
            {
                if (nextPts1.Trim().Equals("AD", StringComparison.CurrentCultureIgnoreCase)) return curServer;
                if (nextPts2.Trim().Equals("AD", StringComparison.CurrentCultureIgnoreCase)) return (byte)(3 - curServer);
                if (nextPts1.Trim().Equals("0") && nextPts2.Trim().Equals("0"))
                {
                    if (curPts1.Trim().Equals("AD", StringComparison.CurrentCultureIgnoreCase)) return curServer;
                    if (curPts2.Trim().Equals("AD", StringComparison.CurrentCultureIgnoreCase)) return (byte)(3 - curServer);
                    byte bPt1;
                    byte bPt2;
                    try
                    {
                        bPt1 = Byte.Parse(curPts1);
                        bPt2 = Byte.Parse(curPts2);
                        if (bPt1 > bPt2) return curServer;
                        else if (bPt2 > bPt1) return (byte)(3 - curServer);
                        else return 99;
                    }
                    catch (Exception ex)
                    {
                        return 99;
                    }
                }
                else
                {
                    if (curPts1.Trim().Equals("AD", StringComparison.CurrentCultureIgnoreCase)) return (byte)(3 - curServer);
                    if (curPts2.Trim().Equals("AD", StringComparison.CurrentCultureIgnoreCase)) return curServer;
                    try
                    {
                        byte bCurPt1 = Byte.Parse(curPts1);
                        byte bCurPt2 = Byte.Parse(curPts2);
                        byte bNextPt1 = Byte.Parse(nextPts1);
                        byte bNextPt2 = Byte.Parse(nextPts2);
                        if (bCurPt1 == bNextPt1 && bNextPt2 > bCurPt2) return (byte)(3 - curServer);
                        else if (bCurPt2 == bNextPt2 && bNextPt1 > bCurPt1) return curServer;
                        else return 99;
                    }
                    catch(Exception ex)
                    {
                        return 99;
                    }
                }
            }
        }
        static public string[] ParsePoints(string Points, byte Server)
        {
            string[] s = Regex.Split(Points.Trim(), DASH_PATTERN);
            string[] p = new string[2];
            if (s.Length == 2)
            {
                if (Server == 1)
                {
                    p[0] = CorrectedPoint(s[0]);
                    p[1] = CorrectedPoint(s[1]);
                }
                else
                {
                    p[0] = CorrectedPoint(s[1]);
                    p[1] = CorrectedPoint(s[0]);
                }
            }
            else
            {
                p[0] = OTHER.ToString();
                p[1] = OTHER.ToString();
            }
            return p;
        }
        static public string CorrectedPoint(string Point)
        {
            try
            {
                switch (Point.Trim().ToUpper())
                {
                    case "JAN":
                        return "1";
                    case "FEB":
                        return "2";
                    case "MAR":
                        return "3";
                    case "APR":
                        return "4";
                    case "MAY":
                        return "5";
                    case "JUN":
                        return "6";
                    case "JUL":
                        return "7";
                    case "AUG":
                        return "8";
                    case "SEP":
                        return "9";
                    case "OCT":
                        return "10";
                    case "NOV":
                        return "11";
                    case "DEC":
                        return "12";
                    default:
                        return (Point.Trim().ToUpper()+"  ").Substring(0,2);
                }
            }
            catch (Exception ex)
            {
                return OTHER.ToString();
            }
        }
    }
}
