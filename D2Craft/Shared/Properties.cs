using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2Craft.Shared
{
    public class Properties
    {
        [Name("code")]
        public string Code { get; set; }
        [Name("stat1")]
        public string Stat1 { get; set; }
        [Name("stat2")]
        public string Stat2 { get; set; }
        [Name("stat3")]
        public string Stat3 { get; set; }
        [Name("stat4")]
        public string Stat4 { get; set; }
        [Name("stat5")]
        public string Stat5 { get; set; }
        [Name("stat6")]
        public string Stat6 { get; set; }
        [Name("stat7")]
        public string Stat7 { get; set; }
        [Ignore]
        public List<string> Stats { get; set; }
    }
}
