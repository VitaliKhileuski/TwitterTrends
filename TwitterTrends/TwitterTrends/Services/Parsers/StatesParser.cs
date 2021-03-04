using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using TwitterTrends.Models;
using TwitterTrends.States;
using TwitterTrends.Models.Parsers;

namespace TwitterTrends.Services.Parsers
{
    public static class StatesParser
    {
        public static Country Parse(string path)
        {
            List<State> states = new List<State>();
            List<Polygon> polygons = new List<Polygon>();
            State state = new State();
            Polygon polygon = new Polygon();

            bool flag = false;
            string temp = string.Empty;
            string jsonString = File.ReadAllText(path);

            //string jsonString = "[[[[]]]]";
            jsonString =DeleteAnotherBrackets(DeleteUselessBrackets(jsonString));

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
    }
}
