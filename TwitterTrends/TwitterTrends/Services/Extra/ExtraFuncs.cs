using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterTrends.Services.Extra
{
    public static class ExtraFuncs
    {
        public static int GetNumberOfWords(string sentiment)
        {
            int numOfW = 1;
            foreach (char c in sentiment)
            {
                if (c == ' ') numOfW++;
            }
            return numOfW;
        }
    }
}
