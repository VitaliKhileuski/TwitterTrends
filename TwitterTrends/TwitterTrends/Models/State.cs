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

        public State()
        {

        }
        public State(string name, List<Polygon> polygons)
        {
            Name = name;
            this.polygons = polygons;
        }
        public State(State state)
        {
            name = state.Name;
            polygons =new List<Polygon>(state.Polygons);
        }
    }
}
