/*
Example of point-to-point log
Server	  Sets 	Games 	Points 	
Roger Federer  	0‑0	0‑0	0‑0	1st serve wide; backhand return crosscourt (shallow); forehand down the line; forehand down the middle, forced error.
Roger Federer  	0‑0	0‑0	15‑0	1st serve down the T, fault (long). 2nd serve to body; backhand return down the middle (shallow); forehand inside-out (wide), unforced error.

Point object represents every point. There are 2 points in the above example.
Point service court should be either DE_CT or AD_CT. It is calculated based on the score, eg. 0-0 is DE_CT service.

Stroke object reprsents every shot, including serve, return and rally. There are 4 strokes in the 0-0 point example above.
Stroke.Type should be one of {TYPE_1ST_SERVE, TYPE_2ND_SERVE, TYPE_RETURN, TYPE_RALLY}
Stroke.Shot is coded as a byte:
1-forehand, 22-backhand, 3-forehand chip/slice, 24-backhand slice, 5-forehand volley, 26-backhand volley, 7-smash, 28-backhand smash
9-forehand drop shot, 30-backhand drop shot, 11-forehand lob, 32-backhand lob, 13-forehand half-volley, 34-backhand half-volley
15-forhand swinging volley, 36-backhand swinging volley, 41-trick shot, 99-unknown shot
Stroke.HandOfShot(byte shot) returns one of {FOREHAND_SHOTS, BACKHAND_SHOTS, UNKNOWN_HAND_SHOT}. Basically, 1-20 are FOREHAND_SHOTS, 21-40 are BACKHAND_SHOTS.
Stroke.Direction should be one of {DIR_WIDE, DIR_BODY, DIR_T, DIR_CROSSCOURT, DIR_DOWN_LINE, DIR_INSIDE_OUT, DIR_INSIDE_IN, DIR_MIDDLE, 99}, 99 being unknown.
Stroke.Hand should be either RIGHT_HAND or !RIGHT_HAND

To check the integrity of the Stroke list, and to facilitate model conversion, we convert Direction to (HitCourt, ToCourt), referring the ball is from HitCourt to ToCourt.
HitCourt can be one of {DE_CT, MIDDLE_CT, AD_CT, 99}, ToCourt is similar.

Direction=DIR_MIDDLE => ToCourt=MIDDLE_CT (regardless the Shot and Hand)
(FOREHAND_SHOTS, DIR_CROSSCOURT or DIR_INSIDE_IN) => (RIGHT_HAND, ToCourt = DE_CT) or (!RIGHT_HAND, ToCourt = AD_CT)
(FOREHAND_SHOTS, DIR_DOWN_LINE or DIR_INSIDE_OUT) => (RIGHT_HAND, ToCourt = AD_CT) or (!RIGHT_HAND, ToCourt = DE_CT)
(BACKHAND_SHOTS, DIR_CROSSCOURT or DIR_INSIDE_IN) => (RIGHT_HAND, ToCourt = AD_CT) or (!RIGHT_HAND, ToCourt = DE_CT)
(BACKHAND_SHOTS, DIR_DOWN_LINE or DIR_INSIDE_OUT) => (RIGHT_HAND, ToCourt = DE_CT) or (!RIGHT_HAND, ToCourt = AD_CT)
For serves, ToCourt=Service Court, except for T serve, ie. Direction = DIR_T => ToCourt=MIDDLE_CT
In all other cased, ToCourt=99

For serves, HitCourt = Service Court
For returns, HitCourt = Service Court, except for returning T serve, ie. Prev.Direction=DIR_T => HitCourt=MIDDLE_CT
For other rallies, HitCourt = Prev.ToCourt

Here is an example showing how this (Direction, HitCourt, ToCourt) identifies errors in the point-to-point log:
Player1 Nadal is LEFT_HAND and Player2 Stanislas Wawrinka is RIGHT_HAND.
Rafael Nadal  	0‑0	0‑0	15‑0	1st serve to body; forehand chip/slice return down the line (deep); forehand approach shot down the line; backhand lob down the middle; smash inside-out, winner. (5-shot rally)
(Player1, LEFT_HAND, 1st serve, From AD to AD); 1st serve to body
(Player2, RIGHT_HAND,  return, From AD to ); forehand??? Right hand from AD side can't be down the line.
Watching this shot at https://www.youtube.com/watch?v=aJHKA3iRyxc

In case HitCourt = 99, (mostly likely because Prev.Direction is 99), we can use this stroke's Direction to infer the HitCourt and Prev.ToCourt
((RIGHT_HAND, FOREHAND_SHOTS) or (!RIGHT_HAND, BACKHAND_SHOTS), (DIR_CROSSCOURT or DIR_DOWN_LINE)) => HitCourt=DE_CT
((RIGHT_HAND, FOREHAND_SHOTS) or (!RIGHT_HAND, BACKHAND_SHOTS), (DIR_INSIDE_OUT or DIR_INSIDE_IN)) => HitCourt=AD_CT
((RIGHT_HAND, BACKHAND_SHOTS) or (!RIGHT_HAND, FOREHAND_SHOTS), (DIR_CROSSCOURT or DIR_DOWN_LINE)) => HitCourt=AD_CT
((RIGHT_HAND, BACKHAND_SHOTS) or (!RIGHT_HAND, FOREHAND_SHOTS), (DIR_INSIDE_OUT or DIR_INSIDE_IN)) => HitCourt=DE_CT

We can also convert point-to-point log to List<Stroke>, then back to strings and comparing them with the original log. Sample code:
List<TennisData.Point> points = WebFunction.GetPointLog(matchURL);
List<TennisData.Stroke> strokes = new List<Stroke>();
foreach (TennisData.Point p in points)
{
    List<Stroke> temp = p.ExtractStrokes();
    strokes.AddRange(temp);
}
List<Stroke> errors = strokes.FindAll(s =>
    (!s.Description.Trim().Replace(" ", "").ToLower().Replace("()", "").Equals(s.ToString().Replace(" ", "").ToLower())
    && !s.Description.Trim().ToLower().Equals("unknown")
    && !s.Description.Trim().ToLower().Contains("penalty"))
);

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TennisData
{

    public class NV{
        public string Name;
        public byte Value;
        public NV(string name, byte value){Name = name; Value = value;}
    };

    public class Stroke
    {
        public const byte RIGHT_HAND = 1;
        public const byte LEFT_HAND = 2;
        public const byte TYPE_1ST_SERVE = 1;
        public const byte TYPE_2ND_SERVE = 2;
        public const byte TYPE_RETURN = 3;
        public const byte TYPE_RALLY = 4;
        public const byte DIR_MIDDLE = 2;
        public const byte DIR_T = 6;
        public const byte DIR_BODY = 5;
        public const byte DIR_WIDE = 4;
        public const byte DIR_CROSSCOURT = 7;
        public const byte DIR_DOWN_LINE = 8;
        public const byte DIR_INSIDE_IN = 9;
        public const byte DIR_INSIDE_OUT = 10;
        public const byte DE_CT = 1;
        public const byte MIDDLE_CT = 2;
        public const byte AD_CT = 3;
        public const byte ACE = 1;
        public const byte FAULT = 2;
        public const byte FORCED_ERROR = 3;
        public const byte UNFORCED_ERROR = 4;
        public const byte WINNER = 5;
        public const byte SERVICE_WINNER = 6;
        public const byte NO_OUTCOME = 7;
        public const byte NO_FAULT = 7;
        public const byte FOREHAND_SHOTS = 1;
        public const byte BACKHAND_SHOTS = 2;
        public const byte UNKNOWN_HAND_SHOT = 99;
        public const byte OUTCOME_WIN = 1;
        public const byte OUTCOME_LOSE = 2;
        public const byte SHOT_FOREHAND = 1;
        public const byte SHOT_BACKHAND = 22;

        private static List<NV> Types = new List<NV>(){
            new NV("1st serve", TYPE_1ST_SERVE),    //1
            new NV("2nd serve", TYPE_2ND_SERVE),    //2
            new NV("return", TYPE_RETURN),  //3
            new NV("", TYPE_RALLY)  //4
        };

        private static List<NV> Shots = new List<NV>(){
            new NV("forehand chip/slice", 3),
            new NV("backhand slice", 24),
            new NV("forehand swinging volley", 15),
            new NV("backhand swinging volley", 36),
            new NV("forehand half-volley", 13),
            new NV("backhand half-volley", 34),
            new NV("forehand volley", 5),
            new NV("backhand volley", 26),
            new NV("forehand drop shot", 9),
            new NV("backhand drop shot", 30),
            new NV("forehand lob", 11),
            new NV("backhand lob", 32),
            new NV("backhand overhead", 28),
            new NV("smash", 7),
            new NV("forehand", 1),
            new NV("backhand", 22),
            new NV("trick shot", 41),
            new NV("", 99)
        };

        private static List<NV> HitPositions = new List<NV>(){
            new NV("at net", 1),
            new NV("at baseline", 2),
            new NV("", 99)
        };

        private static List<NV> Depths = new List<NV>(){
            new NV("\\(shallow\\)", 1),
            new NV("\\(deep\\)", 2),
            new NV("\\(very deep\\)", 3),
            new NV("", 99)
        };

        private static List<NV> NetCords = new List<NV>(){
            new NV("\\(net cord\\)", 1),
            new NV("", 2)
        };

        private static List<NV> Faults = new List<NV>(){
            new NV("\\(net\\)", 1),
            new NV("\\(wide\\)", 2),
            new NV("\\(long\\)", 3),
            new NV("\\(wide and long\\)", 4),
            new NV("\\(foot fault\\)", 5),
            new NV("\\(shank\\)", 6),
            new NV("\\(\\)", 99),
            new NV("", NO_FAULT)
        };

        private static List<NV> Outcomes = new List<NV>(){
            new NV("unforced error", 4),
            new NV("forced error", 3),
            new NV("service winner", 6),
            new NV("winner", 5),
            new NV("fault", 2),
            new NV("ace", 1),
            new NV("", NO_OUTCOME)
        };

        private static List<NV> Directions = new List<NV>(){
            new NV("down the middle", DIR_MIDDLE),  //2
            new NV("down the T", DIR_T),    //6
            new NV("to body", DIR_BODY),
            new NV("wide", DIR_WIDE),
            new NV("crosscourt", DIR_CROSSCOURT),
            new NV("down the line", DIR_DOWN_LINE),
            new NV("inside-in", DIR_INSIDE_IN),
            new NV("inside-out", DIR_INSIDE_OUT)
        };

        private static List<NV> ApproachShots = new List<NV>(){
            new NV("approach shot", 1),
            new NV("", 2)
        };

        public byte Type; // 1st server, 2nd server, 3-return, 4-rally
        public byte Shot; 
        // 1-forehand, 22-backhand, 3-forehand chip/slice, 24-backhand slice, 5-forehand volley, 26-backhand volley, 7-smash, 28-backhand smash
        // 9-forehand drop shot, 30-backhand drop shot, 11-forehand lob, 32-backhand lob, 13-forehand half-volley, 34-backhand half-volley
        // 15-forhand swinging volley, 36-backhand swinging volley, 41-trick shot, 99-unknown shot
        public byte HitPosition;    //1-at net, 2-at baseline, 99-other
        public byte HitCourt;   //1-de_ct, 2-middle_ct, 3-ad_ct, 99-other
        public byte ToCourt;  //1-de_ct, 2-middle_ct, 3-ad_ct, 99-other
        public byte Direction;  //1-de_ct, 2-down the middle, 3-ad_ct, 4-wide, 5-to body, 6-down the T, 99-other
        public byte Depth;  //1-(shallow), 2-(deep), 3-(very deep), 99-other
        public byte NetCord;    //1-(net cord), 2-not
        public byte Fault;  //1-(net), 2-(wide), 3-(long), 4-(wide and long), 5-(foot fault), 6-(shank), 7-no fault, 99-other
        public byte Outcome;    //1-ace, 2-fault, 3-forced error, 4-unforced error, 5-winner, 6-service winner, 7-no outcome
        public byte Hand;   //1-Right, 2-Left
        public byte Player;  //1-Player1, 2-Player2
        public string Description;
        public Point Point;
        public Stroke Prev;
        public Stroke Next;
        public byte ApproachShot;    //1-approach shot, 2-not

        public static byte WhatOutcome(byte outcome)
        {
            if (outcome == 1 || outcome == 5 || outcome == 6)
            {
                return OUTCOME_WIN;
            }
            else if (outcome ==2 || outcome==3 || outcome == 4)
            {
                return OUTCOME_LOSE;
            }
            else
            {
                return NO_OUTCOME;
            }
        }
        public static byte HandOfShot(byte shot)
        {
            if (shot <= 20) { return FOREHAND_SHOTS; }
            else if (shot <= 40) { return BACKHAND_SHOTS; }
            else { return UNKNOWN_HAND_SHOT; }
        }
        public static byte GetValue(List<NV> NVs, ref string str)
        {
            foreach (NV nv in NVs)
            {
                if (Regex.IsMatch(str, nv.Name))
                {
                    str = Regex.Replace(str, nv.Name, "");
                    return nv.Value;
                }
            }
            return 99;
        }

        public Stroke(string str, byte hand, Stroke prev, byte serviceCourt)
        {
            Description = str;
            Type = GetValue(Types, ref str);
            Shot = GetValue(Shots, ref str);
            HitPosition = GetValue(HitPositions, ref str);
            Depth = GetValue(Depths, ref str);
            NetCord = GetValue(NetCords, ref str);
            Fault = GetValue(Faults, ref str);
            Outcome = GetValue(Outcomes, ref str);
            if ((Outcome == 2 || Outcome == 3 || Outcome == 4) && (Fault == NO_FAULT))
            {
                Fault = 99;
            }
            Direction = GetValue(Directions, ref str);
            ApproachShot = GetValue(ApproachShots, ref str);
            HitCourt = 99;
            ToCourt = 99;
            Hand = hand;
            if (Direction == DIR_MIDDLE)
            {
                ToCourt = MIDDLE_CT;
            }
            else
            {
                if (HandOfShot(Shot) == FOREHAND_SHOTS)  // FOREHAND SHOT
                {
                    if (Direction == DIR_CROSSCOURT || Direction == DIR_INSIDE_IN)
                    {
                        if (hand == RIGHT_HAND) { ToCourt = DE_CT; } else { ToCourt = AD_CT; }
                    }
                    else if (Direction == DIR_DOWN_LINE || Direction == DIR_INSIDE_OUT)
                    {
                        if (hand == RIGHT_HAND) { ToCourt = AD_CT; } else { ToCourt = DE_CT; }
                    }
                }
                else if (HandOfShot(Shot) == BACKHAND_SHOTS) // BACKHAND SHOT
                {
                    if (Direction == DIR_CROSSCOURT || Direction == DIR_INSIDE_IN)
                    {
                        if (hand == RIGHT_HAND) { ToCourt = AD_CT; } else { ToCourt = DE_CT; }
                    }
                    else if (Direction == DIR_DOWN_LINE || Direction == DIR_INSIDE_OUT)
                    {
                        if (hand == RIGHT_HAND) { ToCourt = DE_CT; } else { ToCourt = AD_CT; }
                    }
                }
                // Unknown hand shot, mostly 1st/2nd serve, see below. If it is unknown return/rally, ToCourt=99
            }
            if (Type == TYPE_1ST_SERVE || Type == TYPE_2ND_SERVE)
            {
                HitCourt = serviceCourt;
                ToCourt = serviceCourt;
                if (Direction == DIR_T)
                {
                    ToCourt = MIDDLE_CT;
                }
            }
            else if (Type == TYPE_RETURN)
            {
                HitCourt = serviceCourt;
                if (prev != null && prev.ToCourt == MIDDLE_CT) 
                {
                    HitCourt = MIDDLE_CT;
                }
            }
            else
            {
                if (prev != null)
                {
                    HitCourt = prev.ToCourt;
                }
                else
                {
                    HitCourt = 99;
                }
            }
            if (HitCourt == 99)  // if prev.ToCourt is unknown, HitCourt will be unknown too, 
                // use current Direction to infer the HitCourt and prev.ToCourt
            {
                if ((hand == RIGHT_HAND && HandOfShot(Shot) == FOREHAND_SHOTS)
                    || (hand != RIGHT_HAND && HandOfShot(Shot) == BACKHAND_SHOTS)) 
                {
                    if (Direction == DIR_CROSSCOURT || Direction == DIR_DOWN_LINE)
                    {
                        HitCourt = DE_CT;
                    }
                    else if (Direction == DIR_INSIDE_OUT || Direction == DIR_INSIDE_IN)
                    {
                        HitCourt = AD_CT;
                    }
                }
                else if ((hand == RIGHT_HAND && HandOfShot(Shot) == BACKHAND_SHOTS)
                    || (hand != RIGHT_HAND && HandOfShot(Shot) == FOREHAND_SHOTS)) 
                {
                    if (Direction == DIR_CROSSCOURT || Direction == DIR_DOWN_LINE)
                    {
                        HitCourt = AD_CT;
                    }
                    else if (Direction == DIR_INSIDE_OUT || Direction == DIR_INSIDE_IN)
                    {
                        HitCourt = DE_CT;
                    }
                }
                if (prev != null) prev.ToCourt = HitCourt;
            }
            if (prev != null)
            {
                Prev = prev;
                Prev.Next = this;
            }
            else
            {
                Prev = null;
            }
            Next = null;
        }

        public Stroke(string str, byte hand, byte prevDirection, byte serviceCourt)
        {
            Description = str;
            Type = GetValue(Types, ref str);
            //if (Type == 3 || Type == 4)
            //{
                Shot = GetValue(Shots, ref str);
                HitPosition = GetValue(HitPositions, ref str);
                Depth = GetValue(Depths, ref str);
            //}
            NetCord = GetValue(NetCords, ref str);
            Fault = GetValue(Faults, ref str);
            Outcome = GetValue(Outcomes, ref str);
            if ((Outcome == 2 || Outcome == 3 || Outcome == 4) && (Fault == NO_FAULT))
            {
                Fault = 99;
            }
            Direction = GetValue(Directions, ref str);
            if (HandOfShot(Shot) == FOREHAND_SHOTS)  // FOREHAND SHOT
            {
                if (Direction == DIR_CROSSCOURT || Direction == DIR_INSIDE_IN)
                {
                    if (hand == RIGHT_HAND) { ToCourt = DE_CT; } else { ToCourt = AD_CT; }
                }
                else if (Direction == DIR_DOWN_LINE || Direction == DIR_INSIDE_OUT)
                {
                    if (hand == RIGHT_HAND) { ToCourt = AD_CT; } else { ToCourt = DE_CT; }
                }
                else if (Direction == DIR_MIDDLE)
                {
                    ToCourt = MIDDLE_CT;
                }
            }
            else if (HandOfShot(Shot) == BACKHAND_SHOTS) // BACKHAND SHOT
            {
                if (Direction == DIR_CROSSCOURT || Direction == DIR_INSIDE_IN)
                {
                    if (hand == RIGHT_HAND) { ToCourt = AD_CT; } else { ToCourt = DE_CT; }
                }
                else if (Direction == DIR_DOWN_LINE || Direction == DIR_INSIDE_OUT)
                {
                    if (hand == RIGHT_HAND) { ToCourt = DE_CT; } else { ToCourt = AD_CT; }
                }
                else if (Direction == DIR_MIDDLE)
                {
                    ToCourt = MIDDLE_CT;
                }
            }
            Hand = hand;
            if (Type == TYPE_1ST_SERVE || Type == TYPE_2ND_SERVE)
            {
                HitCourt = serviceCourt;
                ToCourt = serviceCourt;
            }
            else if (Type == TYPE_RETURN)
            {
                HitCourt = serviceCourt;
            }
            else
            {
                HitCourt = prevDirection;
            }
            Prev = null;
            Next = null;
        }

        public Stroke(byte type, string str, byte hand, byte prevDirection, byte serviceCourt)
        {
            string directionPattern;
            string shotPattern = "[fbrsvzopuylmhijktq]";
            string hitPositionPattern = "[-=]";
            string depthPattern = "[789]";
            string netCordPattern = "[;]";
            string faultPattern = "[nwdxg!e]";
            string outcomePattern = "[*#@nwdxg!e]";

            Type = type;
            Description = str;
            if (type == 1 || type == 2)
            {
                directionPattern = "[4560]";
                Shot = 99;
                HitPosition = 99;
                HitCourt = serviceCourt;
                Depth = 99;
                NetCord = 2;
                if (Regex.IsMatch(str, outcomePattern))
                {
                    foreach (Match m in Regex.Matches(str, outcomePattern))
                    {
                        switch (m.Value)
                        {
                            case "*":
                                Outcome = 1;
                                break;
                            case "#":
                                Outcome = 6;
                                break;
                            case "@":
                                Outcome = 6;
                                break;
                            default:
                                Outcome = 2;
                                break;
                        }
                        break;
                    }
                }
                else
                {
                    Outcome = 7;
                }
            }
            else
            {
                #region return and other rally
                directionPattern = "[1230]";
                #region shotPattern
                if (Regex.IsMatch(str, shotPattern))
                {
                    foreach (Match m in Regex.Matches(str, shotPattern))
                    {
                        switch (m.Value)
                        {
                            case "f":
                                Shot = 1;
                                break;
                            case "b":
                                Shot = 22;
                                break;
                            case "r":
                                Shot = 3;
                                break;
                            case "s":
                                Shot = 24;
                                break;
                            case "v":
                                Shot = 5;
                                break;
                            case "z":
                                Shot = 26;
                                break;
                            case "o":
                                Shot = 7;
                                break;
                            case "p":
                                Shot = 28;
                                break;
                            case "u":
                                Shot = 9;
                                break;
                            case "y":
                                Shot = 30;
                                break;
                            case "l":
                                Shot = 11;
                                break;
                            case "m":
                                Shot = 32;
                                break;
                            case "h":
                                Shot = 13;
                                break;
                            case "i":
                                Shot = 34;
                                break;
                            case "j":
                                Shot = 15;
                                break;
                            case "k":
                                Shot = 36;
                                break;
                            case "t":
                                Shot = 41;
                                break;
                            default:
                                Shot = 99;
                                break;
                        }
                        break;
                    }
                } else {
                    Shot = 99;
                }
                #endregion
                if (Regex.IsMatch(str, hitPositionPattern))
                {
                    foreach (Match m in Regex.Matches(str, hitPositionPattern))
                    {
                        switch (m.Value)
                        {
                            case "-":
                                HitPosition = 1;
                                break;
                            case "=":
                                HitPosition = 2;
                                break;
                            default:
                                HitPosition = 99;
                                break;
                        }
                        break;
                    }
                }
                else
                {
                    HitPosition = 99;
                }
                if (Regex.IsMatch(str, depthPattern))
                {
                    foreach (Match m in Regex.Matches(str, depthPattern))
                    {
                        switch(m.Value)
                        {
                            case "7":
                                Depth = 1;
                                break;
                            case "8":
                                Depth = 2;
                                break;
                            case "9":
                                Depth = 3;
                                break;
                            default:
                                Depth = 99;
                                break;
                        }
                        break;
                    }
                }
                else
                {
                    Depth = 99;
                }
                if (Regex.IsMatch(str, netCordPattern))
                {
                    foreach (Match m in Regex.Matches(str, netCordPattern))
                    {
                        switch (m.Value)
                        {
                            case ";":
                                NetCord = 1;
                                break;
                            default :
                                NetCord = 2;
                                break;
                        }
                        break;
                    }
                }
                else
                {
                    NetCord = 2;
                }
                if (Regex.IsMatch(str, outcomePattern))
                {
                    foreach (Match m in Regex.Matches(str, outcomePattern))
                    {
                        switch (m.Value)
                        {
                            case "*":
                                Outcome = 5;
                                break;
                            case "#":
                                Outcome = 3;
                                break;
                            case "@":
                                Outcome = 4;
                                break;
                            default:
                                Outcome = 4;
                                break;
                        }
                        break;
                    }
                }
                else
                {
                    Outcome = 7;
                }
                if (type == 3)
                {
                    if (prevDirection == 4 || prevDirection == 5)
                    {
                        HitCourt = serviceCourt;
                    }
                    else if (prevDirection == 6)
                    {
                        HitCourt = 2;
                    }
                    else
                    {
                        HitCourt = 99;
                    }
                }
                else
                {
                    HitCourt = prevDirection;
                }
                #endregion
            }
            if (Regex.IsMatch(str, directionPattern))
            {
                foreach (Match m in Regex.Matches(str, directionPattern))
                {
                    Direction = byte.Parse(m.Value);
                    if (Direction == 0) Direction = 99;
                    break;
                }
            } else {
                Direction = 99;
            }
            if (Regex.IsMatch(str, faultPattern))
            {
                foreach (Match m in Regex.Matches(str, faultPattern))
                {
                    switch (m.Value)
                    {
                        case "n":
                            Fault = 1;
                            break;
                        case "w":
                            Fault = 2;
                            break;
                        case "d":
                            Fault = 3;
                            break;
                        case "x":
                            Fault = 4;
                            break;
                        case "g":
                            Fault = 5;
                            break;
                        case "!":
                            Fault = 6;
                            break;
                        default :
                            Fault = 99;
                            break;
                    }
                    break;
                }
            }
            else
            {
                Fault = 7;
            }
            Hand = hand;
        }
        
        public Stroke()
        {

        }

        public override string ToString()
        {
            string result = "";
            string type = "";
            string dir = "";
            string dir2 = "";
            string shot = "";
            string outcome = "";
            string depth = "";
            string fault = "";
            string netcord = "";
            string approachshot = "";
            string hitposition = "";
            NV nv = Types.FirstOrDefault(x => x.Value == Type);
            if (nv != null) type = nv.Name;
            nv = Shots.FirstOrDefault(x => x.Value == Shot);
            if (nv != null) shot = nv.Name;
            if (Type != TYPE_1ST_SERVE && Type != TYPE_2ND_SERVE)
            {
                if (shot.Equals("")) shot = "unknown shot";
            }
            nv = Directions.FirstOrDefault(x => x.Value == Direction);
            if (nv != null) dir = nv.Name;
            if (ToCourt == MIDDLE_CT) dir2 = Directions.FirstOrDefault(x => x.Value == DIR_MIDDLE).Name;
            if (
                (
                (Hand==RIGHT_HAND && HandOfShot(Shot)==FOREHAND_SHOTS)
                || (Hand != RIGHT_HAND && HandOfShot(Shot) == BACKHAND_SHOTS)
                )
                    && ToCourt==DE_CT
                    && (HitCourt==DE_CT || HitCourt==MIDDLE_CT)
                    )
            {
                dir2 = Directions.FirstOrDefault(x => x.Value == DIR_CROSSCOURT).Name;
            }
            else if (
                (
                (Hand == RIGHT_HAND && HandOfShot(Shot) == FOREHAND_SHOTS)
                || (Hand != RIGHT_HAND && HandOfShot(Shot) == BACKHAND_SHOTS)
                )
                    && ToCourt == AD_CT
                    && (HitCourt == DE_CT)
                    )
            {
                dir2 = Directions.FirstOrDefault(x => x.Value == DIR_DOWN_LINE).Name;
            }
            else if (
                (
                (Hand == RIGHT_HAND && HandOfShot(Shot) == FOREHAND_SHOTS)
                || (Hand != RIGHT_HAND && HandOfShot(Shot) == BACKHAND_SHOTS)
                )
                    && ToCourt == AD_CT
                    && (HitCourt == AD_CT || HitCourt == MIDDLE_CT)
                    )
            {
                dir2 = Directions.FirstOrDefault(x => x.Value == DIR_INSIDE_OUT).Name;
            }
            else if (
                (
                (Hand == RIGHT_HAND && HandOfShot(Shot) == FOREHAND_SHOTS)
                || (Hand != RIGHT_HAND && HandOfShot(Shot) == BACKHAND_SHOTS)
                )
                    && ToCourt == DE_CT
                    && (HitCourt == AD_CT )
                    )
            {
                dir2 = Directions.FirstOrDefault(x => x.Value == DIR_INSIDE_IN).Name;
            }
            else if (
                (
                (Hand == RIGHT_HAND && HandOfShot(Shot) == BACKHAND_SHOTS)
                || (Hand != RIGHT_HAND && HandOfShot(Shot) == FOREHAND_SHOTS)
                )
                    && ToCourt == AD_CT
                    && (HitCourt == AD_CT || HitCourt == MIDDLE_CT)
                    )
            {
                dir2 = Directions.FirstOrDefault(x => x.Value == DIR_CROSSCOURT).Name;
            }
            else if (
                (
                (Hand == RIGHT_HAND && HandOfShot(Shot) == BACKHAND_SHOTS)
                || (Hand != RIGHT_HAND && HandOfShot(Shot) == FOREHAND_SHOTS)
                )
                    && ToCourt == DE_CT
                    && (HitCourt == AD_CT)
                    )
            {
                dir2 = Directions.FirstOrDefault(x => x.Value == DIR_DOWN_LINE).Name;
            }
            else if (
                (
                (Hand == RIGHT_HAND && HandOfShot(Shot) == BACKHAND_SHOTS)
                || (Hand != RIGHT_HAND && HandOfShot(Shot) == FOREHAND_SHOTS)
                )
                    && ToCourt == DE_CT
                    && (HitCourt == DE_CT || HitCourt == MIDDLE_CT)
                    )
            {
                dir2 = Directions.FirstOrDefault(x => x.Value == DIR_INSIDE_OUT).Name;
            }
            else if (
                (
                (Hand == RIGHT_HAND && HandOfShot(Shot) == BACKHAND_SHOTS)
                || (Hand != RIGHT_HAND && HandOfShot(Shot) == FOREHAND_SHOTS)
                )
                    && ToCourt == AD_CT
                    && (HitCourt == DE_CT)
                    )
            {
                dir2 = Directions.FirstOrDefault(x => x.Value == DIR_INSIDE_IN).Name;
            }

            nv = Outcomes.FirstOrDefault(x => x.Value == Outcome);
            if (nv != null) outcome = nv.Name;
            nv = Depths.FirstOrDefault(x => x.Value == Depth);
            if (nv != null) depth = nv.Name.Replace("\\", "");
            nv = Faults.FirstOrDefault(x => x.Value == Fault);
            if (nv != null) fault = nv.Name.Replace("\\", "");
            nv = NetCords.FirstOrDefault(x => x.Value == NetCord);
            if (nv != null) netcord = nv.Name.Replace("\\", "");
            nv = ApproachShots.FirstOrDefault(x => x.Value == ApproachShot);
            if (nv != null) approachshot = nv.Name;
            nv = HitPositions.FirstOrDefault(x => x.Value == HitPosition);
            if (nv != null) hitposition = nv.Name;

            if (Type == TYPE_1ST_SERVE || Type == TYPE_2ND_SERVE)
            {
                result = string.Format("{0} {1}", 
                    type, 
                    dir);
            }
            else if (Type == TYPE_RETURN)
            {
                result = string.Format("{0} return", shot);
                if (!approachshot.Equals(""))
                {
                    result = string.Format("{0} {1}", result, approachshot);
                }
                if (!hitposition.Equals(""))
                {
                    result = string.Format("{0} {1}", result, hitposition);
                }
                if (!dir2.Equals(""))
                {
                    result = string.Format("{0} {1}", result, dir2);
                }
            }
            else
            {
                result = string.Format("{0}", shot);
                if (!approachshot.Equals(""))
                {
                    result = string.Format("{0} {1}", result, approachshot);
                }
                if (!hitposition.Equals(""))
                {
                    result = string.Format("{0} {1}", result, hitposition);
                }
                result = string.Format("{0} {1}", result, dir2);
            }

            if (!depth.Equals(""))
            {
                result = string.Format("{0} {1}", result, depth);
            }

            if (Type == TYPE_1ST_SERVE || Type == TYPE_2ND_SERVE)
            {
                if (!outcome.Equals(""))
                {
                    result = string.Format("{0}, {1}", result.Trim(), outcome);
                }

                if (!fault.Equals("") && !fault.Equals("()"))
                {
                    result = string.Format("{0} {1}", result.Trim(), fault);
                }

                if (Type == TYPE_2ND_SERVE)
                {
                    if (!fault.Equals(""))
                    {
                        result = string.Format("{0}, double fault", result);
                    }
                }
            }
            else
            {
                if (!netcord.Equals(""))
                {
                    result = string.Format("{0} {1}", result.Trim(), netcord);
                }

                if (!fault.Equals("") && !fault.Equals("()"))
                {
                    result = string.Format("{0} {1}", result.Trim(), fault);
                }

                if (!outcome.Equals(""))
                {
                    result = string.Format("{0}, {1}", result.Trim(), outcome);
                }
            }


            return result.Trim();
        }
    }
}
