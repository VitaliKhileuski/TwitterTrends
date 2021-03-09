using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterTrends.Models
{
    public class Sentiment
    {
        private string text;
        private int numberOfWords;
        private double value;

        public string Text { get { return text; } set { text = value; } }
        public int NumberOfWords { get { return numberOfWords; } set { numberOfWords = value; } }
        private double Value { get { return value;  } set { this.value = value; } }

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
