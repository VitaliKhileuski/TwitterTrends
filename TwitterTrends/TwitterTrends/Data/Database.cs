using System;
using System.Collections.Generic;
using System.Text;
using TwitterTrends.Models;
using TwitterTrends.Models.Parsers;

namespace TwitterTrends.Data
{
    class Database
    {
        private static Database Instance = null;

        private List<Tweet> tweets = TweetParser.Parse(@"C:\Users\mashk\source\repos\TwitterTrends\TwitterTrends\TwitterTrends\Data\Tweets\cali_tweets2014.txt");

        public List<Tweet> Tweets { get { return tweets; } }

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
