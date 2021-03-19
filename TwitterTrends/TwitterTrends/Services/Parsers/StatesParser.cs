using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using TwitterTrends.Models;
using TwitterTrends.Models.Parsers;

namespace TwitterTrends.Services.Parsers
{
    public static class StatesParser
    {
        public static Country Parse(string path)
        {
            List<State> states = new List<State>();
            State state = new State();
            Polygon polygon = new Polygon();

            bool flag = false;
            string temp = string.Empty;
            string jsonString = File.ReadAllText(path);

           
            jsonString = DeleteAnotherBrackets(DeleteUselessBrackets(jsonString));

            for (int i = 0; i < jsonString.Length; i++)
            {
                if(temp.Length==0 && jsonString[i] == ']')
                {
                    continue;
                }
                if (jsonString[i] >= 65 && jsonString[i] <= 90)
                {

                    state.Name = jsonString[i].ToString() + jsonString[i + 1];
                    i++;
                }
                if ((jsonString[i] == '[' && jsonString[i + 1] == '[' && jsonString[i + 2] == '[') || flag==true)
                {
                    
                    
                    if (jsonString[i]!= ']')
                    {
                        flag = true;
                        temp += jsonString[i];

                    }
                    else
                    {
                        
                        temp += jsonString[i];
                        polygon.Points.Add(TweetParser.СoordinatesParse(temp));
                        temp = string.Empty;
                        if (jsonString[i + 1] == ']')
                        {
                            state.Polygons.Add(new Polygon(polygon));
                            polygon.Points.Clear();
                            
                            
                        }
                        if (jsonString[i + 2] == ']')
                        {

                            states.Add(new State(state));
                            state.Polygons.Clear();
                            flag = false;
                        }
                    }

                }
            }

            return new Country(states);
        }


        private static string DeleteUselessBrackets(string jsonstring)
        {
            int temp = 0;
            for (int i = 0; i < jsonstring.Length-temp; i++)
            {
                if ((jsonstring[i] == '[' && jsonstring[i + 1] == '[' && jsonstring[i + 2] == '[' && jsonstring[i + 3] == '[')
                    ||(jsonstring[i] == ']' && jsonstring[i + 1] == ']' && jsonstring[i + 2] == ']' && jsonstring[i + 3] == ']'))
                {
                   jsonstring=jsonstring.Remove(i, 1);
                    temp++;
                }

            }
            return jsonstring;
        }
        private static string DeleteAnotherBrackets(string jsonstring)
        {
            int temp = 0;
            for(int i=0; i<jsonstring.Length-temp-5; i++)
            {
                if((jsonstring[i] == '[' && jsonstring[i + 1] == '[' && jsonstring[i + 2] == '[') && jsonstring[i - 2] != ':')
                {
                    jsonstring = jsonstring.Remove(i, 1);
                    temp++;
                }
                if ((jsonstring[i] == ']' && jsonstring[i + 1] == ']' && jsonstring[i + 2] == ']') && jsonstring[i + 5] != '"')
                {
                    jsonstring = jsonstring.Remove(i, 1);
                    temp++;
                }

            }





            return jsonstring;
        }


        public  static bool IsInside(Polygon polygon,Tweet tweet)
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
        public static Country GroupTweetsByStates(List<Tweet>tweets,string path)
        {
           Country country =StatesParser.Parse(path);
            foreach (var state in country.States)
            {
                foreach (var pol in state.Polygons)
                {
                    foreach (var point in pol.Points)
                    {
                        double bufer;
                        bufer = point.X;
                        point.X = point.Y;
                        point.Y = bufer;
                    }
                }
            }
            foreach (var tweet in tweets)
            {
                foreach(var state in country.States)
                {
                    foreach(var polygon in state.Polygons)
                    {
                        if (IsInside(polygon, tweet)) { state.Tweets.Add(tweet); }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            EstimateStatesMood(country);
            return country;
        }

        private static void EstimateStatesMood(Country country)
        {
            
            foreach(var state in country.States)
            {
               
                foreach(var tweet in state.Tweets)
                {
                    state.TotalWeight += tweet.MoodWeight;
                }
                state.TotalWeight = state.TotalWeight / state.Tweets.Count;
            }
        }
    }
}
