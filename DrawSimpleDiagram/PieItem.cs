using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSimpleDiagram
{
    public class PieItem
    {
        public double Count { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }

        public PieItem(string name)
        {
            Name = name;
        }

        public PieItem(string name, double count)
        {
            Name = name;
            Count = count;
        }

        public PieItem(string name, double count, Color color)
        {
            Name = name;
            Count = count;
            Color = color;
        }

        public override string ToString()
        {
            return $"{Name} = {Count}";
        }
    }
}
