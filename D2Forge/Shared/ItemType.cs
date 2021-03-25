using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace D2Forge.Shared
{
    public class ItemType
    {
        [Name("ItemType")]
        public string Type { get; set; }
        [Name("Code")]
        public string Code { get; set; }
        [Name("Equiv1")]
        public string Parent1 { get; set; }
        [Name("Equiv2")]
        public string Parent2 { get; set; }
        [Name("Repair")]
        public bool? Repair { get; set; }
        [Name("Body")]
        public bool? Body { get; set; }
        [Name("BodyLoc1")]
        public string BodyLoc1 { get; set; }
        [Name("BodyLoc2")]
        public string BodyLoc2 { get; set; }
        [Name("Shoots")]
        public string Shoots { get; set; }
        [Name("Quiver")]
        public string Quiver { get; set; }
        [Name("Throwable")]
        public bool? Throwable { get; set; }
        [Name("Reload")]
        public bool? Reload { get; set; }
        [Name("ReEquip")]
        public bool? Reequip { get; set; }
        [Name("AutoStack")]
        public bool? AutoStack { get; set; }
        [Name("Magic")]
        public bool? Magic { get; set; }
        [Name("Rare")]
        public bool? Rare { get; set; }
        [Name("Normal")]
        public bool? Normal { get; set; }
        [Name("Charm")]
        public bool? Charm { get; set; }
        [Name("Gem")]
        public bool? Gem { get; set; }
        [Name("Beltable")]
        public bool? Beltable { get; set; }
        [Name("MaxSock1")]
        public int? MaxSock1 { get; set; }
        [Name("MaxSock25")]
        public int? MaxSock25 { get; set; }
        [Name("MaxSock40")]
        public int? MaxSock40 { get; set; }
        [Name("TreasureClass")]
        public bool? AutoTresureClass { get; set; }
        [Name("Rarity")]
        public int? Rarity { get; set; }
        [Name("StaffMods")]
        public string StaffMods { get; set; }
        [Name("CostFormula")]
        public int? CostFormula { get; set; }
        [Name("Class")]
        public string Class { get; set; }
        [Name("VarInvGfx")]
        public int? VarInvGfx { get; set; }
        [Name("InvGfx1")]
        public string InvGfx1 { get; set; }
        [Name("InvGfx2")]
        public string InvGfx2 { get; set; }
        [Name("InvGfx3")]
        public string InvGfx3 { get; set; }
        [Name("InvGfx4")]
        public string InvGfx4 { get; set; }
        [Name("InvGfx5")]
        public string InvGfx5 { get; set; }
        [Name("InvGfx6")]
        public string InvGfx6 { get; set; }
        [Name("StorePage")]
        public string StorePage { get; set; }
        //*eol comment line so skip
        [Ignore]
        public List<ItemType> Parents { get; set; }
        [Ignore]
        public List<ItemType> Children { get; set; }
    }
}
