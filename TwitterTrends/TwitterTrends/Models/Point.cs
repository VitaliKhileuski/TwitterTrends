using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTrends.Models
{
   public class Point
    {
        private double x;
        private double y;

        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }

       public Point()
        {

        }
       public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
