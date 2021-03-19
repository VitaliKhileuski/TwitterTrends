﻿using System;
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

        private List<Tweet> tweets;
        private Dictionary<char, List<Sentiment>> sentiments;
        private Country country;
        
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
        public Dictionary<char, List<Sentiment>> Sentiments { get { return sentiments; } }

        public Country Country { get { return country; } }

        private Database()
        {
            sentiments = SentimentParser.Parse(@"..\..\..\Data\Sentiments\sentiments.csv");
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
                country = StatesParser.GroupTweetsByStates(tweets, @"..\..\..\Data\States\states.json");
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
