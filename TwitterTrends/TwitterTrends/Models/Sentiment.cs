using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterTrends.Models
{
    public class Sentiment
    {
        private string text;
        private double value;

        public string Text { get { return text; } set { text = value; } }
        public int NumberOfWords 
        { 
            get 
            {
                int numOfW = 1;
                foreach (char c in text)
                {
                    if (c == ' ') numOfW++;
                }
                return numOfW;
            } 
        }
        public double Value { get { return value;  } set { this.value = value; } }

        public Sentiment()
        {

        }
        public Sentiment(string text, double value)
        {
            Text = text;
            Value = value;
        }
    }
}
