using System;
using System.Collections.Generic;
using System.Text;
using TwitterTrends.Models;
using TwitterTrends.Models.Parsers;
using TwitterTrends.Services.Parsers;

namespace TwitterTrends.Data
{
    class Database
    {
        private static Database Instance = null;

        private List<Tweet> tweets = TweetParser.Parse(@"C:\Users\mashk\source\repos\TwitterTrends\TwitterTrends\TwitterTrends\Data\Tweets\cali_tweets2014.txt");
        private Dictionary<string, double> sentiments = SentimentParser.Parse(@"C:\Users\mashk\source\repos\TwitterTrends\TwitterTrends\TwitterTrends\Data\Sentiments\sentiments.csv");

        public List<Tweet> Tweets { get { return tweets; } }
        public Dictionary<string, double> Sentiments { get { return sentiments; } }

        private Database()
        {

        }

        public static Database GetInstance()
        {
            if(Instance == null)
            {
                Instance = new Database();
            }
            return Instance;
        }
    }
}
