using System;
using System.Collections.Generic;
using System.Text;
using TwitterTrends.Data;
using TwitterTrends.Models;
using TwitterTrends.Models.Parsers;

namespace TwitterTrends.ViewModels
{
    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            string temp = Database.GetInstance().Tweets[0].TweetMessage;
        }
    }
}
