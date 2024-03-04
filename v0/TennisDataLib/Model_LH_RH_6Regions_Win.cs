using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace TennisData
{
    class Model_LH_RH_6Regions_Win
    {
        public static List<Stroke> Convert(List<Stroke> temp)
        {
            List<Stroke> result = new List<Stroke>();
            for (int i = 0; i < temp.Count; i++)
            {
                Stroke s = temp[i];
                if (s.Type == Stroke.TYPE_1ST_SERVE)
                {
                    result.Add(s);
                }
                else if (s.Type == Stroke.TYPE_2ND_SERVE)
                {
                    result.Add(s);
                }
                else
                {
                    result.Add(s);
                }
            }
            Stroke lastStroke = temp[temp.Count - 1];
            #region Last Stroke has NO_OUTCOME
            if (lastStroke.Outcome == Stroke.NO_OUTCOME)
            {
                // Without a win/lose outcome, this is an invalid point, most likely unknown shot happens here!
                //For example:
                //Roger Federer   0‑0 2‑1 0‑15    1st serve wide, fault(net). 2nd serve wide; forehand return, forced error.
                //Roger Federer   0‑0 2‑1 15‑15   Unknown.
                //Roger Federer   0‑0 2‑1 30‑15   1st serve wide, fault(net). 2nd serve wide; forehand return crosscourt, forced error.
                byte curServer = lastStroke.Point.Server;
                string curPts1 = lastStroke.Point.Pts1;
                string curPts2 = lastStroke.Point.Pts2;
                byte nextServer = 99;
                string nextPts1 = "";
                string nextPts2 = "";
                if (lastStroke.Point.NextPt != null)
                {
                    nextServer = lastStroke.Point.NextPt.Server;
                    nextPts1 = lastStroke.Point.NextPt.Pts1;
                    nextPts2 = lastStroke.Point.NextPt.Pts2;
                }
                byte winner = TennisFunction.PointWonByPlayer(curServer, curPts1, curPts2, nextServer, nextPts1, nextPts2);
                if (winner == lastStroke.Player)
                {
                    // Player won this stroke
                    if (lastStroke.Prev == null)
                    {
                        lastStroke.Type = Stroke.TYPE_1ST_SERVE;
                        lastStroke.Outcome = Stroke.SERVICE_WINNER;
                        lastStroke.HitCourt = lastStroke.Point.ServiceCourt;
                        lastStroke.Direction = Stroke.DIR_BODY;
                    }
                    else
                    {
                        lastStroke.Outcome = Stroke.WINNER;
                    }
                }
                else if (winner == (byte)(3 - lastStroke.Player))
                {
                    // Player lose this strok
                    if (lastStroke.Prev == null)
                    {
                        lastStroke.Type = Stroke.TYPE_1ST_SERVE;
                        lastStroke.Outcome = Stroke.FAULT;
                        lastStroke.HitCourt = lastStroke.Point.ServiceCourt;
                        lastStroke.Fault = 99;
                        Stroke s = new Stroke();
                        s.Player = lastStroke.Player;
                        s.Hand = lastStroke.Hand;
                        s.Type = Stroke.TYPE_2ND_SERVE;
                        s.Outcome = Stroke.FAULT;
                        s.HitCourt = lastStroke.HitCourt;
                        s.Fault = 99;
                        s.Prev = lastStroke;
                        s.Prev.Next = s;
                        s.Description = "Unknown";
                        s.Point = lastStroke.Point;
                        result.Add(s);
                    }
                    else
                    {
                        lastStroke.Outcome = Stroke.FAULT;
                        lastStroke.Fault = 99;
                    }
                }
                // Totally unknown
            }
            #endregion

            #region Last Stroke is Win
            if ((lastStroke.Type == Stroke.TYPE_RETURN || lastStroke.Type == Stroke.TYPE_RALLY) 
                && Stroke.WhatOutcome(lastStroke.Outcome) == Stroke.OUTCOME_WIN)
            {
                // Win, we must count as opponent's lose
                Stroke s = new Stroke();
                s.Prev = lastStroke;
                s.Prev.Next = s;
                if (lastStroke.Player == 1)
                {
                    s.Player = 2;
                    if (lastStroke.HitCourt == Stroke.DE_CT)
                    {
                        if (lastStroke.Direction == Stroke.DIR_MIDDLE)
                        {
                            s.HitCourt = Stroke.MIDDLE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_CROSSCOURT)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_OUT))
                        {

                            s.HitCourt = Stroke.DE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_DOWN_LINE)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_IN))
                        {

                            s.HitCourt = Stroke.AD_CT;
                        }
                    }
                    else if (lastStroke.HitCourt == Stroke.MIDDLE_CT)
                    {
                        if (lastStroke.Direction == Stroke.DIR_MIDDLE)
                        {
                            s.HitCourt = Stroke.MIDDLE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_CROSSCOURT)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_OUT))
                        {

                            s.HitCourt = Stroke.DE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_OUT)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_CROSSCOURT))
                        {

                            s.HitCourt = Stroke.AD_CT;
                        }
                    }
                    else if (lastStroke.HitCourt == Stroke.AD_CT)
                    {
                        if (lastStroke.Direction == Stroke.DIR_MIDDLE)
                        {
                            s.HitCourt = Stroke.MIDDLE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_IN)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_DOWN_LINE))
                        {

                            s.HitCourt = Stroke.DE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_OUT)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_CROSSCOURT))
                        {

                            s.HitCourt = Stroke.AD_CT;
                        }
                    }
                }
                if (lastStroke.Player == 2)
                {
                    s.Player = 1;
                    if (lastStroke.HitCourt == Stroke.DE_CT)
                    {
                        if (lastStroke.Direction == Stroke.DIR_MIDDLE)
                        {
                            s.HitCourt = Stroke.MIDDLE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_CROSSCOURT)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_OUT))
                        {

                            s.HitCourt = Stroke.DE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_DOWN_LINE)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_IN))
                        {

                            s.HitCourt = Stroke.AD_CT;
                        }
                    }
                    else if (lastStroke.HitCourt == Stroke.MIDDLE_CT)
                    {
                        if (lastStroke.Direction == Stroke.DIR_MIDDLE)
                        {
                            s.HitCourt = Stroke.MIDDLE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_CROSSCOURT)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_OUT))
                        {

                            s.HitCourt = Stroke.DE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_OUT)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_CROSSCOURT))
                        {

                            s.HitCourt = Stroke.AD_CT;
                        }
                    }
                    else if (lastStroke.HitCourt == Stroke.AD_CT)
                    {
                        if (lastStroke.Direction == Stroke.DIR_MIDDLE)
                        {
                            s.HitCourt = Stroke.MIDDLE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_IN)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_DOWN_LINE))
                        {

                            s.HitCourt = Stroke.DE_CT;
                        }
                        else if ((Stroke.HandOfShot(lastStroke.Shot) == Stroke.FOREHAND_SHOTS && lastStroke.Direction == Stroke.DIR_INSIDE_OUT)
                            || (Stroke.HandOfShot(lastStroke.Shot) == Stroke.BACKHAND_SHOTS && lastStroke.Direction == Stroke.DIR_CROSSCOURT))
                        {

                            s.HitCourt = Stroke.AD_CT;
                        }
                    }
                }
                s.Type = Stroke.TYPE_RALLY;
                s.Fault = 99;
                s.Outcome = Stroke.FAULT;
                s.Description = s.ToString();
                result.Add(s);
            }
            #endregion
            return result;
        }

        public static Dictionary<string, int> CountEvents(List<Stroke> strokes)
        {
            Dictionary<string, int> Nums = new Dictionary<string, int>();
            Nums.Add("De_Ply1Serve.Win", strokes.Count(s =>
                s.Player == 2
                && (s.Type == Stroke.TYPE_1ST_SERVE)
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN));
            Nums.Add("De_Ply1Serve.ServeT_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply1Serve.ServeWide_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply1Serve.ServeBody_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("De_Ply1Serve.Serve_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                ));

            Nums.Add("De_Ply1Serve_2nd.Win", strokes.Count(s =>
                s.Player == 2
                && (s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN));
            Nums.Add("De_Ply1Serve_2nd.ServeT_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply1Serve_2nd.ServeWide_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply1Serve_2nd.ServeBody_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("De_Ply1Serve_2nd.Serve_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                ));

            Nums.Add("Ad_Ply1Serve.Win", strokes.Count(s =>
                s.Player == 2
                && (s.Type == Stroke.TYPE_1ST_SERVE)
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN));
            Nums.Add("Ad_Ply1Serve.ServeT_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply1Serve.ServeWide_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply1Serve.ServeBody_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("Ad_Ply1Serve.Serve_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                ));

            Nums.Add("Ad_Ply1Serve_2nd.Win", strokes.Count(s =>
                s.Player == 2
                && (s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN));
            Nums.Add("Ad_Ply1Serve_2nd.ServeT_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply1Serve_2nd.ServeWide_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply1Serve_2nd.ServeBody_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("Ad_Ply1Serve_2nd.Serve_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                ));

            Nums.Add("Ply2_de_stroke.BH_Crosscourt", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_de_stroke.BH_Downline", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_DOWN_LINE
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_de_stroke.BH_DownMid", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_de_stroke.FH_InsideIn", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_IN
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_de_stroke.FH_InsideOut", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_de_stroke.FH_DownMid", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_de_stroke.Error", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT 
                && s.Fault != Stroke.NO_FAULT));

            Nums.Add("Ply2_mid_stroke.FH_InsideOut", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_mid_stroke.FH_Crosscourt", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_mid_stroke.FH_DownMid", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_mid_stroke.BH_InsideOut", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_mid_stroke.BH_Crosscourt", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_mid_stroke.BH_DownMid", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_mid_stroke.Error", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT
                && s.Fault != Stroke.NO_FAULT));

            Nums.Add("Ply2_ad_stroke.FH_Crosscourt", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ad_stroke.FH_Downline", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_DOWN_LINE
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ad_stroke.FH_DownMid", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ad_stroke.BH_InsideIn", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_IN
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ad_stroke.BH_InsideOut", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ad_stroke.BH_DownMid", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ad_stroke.Error", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT
                && s.Fault != Stroke.NO_FAULT));

            Nums.Add("De_Ply2Serve.Win", strokes.Count(s =>
                s.Player == 1
                && (s.Type == Stroke.TYPE_1ST_SERVE)
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN));
            Nums.Add("De_Ply2Serve.ServeT_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply2Serve.ServeWide_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply2Serve.ServeBody_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("De_Ply2Serve.Serve_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                ));

            Nums.Add("De_Ply2Serve_2nd.Win", strokes.Count(s =>
                s.Player == 1
                && (s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN));
            Nums.Add("De_Ply2Serve_2nd.ServeT_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply2Serve_2nd.ServeWide_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply2Serve_2nd.ServeBody_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("De_Ply2Serve_2nd.Serve_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                ));

            Nums.Add("Ad_Ply2Serve.Win", strokes.Count(s =>
                s.Player == 1
                && (s.Type == Stroke.TYPE_1ST_SERVE)
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN));
            Nums.Add("Ad_Ply2Serve.ServeT_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply2Serve.ServeWide_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply2Serve.ServeBody_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("Ad_Ply2Serve.Serve_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                ));

            Nums.Add("Ad_Ply2Serve_2nd.Win", strokes.Count(s =>
                s.Player == 1
                && (s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN));
            Nums.Add("Ad_Ply2Serve_2nd.ServeT_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply2Serve_2nd.ServeWide_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply2Serve_2nd.ServeBody_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("Ad_Ply2Serve_2nd.Serve_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                ));

            Nums.Add("Ply1_de_stroke.FH_Crosscourt", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_de_stroke.FH_Downline", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_DOWN_LINE
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_de_stroke.FH_DownMid", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_de_stroke.BH_InsideIn", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_IN
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_de_stroke.BH_InsideOut", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_de_stroke.BH_DownMid", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_de_stroke.Error", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT
                && s.Fault != Stroke.NO_FAULT));

            Nums.Add("Ply1_mid_stroke.FH_Crosscourt", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_mid_stroke.FH_InsideOut", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_mid_stroke.FH_DownMid", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_mid_stroke.BH_Crosscourt", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_mid_stroke.BH_InsideOut", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_mid_stroke.BH_DownMid", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_mid_stroke.Error", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.MIDDLE_CT
                && s.Fault != Stroke.NO_FAULT));

            Nums.Add("Ply1_ad_stroke.BH_Crosscourt", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ad_stroke.BH_Downline", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_DOWN_LINE
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ad_stroke.BH_DownMid", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ad_stroke.FH_InsideIn", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_IN
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ad_stroke.FH_InsideOut", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ad_stroke.FH_DownMid", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_MIDDLE
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ad_stroke.Error", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT
                && s.Fault != Stroke.NO_FAULT));

            Nums.Add("Ply2_BackHandR.BH_CrossCourt_DE", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_BackHandR.BH_DownLine", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_DOWN_LINE
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_BackHandR.BH_DownMid_DE", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_MIDDLE
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_BackHandR.BH_Error_DE", 
                strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                );
            Nums.Add("Ply2_BackHandR.BH_InsideIn", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_IN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_BackHandR.BH_CrossCourt_AD", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_BackHandR.BH_InsideOut", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_OUT
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_BackHandR.BH_DownMid_AD", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_MIDDLE
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_BackHandR.BH_Error_AD", 
                strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                );

            Nums.Add("Ply2_ForeHandR.FH_InsideOut", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_OUT
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ForeHandR.FH_CrossCourt_DE", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ForeHandR.FH_InsideIn", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_IN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ForeHandR.FH_DownMid_DE", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_MIDDLE
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ForeHandR.FH_Error_DE", 
                strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                );
            Nums.Add("Ply2_ForeHandR.FH_CrossCourt_AD", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ForeHandR.FH_DownLine", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_DOWN_LINE
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ForeHandR.FH_DownMid_AD", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_MIDDLE
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ForeHandR.FH_Error_AD", 
                strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                );

            Nums.Add("Ply1_BackHandR.BH_CrossCourt_DE", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_BackHandR.BH_InsideIn", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_IN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_BackHandR.BH_InsideOut", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_OUT
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_BackHandR.BH_DownMid_DE", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_MIDDLE
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_BackHandR.BH_Error_DE", 
                strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                );
            Nums.Add("Ply1_BackHandR.BH_CrossCourt_AD", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_BackHandR.BH_DownLine", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_DOWN_LINE
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_BackHandR.BH_DownMid_AD", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_MIDDLE
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_BackHandR.BH_Error_AD", 
                strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                );

            Nums.Add("Ply1_ForeHandR.FH_Crosscourt_DE", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ForeHandR.FH_Downline", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_DOWN_LINE
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ForeHandR.FH_DownMid_DE", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_MIDDLE
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ForeHandR.FH_Error_DE", 
                strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                );
            Nums.Add("Ply1_ForeHandR.FH_InsideOut", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_OUT
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ForeHandR.FH_CrossCourt_AD", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ForeHandR.FH_InsideIn", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_IN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ForeHandR.FH_DownMid_AD", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_MIDDLE
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ForeHandR.FH_Error_AD", 
                strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                );

            return Nums;
        }
    }
}
