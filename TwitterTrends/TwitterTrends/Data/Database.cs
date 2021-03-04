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

        private List<Tweet> tweets = null;
        private Dictionary<string, double> sentiments = SentimentParser.Parse(@"..\..\..\Data\Sentiments\sentiments.csv");
        
        public List<Tweet> Tweets
        {
            get 
            {
                if (tweets != null)
                {
                    return tweets;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
        }
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
        public void SetPathTweetFile(string filePath)
        {
            if (Instance != null)
            {
                tweets = TweetParser.Parse(filePath);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
