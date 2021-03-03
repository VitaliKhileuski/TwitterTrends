using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TwitterTrends.Services.Parsers
{
    public static class SentimentParser
    {
        public static Dictionary<string, double> Parse(string path)
        {
            Dictionary<string, double> sentiments = File.ReadAllLines(path)
                .Where(row => row.Length > 0)
                .Select(SentimentParser.ParseSentiment).ToDictionary(k => k.Key, v => v.Value);
            return sentiments;
        }

        private static KeyValuePair<string, double> ParseSentiment(string row)
        {
            var columns = row.Split(',');
            columns[1] = columns[1].Replace('.', ',');
            KeyValuePair<string, double> sentiment = new KeyValuePair<string, double>(columns[0], double.Parse(columns[1]));  //FIX
            return sentiment;
        }
    }
}
