using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitterTrends.Data;

namespace TwitterTrends.Models.Parsers
{
    public static class TweetParser
    {
        private static Dictionary<char, List<Sentiment>> sentiments=Database.GetInstance().Sentiments;
        public static List<Tweet> Parse(string path)
        {
          
            List<Tweet> tweets = new List<Tweet>();
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != String.Empty)
                    {
                        if (line[0] != '[' || (line[1] <= 49 || line[1] >= 57))
                        {
                            continue;
                        }
                        tweets.Add(TweetParse(line));
                    }


                }
            }

            return tweets;
        }

        private static Tweet TweetParse(string line)
        {
            Tweet tweet = new Tweet();
            tweet.PublicationDate = DateParse(line);
            tweet.PointOnMap = СoordinatesParse(line);
            tweet.TweetMessage = MessageParse(line);
            tweet.MoodWeight = GetWeight(tweet.TweetMessage);
            return tweet;
        }
        private static DateTime DateParse(string line)
        {
            Regex year = new Regex(@"\d{4}-\d{2}-\d{2}");
            Regex time = new Regex(@"\d{2}:\d{2}:\d{2}");
            string result = year.Match(line).Value + " " + time.Match(line).Value + " ";
            int[] date = new int[6];
            string temp = "";
            int count = 0;
            foreach (var symbol in result)
            {
                if (symbol == '-' || symbol == ':' || symbol == ' ')
                {

                    date[count] = int.Parse(temp);
                    temp = String.Empty;
                    count++;
                    continue;
                }
                temp += symbol;

            }
            return new DateTime(date[0], date[1], date[2], date[3], date[4], date[5]);
        }
        public static Point СoordinatesParse(string line)
        {
            double[] Coordinates = new double[2];
            string temp = "";
            int count = 0;
            Regex coordinates = new Regex(@"\[-?\d*\.?\d*\, -?\d*\.?\d*\]");
            string s = coordinates.Match(line).Value + "_";
            foreach (var symbol in s)
            {
                if (symbol != '[' && symbol != ']' && symbol != ' ')
                {
                    if (symbol == ',' || symbol == '_')
                    {

                        Coordinates[count] = double.Parse(temp);
                        temp = String.Empty;
                        count++;
                        continue;
                    }
                    if (symbol == '.') { temp += ","; }
                    else
                    {
                        temp += symbol;
                    }



                }
            }
            return new Point(Coordinates[0], Coordinates[1]);
        }
        private static string MessageParse(string line)
        {
            string tweetMessage = "";
            Regex time = new Regex(@"\d{2}:\d{2}:\d{2}");
            string date = time.Match(line).Value;
            int index = line.IndexOf(date) + date.Length;
            tweetMessage = line.Substring(index, line.Length - index);
            tweetMessage = RemoveUsellesWords(tweetMessage);

            return tweetMessage;
        }
        private static string RemoveUsellesWords(string line)
        {
            Regex link = new Regex(@"http://\S*");
            Regex hashtag = new Regex(@"#\S*");
            Regex referenceToPerson = new Regex(@"@\S*");
            line = link.Replace(line, String.Empty);
            line = hashtag.Replace(line, String.Empty);
            line = referenceToPerson.Replace(line, String.Empty);
            line = line.Replace("\t", String.Empty);

            int wordLength = 1;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '@' || line[i] == '#')
                {
                    for (int j = i; j < line.Length; j++)
                    {
                        if (line[j] == ' ')
                        {
                            line.Remove(i, wordLength);
                            wordLength = 1;
                        }
                        else
                        {
                            wordLength++;
                        }
                    }
                }
            }


            return line;
        }
        public static double GetWeight(string message)
        {
            int maxNumberOfWordsInSentiment = 0;
            message = message.Trim();
            string firstWordInPrase = String.Empty;
            List<Sentiment> sentimentsByFirstWord = new List<Sentiment>();
            char[] delimiterChars = {'.', ',', '?', ':' , '!' , ';' };
            string[] phrases = message.Split(delimiterChars);
            double fullWeigth = 0;
            foreach(var phrase in phrases)
            {
                string copyOfPhrase = phrase.Trim();
                if (phrase == ""){continue;}
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

                                if (sentiment.NumberOfWords > maxNumberOfWordsInSentiment)
                                {
                                    maxNumberOfWordsInSentiment = sentiment.NumberOfWords;
                                }
                            }
                        }
                        var second = DateTime.Now;
                        double lol = (second - first).TotalSeconds;
                    }
                    if(sentimentsByFirstWord.Count == 0)
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
            foreach(char symbol in phrase)
            {
                if(symbol==' ')
                {
                    return word.ToLower();
                }
                word += symbol;
            }
            return word.ToLower();
        }
        static private string DeleteLastWordInPhrase(string phrase)
        {
            string newPhrase = String.Empty;

            return newPhrase;
        }
        static private string CutPhrase(string phrase, int maxNumberOfWordsInSentiment)
        {
            string cuttedPhrase = phrase;
            int numOfSpaces = 0;
            for(int i = 0; i < phrase.Length; i++)
            {
                if(phrase[i] == ' ')
                {
                    numOfSpaces++;
                }
                if(numOfSpaces == maxNumberOfWordsInSentiment)
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
                foreach(Sentiment s in sentiments)
                {
                    if(s.NumberOfWords == num && part.ToLower().Trim() == s.Text)
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

