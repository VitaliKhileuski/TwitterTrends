using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterTrends.Models
{
    class Country
    {
        private string name;
        public string Name { get => name; set => name = value; }

        List<State> states = new List<State>();
        internal List<State> States { get => states; set => states = value; }
    }
}
