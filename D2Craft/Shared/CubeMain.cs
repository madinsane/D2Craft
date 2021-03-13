using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace D2Craft.Shared
{
    public class CubeMain
    {
        [Name("description")]
        public string Description { get; set; }
        [Name("input 1")]
        public string Input1 { get; set; }
        [Name("input 2")]
        public string Input2 { get; set; }
        [Name("input 3")]
        public string Input3 { get; set; }
        [Name("input 4")]
        public string Input4 { get; set; }
        [Name("plvl")]
        public int Plvl { get; set; }
        [Name("ilvl")]
        public int Ilvl { get; set; }
        [Name("mod 1")]
        public string Mod1 { get; set; }
        [Name("mod 1 param")]
        public string Mod1Param { get; set; }
        [Name("mod 1 min")]
        public int? Mod1Min { get; set; }
        [Name("mod 1 max")]
        public int? Mod1Max { get; set; }
        [Name("mod 2")]
        public string Mod2 { get; set; }
        [Name("mod 2 param")]
        public string Mod2Param { get; set; }
        [Name("mod 2 min")]
        public int? Mod2Min { get; set; }
        [Name("mod 2 max")]
        public int? Mod2Max { get; set; }
        [Name("mod 3")]
        public string Mod3 { get; set; }
        [Name("mod 3 param")]
        public string Mod3Param { get; set; }
        [Name("mod 3 min")]
        public int? Mod3Min { get; set; }
        [Name("mod 3 max")]
        public int? Mod3Max { get; set; }
        [Name("mod 4")]
        public string Mod4 { get; set; }
        [Name("mod 4 param")]
        public string Mod4Param { get; set; }
        [Name("mod 4 min")]
        public int? Mod4Min { get; set; }
        [Name("mod 4 max")]
        public int? Mod4Max { get; set; }
        [Ignore]
        public string ItemType { get; set; }
        [Ignore]
        public string[] InputTypes { get; set; }
        [Ignore]
        public string[] InputNames { get; set; }
        [Ignore]
        public Mod[] Mods { get; set; }
    }
}
