using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2Craft.Shared
{
    public class StringTbl
    {
        [Index(0)]
        public string Name { get; set; }
        [Index(1)]
        public string Content { get; set; }
    }
}
