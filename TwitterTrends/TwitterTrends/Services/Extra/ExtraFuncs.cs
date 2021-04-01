using System;
using System.Collections.Generic;
using System.Text;
using TwitterTrends.Models;
using TwitterTrends.Services.Parsers;

namespace TwitterTrends.Services.Extra
{
    public static class ExtraFuncs
    {
        public static int GetNumberOfWords(string sentiment)
        {
            int numOfW = 1;
            foreach (char c in sentiment)
            {
                if (c == ' ') numOfW++;
            }
            return numOfW;
        }
        public static bool IsInside(Polygon polygon, Tweet tweet)
        {
            bool flag = false;
            for (int i = 0, j = polygon.Points.Count - 1; i < polygon.Points.Count; j = i++)
            {
                if ((((polygon.Points[i].Y <= tweet.PointOnMap.Y) && (tweet.PointOnMap.Y < polygon.Points[j].Y)) || ((polygon.Points[j].Y <= tweet.PointOnMap.Y)
                    && (tweet.PointOnMap.Y < polygon.Points[i].Y))) &&
                    (((polygon.Points[j].Y - polygon.Points[i].Y) != 0) &&
                    (tweet.PointOnMap.X > ((polygon.Points[j].X - polygon.Points[i].X)
                    * (tweet.PointOnMap.Y - polygon.Points[i].Y) / (polygon.Points[j].Y - polygon.Points[i].Y) + polygon.Points[i].X))))
                {
                    flag = !flag;
                }
            }
            return flag;
        }

        public static Country GroupTweetsByStates(List<Tweet> tweets, string path)
        {
            Country country = StatesParser.Parse(path);
            foreach (var state in country.States)
            {
                foreach (var pol in state.Polygons)
                {
                    foreach (var point in pol.Points)
                    {
                        double buffer = point.X;
                        point.X = point.Y;
                        point.Y = buffer;
                    }
                }
            }
            foreach (var tweet in tweets)
            {
                foreach (var state in country.States)
                {
                    foreach (var polygon in state.Polygons)
                    {
                        if (IsInside(polygon, tweet))
                        {
                            state.Tweets.Add(tweet);
                            state.TotalWeight += tweet.MoodWeight;
                            if (state.isMoodDefined == false) state.isMoodDefined = true;
                        }
                        else continue;
                    }
                }
            }
            return country;
        }
    }
}
