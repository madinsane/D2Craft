using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace D2Forge.Shared
{
    //This file is used for quick lookup for items on the crafted items page
    public class ItemShort
    {
        [Name("NameStr")]
        public string NameStr { get; set; }
        [Name("qlvl")]
        public int Qlvl { get; set; }
        [Name("mlvl")]
        public int? Mlvl { get; set; }
        [Ignore]
        public string FullName { get; set; }
    }
}
