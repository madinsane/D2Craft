using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2Craft.Shared
{
    public class Mod
    {
        public string Name { get; set; }
        public string Param { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string FullMod { get; set; }

        public Mod(string name, string param, int min, int max)
        {
            Name = name;
            Param = param;
            Min = min;
            Max = max;
            FullMod = "";
        }
    }
}
