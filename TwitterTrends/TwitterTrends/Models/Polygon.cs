using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterTrends.Models
{
    public class Polygon
    {
        List<Point> points = new List<Point>();

        public List<Point> Points { get => points; set => points = value; }

        public Polygon() { }

        public Polygon(Polygon polygon)
        {
            points =new List<Point>(polygon.Points);
        }
    }
}
