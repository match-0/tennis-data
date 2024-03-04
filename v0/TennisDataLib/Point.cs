using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TennisData
{
    public class Point
    {
        private static char[] STROKE_DELIMETER = ";.".ToArray();

        public byte Set1;
        public byte Set2;
        public byte Game1;
        public byte Game2;
        public string Pts1;
        public string Pts2;
        public byte Server;
        public string Strokes;  // point-to-point description, match charting
        public string S1;   // Raw point-to-point description, 1st serve, in CSV
        public string S2;   // Raw point-to-point description, 2nd serve, in CSV
        public byte ServiceCourt;
        override public string ToString()
        {
            return string.Format("{0} {1}-{2} {3}-{4} {5}-{6} {7}", Server, Set1, Set2, Game1, Game2, Pts1, Pts2, Strokes);
        }
        public MatchInfo Match;
        public Point PrevPt;
        public Point NextPt;
        public List<Stroke> ExtractStrokes()
        {
            List<Stroke> ls = new List<Stroke>();
            string[] st = Strokes.Split(STROKE_DELIMETER);
            byte Player = Server;
            Stroke s = null; 
            ServiceCourt = TennisFunction.ServiceCourtByPoint(Pts1, Pts2);
            Stroke Prev = null;
            foreach (string str in st)
            {
                if (str.Trim().Equals("")) continue;
                if (Regex.IsMatch(str, "\\(\\d+-shot rally\\)")) continue;
                if (Regex.IsMatch(str, "1st serve") || Regex.IsMatch(str, "2nd serve"))
                {
                    Player = Server;
                }
                if (Player == 1)
                {
                    s = new Stroke(str, Match.Player1.Hand, Prev, ServiceCourt);
                }
                else
                {
                    s = new Stroke(str, Match.Player2.Hand, Prev, ServiceCourt);
                }
                s.Player = Player;
                Player = (byte)(3 - Player); //alternating 1, 2, 1, 2 ...
                s.Point = this;
                Prev = s;
                ls.Add(s);
            }
            return ls;
        }
        public List<Stroke> ExtractStrokes2()
        {
            string pattern = "(?=[fbrsvzopuylmhijktq])";
            List<Stroke> ls = new List<Stroke>();
            Stroke s = null;
            byte PrevDirection = 99;
            if (S2.Equals(""))
            {
                string[] strokes = Regex.Split(S1, pattern);
                if (strokes.Length >= 1) 
                {
                    if (Server == 1)
                    {
                        s = new Stroke(1, strokes[0], Match.Player1.Hand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                    }
                    else
                    {
                        s = new Stroke(1, strokes[0], Match.Player2.Hand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                    }
                    s.Player = Server;
                    PrevDirection = s.Direction;
                    ls.Add(s);
                }
                if (strokes.Length >= 2)
                {
                    if (Server == 1)
                    {
                        s = new Stroke(3, strokes[1], Match.Player2.Hand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                        s.Player = 2;
                    }
                    else
                    {
                        s = new Stroke(3, strokes[1], Match.Player1.Hand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                        s.Player = 1;
                    }
                    PrevDirection = s.Direction;
                    ls.Add(s);
                }
                for (int i = 2; i < strokes.Length; i++)
                {
                    byte PlayerHand = 0;
                    if ((Server + i) % 2 == 1)
                    {
                        PlayerHand = Match.Player1.Hand;
                    }
                    else
                    {
                        PlayerHand = Match.Player2.Hand;
                    }
                    s = new Stroke(4, strokes[i], PlayerHand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                    if ((Server + i) % 2 == 1)
                    {
                        s.Player = 1;
                    }
                    else
                    {
                        s.Player = 2;
                    }
                    PrevDirection = s.Direction;
                    ls.Add(s);
                }
            }
            else
            {
                string[] strokes = Regex.Split(S1, pattern);
                if (strokes.Length >= 1)
                {
                    if (Server == 1)
                    {
                        s = new Stroke(1, strokes[0], Match.Player1.Hand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                    }
                    else
                    {
                        s = new Stroke(1, strokes[0], Match.Player2.Hand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                    }
                    s.Player = Server;
                    ls.Add(s);
                }
                strokes = Regex.Split(S2, pattern);
                if (strokes.Length >= 1)
                {
                    if (Server == 1)
                    {
                        s = new Stroke(2, strokes[0], Match.Player1.Hand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                    }
                    else
                    {
                        s = new Stroke(2, strokes[0], Match.Player2.Hand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                    }
                    s.Player = Server;
                    PrevDirection = s.Direction;
                    ls.Add(s);
                }
                if (strokes.Length >= 2)
                {
                    if (Server == 1)
                    {
                        s = new Stroke(3, strokes[1], Match.Player2.Hand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                        s.Player = 2;
                    }
                    else
                    {
                        s = new Stroke(3, strokes[1], Match.Player1.Hand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                        s.Player = 1;
                    }
                    PrevDirection = s.Direction;
                    ls.Add(s);
                }
                for (int i = 2; i < strokes.Length; i++)
                {
                    byte PlayerHand = 0;
                    if ((Server + i) % 2 == 1)
                    {
                        PlayerHand = Match.Player1.Hand;
                    }
                    else
                    {
                        PlayerHand = Match.Player2.Hand;
                    }
                    s = new Stroke(4, strokes[i], PlayerHand, PrevDirection, TennisFunction.ServiceCourtByPoint(Pts1, Pts2));
                    if ((Server + i) % 2 == 1)
                    {
                        s.Player = 1;
                    }
                    else
                    {
                        s.Player = 2;
                    }
                    PrevDirection = s.Direction;
                    ls.Add(s);
                }
            }

            return ls;
        }
    }
}
