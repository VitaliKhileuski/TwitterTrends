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
            StatesParser.Parse(@"C:\Users\Vitali Khileuski\source\repos\TwitterTrends\TwitterTrends\TwitterTrends\Data\States\states.json");
        }

    }
}
