using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TennisData
{
    public class DatabaseFunction
    {
        static private SqlConnection GetDatabaseConnection(string name)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            return new SqlConnection(settings.ConnectionString);
        }

        static public Int16 GetPlayerRankingOnDate(string name, DateTime dt)
        {
            Int16 result = -1;
            using (SqlConnection conn = GetDatabaseConnection("TennisDataLocal"))
            {
                conn.Open();
                string searchRankAndDate = @"select rank, date from rank
                                            where name = @name
                                            and date < @rankdate
                                            order by date desc";
                using (SqlCommand cmdSearch = new SqlCommand(searchRankAndDate, conn))
                {
                    cmdSearch.Parameters.Add(new SqlParameter("name", name));
                    cmdSearch.Parameters.Add(new SqlParameter("rankdate", dt));
                    SqlDataReader reader = cmdSearch.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        result = reader.GetInt16(0);
                    }
                    reader.Close();
                }
                if (result != -1) return result;

                searchRankAndDate = @"select rank, date from wtarank
                                            where name = @name
                                            and date < @rankdate
                                            order by date desc";
                using (SqlCommand cmdSearch = new SqlCommand(searchRankAndDate, conn))
                {
                    cmdSearch.Parameters.Add(new SqlParameter("name", name));
                    cmdSearch.Parameters.Add(new SqlParameter("rankdate", dt));
                    SqlDataReader reader = cmdSearch.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        result = reader.GetInt16(0);
                    }
                    reader.Close();
                }
            }
            return result;
        }

        static public Dictionary<string, Int16> GetPlayerRanking(byte playerHand)
        {
            Dictionary<string, Int16> result = new Dictionary<string, Int16>();
            using (SqlConnection conn = GetDatabaseConnection("TennisDataLocal"))
            {
                conn.Open();
                string searchLatestRankDate = @"select max(date) from rank";
                string searchPlayers = @"select rank.rank, player.name from rank, player
                                        where
                                        rank.name = player.name and
                                        date = @rankdate and
                                        player.hand = @playerhand";
                DateTime latestRankDate = DateTime.Today;
                using (SqlCommand cmdSearch = new SqlCommand(searchLatestRankDate, conn))
                {
                    SqlDataReader reader = cmdSearch.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        latestRankDate = reader.GetDateTime(0);
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = new SqlCommand(searchPlayers, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("rankdate", latestRankDate));
                    cmd.Parameters.Add(new SqlParameter("playerhand", playerHand));
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(1).Trim(), reader.GetInt16(0));
                        }
                    }
                    reader.Close();
                }
                searchLatestRankDate = @"select max(date) from wtarank";
                searchPlayers = @"select wtarank.rank, player.name from wtarank, player
                                        where
                                        wtarank.name = player.name and
                                        wtarank.date = @rankdate and
                                        player.hand = @playerhand";
                using (SqlCommand cmdSearch = new SqlCommand(searchLatestRankDate, conn))
                {
                    SqlDataReader reader = cmdSearch.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        latestRankDate = reader.GetDateTime(0);
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = new SqlCommand(searchPlayers, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("rankdate", latestRankDate));
                    cmd.Parameters.Add(new SqlParameter("playerhand", playerHand));
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(1).Trim(), reader.GetInt16(0));
                        }
                    }
                    reader.Close();
                }
            }
            return result;
        }

        static public List<string> GetSimilarATPRankingPlayers(string name, int delta = 20)
        {
            List<string> result = new List<string>();
            using (SqlConnection conn = GetDatabaseConnection("TennisDataLocal"))
            {
                conn.Open();
                string searchPlayerHand = @"select hand from player
                                        where
                                        name = @Name";
                string searchLatestRankDate = @"select max(date) from rank";
                string searchLatestRank = @"select rank from rank
                                        where
                                        date = @RankDate and
                                        name = @Name";
                string searchPlayers = @"select rank.rank, player.name from rank, player
                                        where
                                        rank.name = player.name and
                                        date = @rankdate and
                                        player.hand = @playerhand and
                                        rank.rank >= @toprank and
                                        rank.rank <= @bottomrank";
                byte playerHand = 1;
                using (SqlCommand cmdSearch = new SqlCommand(searchPlayerHand, conn))
                {
                    cmdSearch.Parameters.Add(new SqlParameter("Name", name));
                    SqlDataReader reader = cmdSearch.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        playerHand = reader.GetByte(0);
                    }
                    reader.Close();
                }
                DateTime latestRankDate = DateTime.Today;
                using (SqlCommand cmdSearch = new SqlCommand(searchLatestRankDate, conn))
                {
                    SqlDataReader reader = cmdSearch.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        latestRankDate = reader.GetDateTime(0);
                    }
                    reader.Close();
                }
                int latestRank = 5000;
                using (SqlCommand cmdSearch = new SqlCommand(searchLatestRank, conn))
                {
                    cmdSearch.Parameters.Add(new SqlParameter("RankDate", latestRankDate));
                    cmdSearch.Parameters.Add(new SqlParameter("Name", name));
                    SqlDataReader reader = cmdSearch.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        latestRank = reader.GetInt16(0);
                    }
                    reader.Close();
                }
                int topRank = Math.Max(1, latestRank - delta);
                int bottomRank = Math.Min(5000, latestRank + delta);
                using (SqlCommand cmd = new SqlCommand(searchPlayers, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("rankdate", latestRankDate));
                    cmd.Parameters.Add(new SqlParameter("playerhand", playerHand));
                    cmd.Parameters.Add(new SqlParameter("toprank", topRank));
                    cmd.Parameters.Add(new SqlParameter("bottomrank", bottomRank));
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(1).Trim());
                        }
                    }
                    reader.Close();
                }

                if (result.Count == 0)
                {
                    // Try WTA
                    searchLatestRankDate = @"select max(date) from wtarank";
                    searchLatestRank = @"select rank from wtarank
                                        where
                                        date = @RankDate and
                                        name = @Name";
                    searchPlayers = @"select wtarank.rank, player.name from wtarank, player
                                        where
                                        wtarank.name = player.name and
                                        wtarank.date = @rankdate and
                                        player.hand = @playerhand and
                                        wtarank.rank >= @toprank and
                                        wtarank.rank <= @bottomrank";
                    latestRankDate = DateTime.Today;
                    using (SqlCommand cmdSearch = new SqlCommand(searchLatestRankDate, conn))
                    {
                        SqlDataReader reader = cmdSearch.ExecuteReader();
                        if (reader.HasRows && reader.Read())
                        {
                            latestRankDate = reader.GetDateTime(0);
                        }
                        reader.Close();
                    }
                    latestRank = 5000;
                    using (SqlCommand cmdSearch = new SqlCommand(searchLatestRank, conn))
                    {
                        cmdSearch.Parameters.Add(new SqlParameter("RankDate", latestRankDate));
                        cmdSearch.Parameters.Add(new SqlParameter("Name", name));
                        SqlDataReader reader = cmdSearch.ExecuteReader();
                        if (reader.HasRows && reader.Read())
                        {
                            latestRank = reader.GetInt16(0);
                        }
                        reader.Close();
                    }
                    topRank = Math.Max(1, latestRank - delta);
                    bottomRank = Math.Min(5000, latestRank + delta);
                    using (SqlCommand cmd = new SqlCommand(searchPlayers, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("rankdate", latestRankDate));
                        cmd.Parameters.Add(new SqlParameter("playerhand", playerHand));
                        cmd.Parameters.Add(new SqlParameter("toprank", topRank));
                        cmd.Parameters.Add(new SqlParameter("bottomrank", bottomRank));
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.Add(reader.GetString(1).Trim());
                            }
                        }
                        reader.Close();
                    }
                }
            }
            return result;
        }

        static public void UpdateMatchList(Dictionary<string, string> matches)
        {
            using (SqlConnection conn = GetDatabaseConnection("LocalDB"))
            {
                conn.Open();
                string searchMatch = "SELECT NAME, URL FROM MATCH WHERE NAME = @Name";
                string updateMatch = "UPDATE MATCH SET URL = @Url WHERE NAME = @Name";
                string insertMatch = "INSERT INTO MATCH(NAME, URL) VALUES(@Name, @Url)";

                foreach(string key in matches.Keys){
                    string value = matches[key];
                    string sql;

                    using (SqlCommand cmdSearch = new SqlCommand(searchMatch, conn)){
                        cmdSearch.Parameters.Add(new SqlParameter("Name", key));
                        SqlDataReader reader = cmdSearch.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //Do nothing here. Use update SQL to update the data
                            }
                            sql = updateMatch;
                        }
                        else
                        {
                            sql = insertMatch;
                        }
                        reader.Close();
                    }
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("Name", key));
                        cmd.Parameters.Add(new SqlParameter("Url", value));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        static public void DownloadRankingHistory(DateTime DateFrom, DateTime DateTo, string Name = "")
        {
            using (SqlConnection conn = GetDatabaseConnection("LocalDB"))
            {
                conn.Open();
                string searchRank = @"
                    select date, name
                    from rank
                    where date = @date and
                    name = @name";
                string updateRank = @"
                    update rank
                    set 
                      rank = @rank,
                      points = @points,
                      age = @age,
                      tourplayed = @tourplayed,
                      pointsdropping = @pointsdropping,
                      nextbest = @nextbest
                    where date = @date and
                    name = @name";
                string insertRank = @"
                    insert into rank(date, name, rank, points, age, tourplayed, pointsdropping, nextbest)
                    values(@date, @name, @rank, @points, @age, @tourplayed, @pointsdropping, @nextbest)";
                string deleteRank = @"
                    delete from rank
                    where date = @date and 
                    name = @name";

                for(DateTime d=DateFrom; d<=DateTo; d = d.AddDays(1)) { 
                    if (d.DayOfWeek != DayOfWeek.Monday) { continue; }
                    List<RankHt> rankList = WebFunction.GetRankHt(d);
                    if (rankList != null)
                    {
                        foreach (RankHt r in rankList)
                        {
                            using (SqlCommand cmdUpdate = new SqlCommand(updateRank, conn))
                            {
                                cmdUpdate.Parameters.Add(new SqlParameter("date", r.Date));
                                cmdUpdate.Parameters.Add(new SqlParameter("name", r.Name));
                                cmdUpdate.Parameters.Add(new SqlParameter("rank", r.Rank));
                                cmdUpdate.Parameters.Add(new SqlParameter("points", r.Points));
                                cmdUpdate.Parameters.Add(new SqlParameter("age", r.Age));
                                cmdUpdate.Parameters.Add(new SqlParameter("tourplayed", r.TourPlayed));
                                cmdUpdate.Parameters.Add(new SqlParameter("pointsdropping", r.PointsDropping));
                                cmdUpdate.Parameters.Add(new SqlParameter("nextbest", r.NextBest));

                                int rowcount = cmdUpdate.ExecuteNonQuery();

                                if (rowcount == 0)
                                {
                                    using (SqlCommand cmdInsert = new SqlCommand(insertRank, conn))
                                    {
                                        cmdInsert.Parameters.Add(new SqlParameter("date", r.Date));
                                        cmdInsert.Parameters.Add(new SqlParameter("name", r.Name));
                                        cmdInsert.Parameters.Add(new SqlParameter("rank", r.Rank));
                                        cmdInsert.Parameters.Add(new SqlParameter("points", r.Points));
                                        cmdInsert.Parameters.Add(new SqlParameter("age", r.Age));
                                        cmdInsert.Parameters.Add(new SqlParameter("tourplayed", r.TourPlayed));
                                        cmdInsert.Parameters.Add(new SqlParameter("pointsdropping", r.PointsDropping));
                                        cmdInsert.Parameters.Add(new SqlParameter("nextbest", r.NextBest));

                                        cmdInsert.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //static public void UpdateATPPlayer()
        //{
        //    using (SqlConnection conn = GetDatabaseConnection("LocalDB"))
        //    {
        //        conn.Open();
        //        string searchName = @"
        //            select distinct(name) as name
        //            from rank
        //            where name not in (select name from ATP_PLAYER)";
        //        string updateName = @"
        //            update ATP_PLAYER
        //            set code1 = @code1, code2=@code2
        //            where name = @name";
        //        string insertName = @"
        //            insert into ATP_PLAYER(code1, code2, name)
        //            values(@code1, @code2, @name)";
        //        string deleteName = @"
        //            delete from ATP_PLAYER
        //            where name = @name";

        //        List<String> names = new List<string>();
        //        using (SqlCommand cmdSearch = new SqlCommand(searchName, conn))
        //        {
        //            SqlDataReader reader = cmdSearch.ExecuteReader();
        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                {
        //                    names.Add(reader.GetString(0));
        //                }
        //            }
        //            reader.Close();
        //        }

        //        foreach (string name in names)
        //        {
        //            ATPPlayer player = WebFunction.GetATPPlayer(name);
        //            if (player != null)
        //            {
        //                using (SqlCommand cmdUpdate = new SqlCommand(updateName, conn))
        //                {
        //                    cmdUpdate.Parameters.Add(new SqlParameter("code1", player.Code1));
        //                    cmdUpdate.Parameters.Add(new SqlParameter("code2", player.Code2));
        //                    cmdUpdate.Parameters.Add(new SqlParameter("name", player.Name));

        //                    int rowcount = cmdUpdate.ExecuteNonQuery();

        //                    if (rowcount == 0)
        //                    {
        //                        using (SqlCommand cmdInsert = new SqlCommand(insertName, conn))
        //                        {
        //                            cmdInsert.Parameters.Add(new SqlParameter("code1", player.Code1));
        //                            cmdInsert.Parameters.Add(new SqlParameter("code2", player.Code2));
        //                            cmdInsert.Parameters.Add(new SqlParameter("name", player.Name));

        //                            cmdInsert.ExecuteNonQuery();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        static public void UpdateATPPlayerHand()
        {
            using (SqlConnection conn = GetDatabaseConnection("LocalDB"))
            {
                conn.Open();
                string searchPlayer = @"
                    select code1, code2
                    from atp_player";
                string updatePlayerHand = @"
                    update ATP_PLAYER
                    set hand = @hand
                    where code2 = @code2";

                List<String> urlList = new List<string>();
                using (SqlCommand cmdSearch = new SqlCommand(searchPlayer, conn))
                {
                    SqlDataReader reader = cmdSearch.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            urlList.Add(reader.GetString(0).Trim() + '/' + reader.GetString(1).Trim());
                        }
                    }
                    reader.Close();
                }

                foreach (string url in urlList)
                {
                    int hand = WebFunction.GetATPPlayerHand(url);
                    if (hand != -1)
                    {
                        string code2 = url.Split('/')[1];
                        using (SqlCommand cmdUpdate = new SqlCommand(updatePlayerHand, conn))
                        {
                            cmdUpdate.Parameters.Add(new SqlParameter("hand", hand));
                            cmdUpdate.Parameters.Add(new SqlParameter("code2", code2));

                            int rowcount = cmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        static public void DownloadATPTour(int year)
        {
            using (SqlConnection conn = GetDatabaseConnection("LocalDB"))
            {
                conn.Open();
                string updateATPTour = @"
                    update ATP_TOUR
                    set 
                      resulturl = @resulturl,
                      code1 = @code1,
                      year = @year,
                      datetill = @datetill,
                      surface = @surface
                    where
                      name = @name
                      and datefrom = @datefrom";
                string insertATPTour = @"
                    insert into ATP_TOUr(name, datefrom, resulturl, code1, year, datetill, surface)
                    values(@name, @datefrom, @resulturl, @code1, @year, @datetill, @surface)";

                List<ATPTour> tourList = WebFunction.GetATPTours(year);
                if (tourList != null)
                {
                    foreach (ATPTour r in tourList)
                    {
                        using (SqlCommand cmdUpdate = new SqlCommand(updateATPTour, conn))
                        {
                            cmdUpdate.Parameters.Add(new SqlParameter("name", r.Name));
                            cmdUpdate.Parameters.Add(new SqlParameter("datefrom", r.DateFrom));
                            cmdUpdate.Parameters.Add(new SqlParameter("resulturl", r.ResultURL));
                            cmdUpdate.Parameters.Add(new SqlParameter("code1", r.Code1));
                            cmdUpdate.Parameters.Add(new SqlParameter("year", r.Year));
                            cmdUpdate.Parameters.Add(new SqlParameter("datetill", r.DateTill));
                            cmdUpdate.Parameters.Add(new SqlParameter("surface", r.Surface));

                            int rowcount = cmdUpdate.ExecuteNonQuery();

                            if (rowcount == 0)
                            {
                                using (SqlCommand cmdInsert = new SqlCommand(insertATPTour, conn))
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("name", r.Name));
                                    cmdInsert.Parameters.Add(new SqlParameter("datefrom", r.DateFrom));
                                    cmdInsert.Parameters.Add(new SqlParameter("resulturl", r.ResultURL));
                                    cmdInsert.Parameters.Add(new SqlParameter("code1", r.Code1));
                                    cmdInsert.Parameters.Add(new SqlParameter("year", r.Year));
                                    cmdInsert.Parameters.Add(new SqlParameter("datetill", r.DateTill));
                                    cmdInsert.Parameters.Add(new SqlParameter("surface", r.Surface));

                                    cmdInsert.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                
            }
        }

        static public void DownloadATPResult()
        {
            using (SqlConnection conn = GetDatabaseConnection("LocalDB"))
            {
                conn.Open();
                string selectURL = @"
                    select distinct code1, year, resulturl from atp_tour";
                string insertATPTour = @"
                    insert into ATP_RESULT([TourCode1], [Year]
      ,[WinnerCode2]
      ,[LoserCode2]
      ,[Set1WinScore]
      ,[Set1LoseScore]
      ,[Set1TBScore]
      ,[Set2WinScore]
      ,[Set2LoseScore]
      ,[Set2TBScore]
      ,[Set3WinScore]
      ,[Set3LoseScore]
      ,[Set3TBScore]
      ,[Set4WinScore]
      ,[Set4LoseScore]
      ,[Set4TBScore]
      ,[Set5WinScore]
      ,[Set5LoseScore]
      ,[Set5TBScore]
      ,[WinTotalServe]
      ,[Win1stIn]
      ,[Win1stWon]
      ,[Win2ndWon]
      ,[LoseTotalServe]
      ,[Lose1stIn]
      ,[Lose1stWon]
      ,[Lose2ndWon])
                    values(
        @TourCode1
      , @Year
      , @WinnerCode2
      , @LoserCode2
      , @Set1WinScore
      , @Set1LoseScore
      , @Set1TBScore
      , @Set2WinScore
      , @Set2LoseScore
      , @Set2TBScore
      , @Set3WinScore
      , @Set3LoseScore
      , @Set3TBScore
      , @Set4WinScore
      , @Set4LoseScore
      , @Set4TBScore
      , @Set5WinScore
      , @Set5LoseScore
      , @Set5TBScore
      , @WinTotalServe
      , @Win1stIn
      , @Win1stWon
      , @Win2ndWon
      , @LoseTotalServe
      , @Lose1stIn
      , @Lose1stWon
      , @Lose2ndWon)";

                List<String> urlList = new List<string>();
                List<int> tourCode1List = new List<int>();
                List<int> yearList = new List<int>();
                using (SqlCommand cmdSearch = new SqlCommand(selectURL, conn))
                {
                    SqlDataReader reader = cmdSearch.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tourCode1List.Add(reader.GetInt32(0));
                            yearList.Add(reader.GetInt32(1));
                            urlList.Add(reader.GetString(2));
                        }
                    }
                    reader.Close();
                }

                for(int i=0; i<urlList.Count; i++)
                {
                    List<PlayersResult> resultList = WebFunction.GetATPPlayersResult(urlList[i]);
                    foreach (PlayersResult r in resultList)
                    {
                        try {
                            ATPResult ar = WebFunction.GetATPResult(r.MatchURL);
                            using (SqlCommand cmdInsert = new SqlCommand(insertATPTour, conn))
                            {
                                #region setup parameters
                                cmdInsert.Parameters.Add(new SqlParameter("TourCode1", tourCode1List[i]));
                                cmdInsert.Parameters.Add(new SqlParameter("Year", yearList[i]));
                                cmdInsert.Parameters.Add(new SqlParameter("WinnerCode2", r.WinnerCode2));
                                cmdInsert.Parameters.Add(new SqlParameter("LoserCode2", r.LoserCode2));
                                if (ar.Set1WinScore != -1) {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set1WinScore", ar.Set1WinScore));
                                } else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set1WinScore", DBNull.Value));
                                }
                                if (ar.Set1LoseScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set1LoseScore", ar.Set1LoseScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set1LoseScore", DBNull.Value));
                                }
                                if (ar.Set1TBScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set1TBScore", ar.Set1TBScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set1TBScore", DBNull.Value));
                                }
                                if (ar.Set2WinScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set2WinScore", ar.Set2WinScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set2WinScore", DBNull.Value));
                                }
                                if (ar.Set2LoseScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set2LoseScore", ar.Set2LoseScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set2LoseScore", DBNull.Value));
                                }
                                if (ar.Set2TBScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set2TBScore", ar.Set2TBScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set2TBScore", DBNull.Value));
                                }
                                if (ar.Set3WinScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set3WinScore", ar.Set3WinScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set3WinScore", DBNull.Value));
                                }
                                if (ar.Set3LoseScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set3LoseScore", ar.Set3LoseScore));
                                } else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set3LoseScore", DBNull.Value));
                                }
                                if (ar.Set3TBScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set3TBScore", ar.Set3TBScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set3TBScore", DBNull.Value));
                                }
                                if (ar.Set4WinScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set4WinScore", ar.Set4WinScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set4WinScore", DBNull.Value));
                                }
                                if (ar.Set4LoseScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set4LoseScore", ar.Set4LoseScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set4LoseScore", DBNull.Value));
                                }
                                if (ar.Set4TBScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set4TBScore", ar.Set4TBScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set4TBScore", DBNull.Value));
                                }
                                if (ar.Set5WinScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set5WinScore", ar.Set5WinScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set5WinScore", DBNull.Value));
                                }
                                if (ar.Set5LoseScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set5LoseScore", ar.Set5LoseScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set5LoseScore", DBNull.Value));
                                }
                                if (ar.Set5TBScore != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set5TBScore", ar.Set5TBScore));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Set5TBScore", DBNull.Value));
                                }
                                if (ar.WinTotalServe != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("WinTotalServe", ar.WinTotalServe));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("WinTotalServe", DBNull.Value));
                                }
                                if (ar.Win1stIn != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Win1stIn", ar.Win1stIn));
                                } 
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Win1stIn", DBNull.Value));
                                }
                                if (ar.Win1stWon != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Win1stWon", ar.Win1stWon));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Win1stWon", DBNull.Value));
                                }
                                if (ar.Win2ndWon != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Win2ndWon", ar.Win2ndWon));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Win2ndWon", DBNull.Value));
                                }
                                if (ar.LoseTotalServe != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("LoseTotalServe", ar.LoseTotalServe));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("LoseTotalServe", DBNull.Value));
                                }
                                if (ar.Lose1stIn != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Lose1stIn", ar.Lose1stIn));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Lose1stIn", DBNull.Value));
                                }
                                if (ar.Lose1stWon != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Lose1stWon", ar.Lose1stWon));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Lose1stWon", DBNull.Value));
                                }
                                if (ar.Lose2ndWon != -1)
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Lose2ndWon", ar.Lose2ndWon));
                                }
                                else
                                {
                                    cmdInsert.Parameters.Add(new SqlParameter("Lose2ndWon", DBNull.Value));
                                }
                                #endregion
                                cmdInsert.ExecuteNonQuery();
                            }
                        }catch (Exception e) { }
                    }
                }
            }
        }
    }
}
