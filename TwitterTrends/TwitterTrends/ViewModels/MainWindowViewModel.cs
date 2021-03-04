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
        #region Open file with tweets
        private Command openTweetsFile;
        public bool CanOpenTweetsFileExecuted(object p)
        {
            return true;
        }
        public void OnOpenTweetsFileExecute(object p)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
               Database.GetInstance().SetPathTweetFile(openFileDialog.FileName);
        }
        
        public Command OpenTweetsFile
        {
            get
            {
                return openTweetsFile = new Command(OnOpenTweetsFileExecute, CanOpenTweetsFileExecuted);
            }
        }
        #endregion



        public MainWindowViewModel()
        {
            StatesParser.Parse(@"C:\Users\Vitali Khileuski\source\repos\TwitterTrends\TwitterTrends\TwitterTrends\Data\States\states.json");
        }

    }
}
