using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterTrends.Models
{
    public class State
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        List<Polygon> polygons = new List<Polygon>();

        internal List<Polygon> Polygons { get => polygons; set => polygons = value; }
        private List<Tweet> tweets = new List<Tweet>();
        public List<Tweet> Tweets { get => tweets; set => tweets = value; }
        private double totalWeight;
        public double TotalWeight { get => totalWeight; set => totalWeight = value; }
        public bool isMoodDefined = false;
        public State()
        {

        }
        public State(string name, List<Polygon> polygons)
        {
            Name = name;
            this.polygons = polygons;
            TotalWeight = 0;
            Tweets = new List<Tweet>();
        }
        public State(State state)
        {
            name = state.Name;
            polygons =new List<Polygon>(state.Polygons);
            totalWeight = state.TotalWeight;
            tweets = new List<Tweet>(state.Tweets);
        }
    }
}
