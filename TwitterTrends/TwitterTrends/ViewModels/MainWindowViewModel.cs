using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using TwitterTrends.Commands;
using TwitterTrends.Data;
using TwitterTrends.Models;
using TwitterTrends.Models.Parsers;
using TwitterTrends.Services.Parsers;

namespace TwitterTrends.ViewModels
{
    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            //StatesParser.Parse(@"C:\Users\Vitali Khileuski\source\repos\TwitterTrends\TwitterTrends\TwitterTrends\Data\States\states.json");
            var first = DateTime.Now;
            //Dictionary<char, List<Sentiment>> keyValuePairs = SentimentParser.Parse(@"C:\Users\Vitali Khileuski\source\repos\TwitterTrends\TwitterTrends\TwitterTrends\Data\Sentiments\sentiments.csv");


            //Database.GetInstance().SetPathTweetFile(@"C:\Users\Vitali Khileuski\source\repos\TwitterTrends\TwitterTrends\TwitterTrends\Data\Tweets\weekend_tweets2014.txt");
            List<Tweet> tweets = TweetParser.Parse(@"C:..\..\..\Data\Tweets\weekend_tweets2014.txt");
            TweetParser.GetWeight("yellow spot fungus");
            var second = DateTime.Now;
            double lol = (second - first).TotalSeconds;
        }

    }
}
