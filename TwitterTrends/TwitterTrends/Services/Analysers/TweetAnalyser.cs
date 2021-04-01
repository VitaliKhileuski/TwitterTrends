using System;
using System.Collections.Generic;
using System.Text;
using TwitterTrends.Models;
using TwitterTrends.Services.Extra;

namespace TwitterTrends.Services.Analysers
{
    public static class TweetAnalyser
    {
        public static double GetWeight(string message, Dictionary<char, List<Sentiment>> sentiments)
        {
            int maxNumberOfWordsInSentiment = 0;
            message = message.Trim();
            string firstWordInPrase = String.Empty;
            List<Sentiment> sentimentsByFirstWord = new List<Sentiment>();
            char[] delimiterChars = { '.', ',', '?', ':', '!', ';' };
            string[] phrases = message.Split(delimiterChars);
            double fullWeigth = 0;
            foreach (var phrase in phrases)
            {
                string copyOfPhrase = phrase.Trim();
                if (phrase == "") { continue; }
                while (copyOfPhrase.Length != 0) //FIX
                {
                    firstWordInPrase = GetFirstWordInPhrase(copyOfPhrase);
                    if (sentiments.ContainsKey(firstWordInPrase[0]))
                    {
                        var first = DateTime.Now;
                        foreach (var sentiment in sentiments[firstWordInPrase[0]])
                        {
                            if ((sentiment.Text + " ").StartsWith(firstWordInPrase + " "))
                            {
                                sentimentsByFirstWord.Add(sentiment);

                                if (ExtraFuncs.GetNumberOfWords(sentiment.Text) > maxNumberOfWordsInSentiment)
                                {
                                    maxNumberOfWordsInSentiment = ExtraFuncs.GetNumberOfWords(sentiment.Text);
                                }
                            }
                        }
                        var second = DateTime.Now;
                        double lol = (second - first).TotalSeconds;
                    }
                    if (sentimentsByFirstWord.Count == 0)
                    {
                        copyOfPhrase = copyOfPhrase.Remove(0, firstWordInPrase.Length).Trim();
                        continue;
                    }
                    string temp = CutPhrase(copyOfPhrase, maxNumberOfWordsInSentiment);
                    fullWeigth += GetWeightOfPartOfPhrase(ref temp, sentimentsByFirstWord, maxNumberOfWordsInSentiment);
                    copyOfPhrase = copyOfPhrase.Remove(0, temp.Length).Trim();
                    sentimentsByFirstWord.Clear();
                }
            }
            return fullWeigth;
        }
        static private string GetFirstWordInPhrase(string phrase)
        {
            string word = String.Empty;
            foreach (char symbol in phrase)
            {
                if (symbol == ' ')
                {
                    return word.ToLower();
                }
                word += symbol;
            }
            return word.ToLower();
        }
        static private string CutPhrase(string phrase, int maxNumberOfWordsInSentiment)
        {
            string cuttedPhrase = phrase;
            int numOfSpaces = 0;
            for (int i = 0; i < phrase.Length; i++)
            {
                if (phrase[i] == ' ')
                {
                    numOfSpaces++;
                }
                if (numOfSpaces == maxNumberOfWordsInSentiment)
                {
                    cuttedPhrase = cuttedPhrase.Substring(0, i);
                    break;
                }
            }
            return cuttedPhrase;
        }
        static private double GetWeightOfPartOfPhrase(ref string part, List<Sentiment> sentiments, int num)
        {
            double weight = 0;
            while (true)
            {
                foreach (Sentiment s in sentiments)
                {
                    if (ExtraFuncs.GetNumberOfWords(s.Text) == num && part.ToLower().Trim() == s.Text)
                    {
                        return s.Value;
                    }
                }
                if (part.LastIndexOf(' ') != -1) { part = part.Substring(0, part.LastIndexOf(' ')); num--; }
                else break;
            }
            return weight;
        }
    }
}
