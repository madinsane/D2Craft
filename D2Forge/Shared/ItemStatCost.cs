using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2Forge.Shared
{
    public class ItemStatCost
    {
        [Name("Stat")]
        public string Stat { get; set; }
        [Name("ID")]
        public int Id { get; set; }
        [Name("descpriority")]
        public int? DescPriority { get; set; }
        [Name("descfunc")]
        public int? DescFunc { get; set; }
        [Name("descval")]
        public int? DescVal { get; set; }
        [Name("descstrpos")]
        public string DescStrPos { get; set; }
        [Name("descstrneg")]
        public string DescStrNeg { get; set; }
        [Name("descstr2")]
        public string DescStr2 { get; set; }
        [Name("dgrp")]
        public int? DescGroup { get; set; }
        [Name("dgrpfunc")]
        public int? DescGroupFunc { get; set; }
        [Name("dgrpval")]
        public int? DescGroupVal { get; set; }
        [Name("dgrpstrpos")]
        public string DescGroupStrPos { get; set; }
        [Name("dgrpstrneg")]
        public string DescGroupStrNeg { get; set; }
        [Name("dgrpstr2")]
        public string DescGroupStr2 { get; set; }

    }
}
