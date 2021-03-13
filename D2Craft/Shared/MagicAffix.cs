using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2Craft.Shared
{
    public class MagicAffix
    {
        [Name("Name")]
        public string Name { get; set; }
        [Name("spawnable")]
        public bool? Spawnable { get; set; }
        [Name("rare")]
        public bool? Rare { get; set; }
        [Name("level")]
        public int? Level { get; set; }
        [Name("maxlevel")]
        public int? MaxLevel { get; set; }
        [Name("levelreq")]
        public int? LevelReq { get; set; }
        [Name("classspecific")]
        public string ClassSpecific { get; set; }
        [Name("class")]
        public string Class { get; set; }
        [Name("classlevelreq")]
        public int? ClassLevelReq { get; set; }
        [Name("frequency")]
        public int? Frequency { get; set; }
        [Name("group")]
        public int? Group { get; set; }
        [Name("mod1code")]
        public string Mod1Code { get; set; }
        [Name("mod1param")]
        public string Mod1Param { get; set; }
        [Name("mod1min")]
        public int? Mod1Min { get; set; }
        [Name("mod1max")]
        public int? Mod1Max { get; set; }
        [Name("mod2code")]
        public string Mod2Code { get; set; }
        [Name("mod2param")]
        public string Mod2Param { get; set; }
        [Name("mod2min")]
        public int? Mod2Min { get; set; }
        [Name("mod2max")]
        public int? Mod2Max { get; set; }
        [Name("mod3code")]
        public string Mod3Code { get; set; }
        [Name("mod3param")]
        public string Mod3Param { get; set; }
        [Name("mod3min")]
        public int? Mod3Min { get; set; }
        [Name("mod3max")]
        public int? Mod3Max { get; set; }
        [Name("transform")]
        public int? Transform { get; set; }
        [Name("transformcolor")]
        public string TransformColor { get; set; }
        [Name("itype1")]
        public string IType1 { get; set; }
        [Name("itype2")]
        public string IType2 { get; set; }
        [Name("itype3")]
        public string IType3 { get; set; }
        [Name("itype4")]
        public string IType4 { get; set; }
        [Name("itype5")]
        public string IType5 { get; set; }
        [Name("itype6")]
        public string IType6 { get; set; }
        [Name("itype7")]
        public string IType7 { get; set; }
        [Name("etype1")]
        public string EType1 { get; set; }
        [Name("etype2")]
        public string EType2 { get; set; }
        [Name("etype3")]
        public string EType3 { get; set; }
        [Name("etype4")]
        public string EType4 { get; set; }
        [Name("etype5")]
        public string EType5 { get; set; }
        [Name("divide")]
        public int? Divide { get; set; }
        [Name("multiply")]
        public int? Multiply { get; set; }
        [Name("add")]
        public int? Add { get; set; }
    }
}
