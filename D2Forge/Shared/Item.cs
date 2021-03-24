using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace D2Forge.Shared
{
    public class Item
    {
        [Name("name")]
        public string Name { get; set; }
        //*name is comment field
        //szFlavorText is unused
        [Name("version")]
        public int? Version { get; set; }
        [Name("compactsave")]
        public bool? CompactSave { get; set; }
        [Name("rarity")]
        public int? Rarity { get; set; }
        [Name("spawnable")]
        public bool? Spawnable { get; set; }
        [Name("minac")]
        public int? MinAc { get; set; }
        [Name("maxac")]
        public int? MaxAc { get; set; }
        //Absorb is unused so skip
        [Name("speed")]
        public int? Speed { get; set; }
        [Name("reqstr")]
        public int? ReqStr { get; set; }
        [Name("block")]
        public int? Block { get; set; }
        [Name("durability")]
        public int? Durability { get; set; }
        [Name("nodurability")]
        public bool? NoDurability { get; set; }
        [Name("indestructible")]
        public bool? Indestructible { get; set; }
        [Name("durwarning")]
        public int? DurWarning { get; set; }
        [Name("level")]
        public int? Level { get; set; }
        [Name("levelreq")]
        public int? LevelReq { get; set; }
        [Name("cost")]
        public int? Cost { get; set; }
        [Name("gamble cost")]
        public int? GambleCost { get; set; }
        [Name("code")]
        public string Code { get; set; }
        [Name("namestr")]
        public string NameStr { get; set; }
        [Name("magic lvl")]
        public int? MagicLvl { get; set; }
        [Name("auto prefix")]
        public int? AutoPrefix { get; set; }
        [Name("alternategfx")]
        public string AlternateGfx { get; set; }
        //OpenBetaGfx is unused so skip
        [Name("normcode")]
        public string NormCode { get; set; }
        [Name("ubercode")]
        public string UberCode { get; set; }
        [Name("ultracode")]
        public string UltraCode { get; set; }
        //spelloffset is unused/unknown so skip
        //component - rArm - lArm - Torso - Legs - rSPad - lSPad are anim related so not relevant
        [Name("invwidth")]
        public int? InvWidth { get; set; }
        [Name("invheight")]
        public int? InvHeight { get; set; }
        [Name("hasinv")]
        public bool? HasInv { get; set; }
        [Name("gemsockets")]
        public int? GemSockets { get; set; }
        [Name("gemapplytype")]
        public int? GemApplyType { get; set; }
        //flippyfile is used for drop image not relevant
        [Name("invfile")]
        public string InvFile { get; set; }
        [Name("uniqueinvfile")]
        public string UniqueInvFile { get; set; }
        [Name("setinvfile")]
        public string SetInvFile { get; set; }
        //Transmogrify - TMogType - TMogMin - TMogMax are unused in vanilla
        [Name("useable")]
        public bool? Useable { get; set; }
        [Name("throwable")]
        public bool? Throwable { get; set; }
        [Name("missiletype")]
        public string MissileType { get; set; }
        [Name("stackable")]
        public bool? Stackable { get; set; }
        [Name("minstack")]
        public int? MinStack { get; set; }
        [Name("maxstack")]
        public int? MaxStack { get; set; }
        [Name("spawnstack")]
        public int? SpawnStack { get; set; }
        [Name("qntwarning")]
        public int? QuantWarning { get; set; }
        [Name("type")]
        public string Type1 { get; set; }
        [Name("type2")]
        public string Type2 { get; set; }
        //dropsound - dropsfxframe are only used for drop sfx not relevant
        [Name("usesound")]
        public string UseSound { get; set; }
        [Name("quest")]
        public int? Quest { get; set; }
        [Name("questdiffcheck")]
        public bool? QuestDiffCheck { get; set; }
        [Name("unique")]
        public bool? Unique { get; set; }
        //transparent - transtbl are unknown/unused
        [Name("quivered")]
        public int? Quivered { get; set; }
        //lightradius is unused
        //Has multiple meanings based on which item type it is
        [Name("belt")]
        public int? Belt { get; set; }
        [Name("autobelt")]
        public bool? AutoBelt { get; set; }
        //Appears twice in armor.txt, possible issue
        [Name("mindam")]
        public int? MinDam { get; set; }
        [Name("maxdam")]
        public int? MaxDam { get; set; }
        [Name("StrBonus")]
        public int? StrBonus { get; set; }
        [Name("DexBonus")]
        public int? DexBonus { get; set; }
        [Name("gemoffset")]
        public int? GemOffset { get; set; }
        //Only notable use is for material
        [Name("bitfield1")]
        public int? Material { get; set; }
        //Vendor columns not really relevant
        //Source Art - Game Art not used
        [Name("InvTrans")]
        public int? InvTrans { get; set; }
        //SkipName is only used to hide information so not relevant
        //NightmareUpgrade and HellUpgrade are for vendors
        //Nameable is just for personalise so not relevant

        //Weapon exclusives
        [Name("1or2handed")]
        public bool? OneOrTwoHanded { get; set; }
        [Name("2handed")]
        public bool? TwoHanded { get; set; }
        [Name("2handmindam")]
        public int? TwoHandMinDam { get; set; }
        [Name("2handmaxdam")]
        public int? TwoHandMaxDam { get; set; }
        [Name("minmisdam")]
        public int? MissMinDam { get; set; }
        [Name("maxmisdam")]
        public int? MissMaxDam { get; set; }
        //Headerless column?
        [Name("rangeadder")]
        public int? RangeAdder { get; set; }
        [Name("reqdex")]
        public int? ReqDex { get; set; }
        [Name("wclass")]
        public string WeaponClass { get; set; }
        [Name("2handedwclass")]
        public string TwoHandWeaponClass { get; set; }
        //Hit class is for sfx on hit so not relevant
        //Special is comment field

        //Misc exclusives
        [Name("pSpell")]
        public int? PSpell { get; set; }
        [Name("state")]
        public string State { get; set; }
        [Name("cstate1")]
        public string CState1 { get; set; }
        [Name("cstate2")]
        public string CState2 { get; set; }
        [Name("len")]
        public int? Length { get; set; }
        [Name("stat1")]
        public string Stat1 { get; set; }
        [Name("calc1")]
        public int? Calc1 { get; set; }
        [Name("stat2")]
        public string Stat2 { get; set; }
        [Name("calc2")]
        public int? Calc2 { get; set; }
        [Name("stat3")]
        public string Stat3 { get; set; }
        [Name("calc3")]
        public int? Calc3 { get; set; }
        [Name("spelldesc")]
        public int? SpellDesc { get; set; }
        [Name("spelldescstr")]
        public string SpellDescStr { get; set; }
        [Name("spelldesccalc")]
        public string SpellDescCalc { get; set; }
        [Name("BetterGem")]
        public string BetterGem { get; set; }
    }
}
