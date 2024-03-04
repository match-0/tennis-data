using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisData
{
    public class RankHt
    {
        public DateTime Date;
        public string Name;
        public Int16 Rank;
        public int Points;
        public Int16 Age;
        public Int16 TourPlayed;
        public int PointsDropping;
        public int NextBest;

        public RankHt(DateTime date, string name, Int16 rank, int points, Int16 age, Int16 tourplayed, int pointsDropping, int nextBest)
        {
            Date = date;
            Name = name;
            Rank = rank;
            Points = points;
            Age = age;
            TourPlayed = tourplayed;
            PointsDropping = pointsDropping;
            NextBest = nextBest;
        }
        public RankHt() { }
    }
}
