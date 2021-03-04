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

        public State(string name, List<Polygon> polygons)
        {
            Name = name;
            this.polygons = polygons;
        }
    }
}
