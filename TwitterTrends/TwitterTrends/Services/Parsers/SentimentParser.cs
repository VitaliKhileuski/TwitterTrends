using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TwitterTrends.Models;

namespace TwitterTrends.Services.Parsers
{
    public static class SentimentParser
    {
        public static Dictionary<char, List<Sentiment>> Parse(string path)
        {
            Dictionary<char, List<Sentiment>> sentiments = new Dictionary<char, List<Sentiment>>();
            string[] temp = File.ReadAllLines(path);
            foreach(string t in temp)
            {
                bool flag = true;
                foreach(char c in sentiments.Keys)
                {
                    if (t.StartsWith(c))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    KeyValuePair<char, List<Sentiment>> k = ParseByFirstLetter(t[0], temp);
                    sentiments.Add(k.Key, k.Value);
                }
            }
            return sentiments;
        }

        private static KeyValuePair<char, List<Sentiment>> ParseByFirstLetter(char letter, string[] allSentiments)
        {
            List<Sentiment> tempSentiments2 = allSentiments
                .Where(row => row.Length > 0)
                .Where(row => row.StartsWith(letter))
                .Select(SentimentParser.ParseSentiment).ToList<Sentiment>();
            KeyValuePair<char, List<Sentiment>> tempSentiments = new KeyValuePair<char, List<Sentiment>>(key: letter, value: tempSentiments2);
            return tempSentiments;
        }

        private static Sentiment ParseSentiment(string row)
        {
            var columns = row.Split(',');
            columns[1] = columns[1].Replace('.', ',');
            Sentiment sentiment = new Sentiment(columns[0], double.Parse(columns[1]));
            return sentiment;
        }

        private static int GetNumberOfWords(string sentiment)
        {
            int numOfW = 1;
            foreach(char c in sentiment)
            {
                if (c == ' ') numOfW++;
            }
            return numOfW;
        }
    }
}
