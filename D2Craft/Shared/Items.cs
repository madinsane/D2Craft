using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace D2Craft.Shared
{
    public class Items
    {
        [Name("NameStr")]
        public string NameStr { get; set; }
        [Name("qlvl")]
        public int Qlvl { get; set; }
        [Ignore]
        public string FullName { get; set; }
    }
}
