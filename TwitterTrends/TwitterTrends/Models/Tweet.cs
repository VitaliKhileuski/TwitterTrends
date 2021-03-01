using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTrends.Models
{
    public class Tweet
    {
        private Point pointOnMap;
        private string tweetMessage;
        private DateTime publicationDate;
        private double moodWeight;
        private int stateId;

        public Point PointOnMap { get { return pointOnMap; } set { pointOnMap = value; } }
        public string TweetMessage { get { return tweetMessage; } set { tweetMessage = value; } }
        public DateTime PublicationDate { get { return publicationDate; } set { publicationDate = value; } }

        public double MoodWeight { get { return moodWeight; } set { moodWeight = value; } }
        public int StateId { get { return stateId; } set { stateId = value; } }

        public Tweet()
        {

        }
        public Tweet(Point pointOnMap,string tweetMessage, DateTime publicationDate,double moodWeight)
        {
            PointOnMap = pointOnMap;
            TweetMessage = tweetMessage;
            PublicationDate = publicationDate;
            MoodWeight = moodWeight;
        }

    }
}
