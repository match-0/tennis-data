using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace TennisData
{
    public class Model_RH_LH_4Regions
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
                    if (s.Direction == Stroke.DIR_MIDDLE)
                    {
                        #region Down the middle
                        if (s.HitCourt == Stroke.DE_CT &&
                            (
                                 (s.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS)
                              || (s.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS)
                            )
                        )
                        {
                            #region AAA
                            if (i + 1 < temp.Count)
                            {
                                Stroke oppo = temp[i + 1];
                                if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_DOWN_LINE;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_DOWN_LINE;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_DOWN_LINE;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_DOWN_LINE;
                                }
                            }
                            #endregion
                        }
                        else if (s.HitCourt == Stroke.DE_CT &&
                            (
                                 (s.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS)
                              || (s.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS)
                            )
                        )
                        {
                            #region BBB
                            if (i + 1 < temp.Count)
                            {
                                Stroke oppo = temp[i + 1];
                                if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_IN;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_IN;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_IN;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_IN;
                                }
                            }
                            #endregion
                        }
                        else if (s.HitCourt == Stroke.AD_CT &&
                            (
                                 (s.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS)
                              || (s.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS)
                            )
                        )
                        {
                            #region CCC
                            if (i + 1 < temp.Count)
                            {
                                Stroke oppo = temp[i + 1];
                                if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_IN;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_IN;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_IN;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_IN;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                            }
                            #endregion
                        }
                        else if (s.HitCourt == Stroke.AD_CT &&
                            (
                                 (s.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS)
                              || (s.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS)
                            )
                        )
                        {
                            #region DDD
                            if (i + 1 < temp.Count)
                            {
                                Stroke oppo = temp[i + 1];
                                if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_DOWN_LINE;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_DOWN_LINE;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_DOWN_LINE;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_DOWN_LINE;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                            }
                            #endregion
                        }
                        else if (s.HitCourt == Stroke.MIDDLE_CT &&
                            (
                                 (s.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS)
                              || (s.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS)
                            )
                        )
                        {
                            #region EEE
                            if (i + 1 < temp.Count)
                            {
                                Stroke oppo = temp[i + 1];
                                if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                            }
                            #endregion
                        }
                        else if (s.HitCourt == Stroke.MIDDLE_CT &&
                            (
                                 (s.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS)
                              || (s.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS)
                            )
                        )
                        {
                            #region FFF
                            if (i + 1 < temp.Count)
                            {
                                Stroke oppo = temp[i + 1];
                                if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.DE_CT;
                                    s.ToCourt = Stroke.DE_CT;
                                    s.Direction = Stroke.DIR_INSIDE_OUT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.BACKHAND_SHOTS
                                    && oppo.Direction == Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                                else if (oppo.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(oppo.Shot) == Stroke.FOREHAND_SHOTS
                                    && oppo.Direction != Stroke.DIR_INSIDE_OUT)
                                {
                                    oppo.HitCourt = Stroke.AD_CT;
                                    s.ToCourt = Stroke.AD_CT;
                                    s.Direction = Stroke.DIR_CROSSCOURT;
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                    if (s.HitCourt==Stroke.MIDDLE_CT && s.Type == Stroke.TYPE_RETURN)
                    {
                        if ((s.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS)
                              || (s.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS)
                              )
                        {
                            if (s.Point.ServiceCourt == Stroke.DE_CT)
                            {
                                s.HitCourt = Stroke.DE_CT;
                                if (s.Prev != null) s.Prev.ToCourt = Stroke.DE_CT;
                                if (s.ToCourt == Stroke.AD_CT) s.Direction = Stroke.DIR_DOWN_LINE;
                            }
                            else
                            {
                                s.HitCourt = Stroke.AD_CT;
                                if (s.Prev != null) s.Prev.ToCourt = Stroke.AD_CT;
                                if (s.ToCourt == Stroke.DE_CT) s.Direction = Stroke.DIR_INSIDE_IN;
                            }
                        }
                        else if ((s.Hand == Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS)
                              || (s.Hand != Stroke.RIGHT_HAND && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS)
                              )
                        {
                            if (s.Point.ServiceCourt == Stroke.DE_CT)
                            {
                                s.HitCourt = Stroke.DE_CT;
                                if (s.Prev != null) s.Prev.ToCourt = Stroke.DE_CT;
                                if (s.ToCourt == Stroke.AD_CT) s.Direction = Stroke.DIR_INSIDE_IN;
                            }
                            else
                            {
                                s.HitCourt = Stroke.AD_CT;
                                if (s.Prev != null) s.Prev.ToCourt = Stroke.AD_CT;
                                if (s.ToCourt == Stroke.DE_CT) s.Direction = Stroke.DIR_DOWN_LINE;
                            }
                        }
                    }
                    //if (s.Direction != Stroke.MIDDLE_CT)
                    //{
                        result.Add(s);
                    //}
                }
            }
            return result;
        }

        public static Dictionary<string, int> CountEvents(List<Stroke> strokes)
        {
            Dictionary<string, int> Nums = new Dictionary<string, int>();
            Nums.Add("De_Ply1Serve.ServeT_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply1Serve.ServeT_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply1Serve.ServeWide_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply1Serve.ServeWide_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply1Serve.ServeBody_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("De_Ply1Serve.ServeBody_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));

            Nums.Add("De_Ply1Serve_2nd.ServeT_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply1Serve_2nd.ServeT_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply1Serve_2nd.ServeWide_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply1Serve_2nd.ServeWide_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply1Serve_2nd.ServeBody_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("De_Ply1Serve_2nd.ServeBody_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));

            Nums.Add("Ad_Ply1Serve.ServeT_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply1Serve.ServeT_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply1Serve.ServeWide_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply1Serve.ServeWide_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply1Serve.ServeBody_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("Ad_Ply1Serve.ServeBody_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));

            Nums.Add("Ad_Ply1Serve_2nd.ServeT_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply1Serve_2nd.ServeT_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply1Serve_2nd.ServeWide_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply1Serve_2nd.ServeWide_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply1Serve_2nd.ServeBody_in", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("Ad_Ply1Serve_2nd.ServeBody_err", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));

            Nums.Add("Ply2_de_stroke.BH_Crosscourt", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_de_stroke.BH_Downline", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_DOWN_LINE
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_de_stroke.FH_InsideIn", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_IN
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_de_stroke.FH_InsideOut", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_de_stroke.FH_Error", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT 
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT));
            Nums.Add("Ply2_de_stroke.BH_Error", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT 
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT));

            Nums.Add("Ply2_ad_stroke.FH_Crosscourt", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ad_stroke.FH_Downline", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_DOWN_LINE
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ad_stroke.BH_InsideIn", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_IN
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ad_stroke.BH_InsideOut", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ad_stroke.FH_Error", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT));
            Nums.Add("Ply2_ad_stroke.BH_Error", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT));

            Nums.Add("De_Ply2Serve.ServeT_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply2Serve.ServeT_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply2Serve.ServeWide_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply2Serve.ServeWide_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply2Serve.ServeBody_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("De_Ply2Serve.ServeBody_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));

            Nums.Add("De_Ply2Serve_2nd.ServeT_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply2Serve_2nd.ServeT_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("De_Ply2Serve_2nd.ServeWide_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply2Serve_2nd.ServeWide_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("De_Ply2Serve_2nd.ServeBody_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("De_Ply2Serve_2nd.ServeBody_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.DE_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));

            Nums.Add("Ad_Ply2Serve.ServeT_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply2Serve.ServeT_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply2Serve.ServeWide_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply2Serve.ServeWide_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply2Serve.ServeBody_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("Ad_Ply2Serve.ServeBody_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_1ST_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));

            Nums.Add("Ad_Ply2Serve_2nd.ServeT_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply2Serve_2nd.ServeT_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_T));
            Nums.Add("Ad_Ply2Serve_2nd.ServeWide_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply2Serve_2nd.ServeWide_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_WIDE));
            Nums.Add("Ad_Ply2Serve_2nd.ServeBody_in", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault == Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));
            Nums.Add("Ad_Ply2Serve_2nd.ServeBody_err", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_2ND_SERVE
                && s.HitCourt == Stroke.AD_CT && s.Fault != Stroke.NO_FAULT
                && s.Direction == Stroke.DIR_BODY));

            Nums.Add("Ply1_de_stroke.FH_Crosscourt", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_de_stroke.FH_Downline", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_DOWN_LINE
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_de_stroke.BH_InsideIn", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_IN
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_de_stroke.BH_InsideOut", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_de_stroke.FH_Error", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT));
            Nums.Add("Ply1_de_stroke.BH_Error", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT));

            Nums.Add("Ply1_ad_stroke.BH_Crosscourt", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_CROSSCOURT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ad_stroke.BH_Downline", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_DOWN_LINE
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ad_stroke.FH_InsideIn", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_IN
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ad_stroke.FH_InsideOut", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT && s.Direction == Stroke.DIR_INSIDE_OUT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ad_stroke.FH_Error", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT));
            Nums.Add("Ply1_ad_stroke.BH_Error", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RALLY
                && s.HitCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT));

            Nums.Add("Ply2_BackHandR.BH_CrossCourt", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_BackHandR.BH_DownLine", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_DOWN_LINE
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            // Return error should include ace and service win
            Nums.Add("Ply2_BackHandR.BH_Error_DE",
                strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                +
                strokes.Count(s =>
                s.Player == 1
                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.DE_CT
                && s.Direction == Stroke.DIR_WIDE
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                );
            Nums.Add("Ply2_BackHandR.BH_InsideIn", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_IN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_BackHandR.BH_InsideOut", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_OUT
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            // Return error should include ace and service win
            Nums.Add("Ply2_BackHandR.BH_Error_AD",
                strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                +
                strokes.Count(s =>
                s.Player == 1
                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.AD_CT
                && s.Direction == Stroke.DIR_T
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                );

            Nums.Add("Ply2_ForeHandR.FH_InsideOut", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_OUT
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ForeHandR.FH_InsideIn", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_IN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            // Return error should include ace and service win
            Nums.Add("Ply2_ForeHandR.FH_Error_DE",
                strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                +
                strokes.Count(s =>
                s.Player == 1
                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.DE_CT
                && (s.Direction == Stroke.DIR_T || s.Direction == Stroke.DIR_BODY)
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                );
            Nums.Add("Ply2_ForeHandR.FH_CrossCourt", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply2_ForeHandR.FH_DownLine", strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_DOWN_LINE
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            // Return error should include ace and service win
            Nums.Add("Ply2_ForeHandR.FH_Error_AD",
                strokes.Count(s =>
                s.Player == 2 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                +
                strokes.Count(s =>
                s.Player == 1
                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.AD_CT
                && (s.Direction == Stroke.DIR_WIDE || s.Direction == Stroke.DIR_BODY)
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                );

            Nums.Add("Ply1_BackHandR.BH_InsideIn", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_IN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_BackHandR.BH_InsideOut", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_OUT
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            // Return error should include ace and service win
            Nums.Add("Ply1_BackHandR.BH_Error_DE",
                strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                +
                strokes.Count(s =>
                s.Player == 2
                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.DE_CT
                && (s.Direction == Stroke.DIR_T)
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                );
            Nums.Add("Ply1_BackHandR.BH_CrossCourt", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_BackHandR.BH_DownLine", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_DOWN_LINE
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            // Return error should include ace and service win
            Nums.Add("Ply1_BackHandR.BH_Error_AD",
                strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.BACKHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                +
                strokes.Count(s =>
                s.Player == 2
                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.AD_CT
                && (s.Direction == Stroke.DIR_WIDE)
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                );

            Nums.Add("Ply1_ForeHandR.FH_Crosscourt", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_CROSSCOURT
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ForeHandR.FH_Downline", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_DOWN_LINE
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            // Return error should include ace and service win
            Nums.Add("Ply1_ForeHandR.FH_Error_DE",
                strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.DE_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                +
                strokes.Count(s =>
                s.Player == 2
                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.DE_CT
                && (s.Direction == Stroke.DIR_WIDE || s.Direction == Stroke.DIR_BODY)
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                );
            Nums.Add("Ply1_ForeHandR.FH_InsideOut", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_OUT
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            Nums.Add("Ply1_ForeHandR.FH_InsideIn", strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Direction == Stroke.DIR_INSIDE_IN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault == Stroke.NO_FAULT));
            // Return error should include ace and service win
            Nums.Add("Ply1_ForeHandR.FH_Error_AD",
                strokes.Count(s =>
                s.Player == 1 && s.Type == Stroke.TYPE_RETURN
                && s.Point.ServiceCourt == Stroke.AD_CT
                && Stroke.HandOfShot(s.Shot) == Stroke.FOREHAND_SHOTS
                && s.Fault != Stroke.NO_FAULT)
                +
                strokes.Count(s =>
                s.Player == 2
                && (s.Type == Stroke.TYPE_1ST_SERVE || s.Type == Stroke.TYPE_2ND_SERVE)
                && s.Point.ServiceCourt == Stroke.AD_CT
                && (s.Direction == Stroke.DIR_T || s.Direction == Stroke.DIR_BODY)
                && Stroke.WhatOutcome(s.Outcome) == Stroke.OUTCOME_WIN)
                );

            return Nums;
        }
    }
}
