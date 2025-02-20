﻿using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static D2Forge.Shared.Constants;

namespace D2Forge.Shared
{
    public class DataManager
    {

        public List<CubeMain> Recipes { get; set; }
        public Dictionary<string, ItemStatCost> ISC { get; set; }
        public Dictionary<string, ItemStatCost> ISCGroups { get; set; }
        public Dictionary<string, Properties> Properties { get; set; }
        public List<MagicAffix> Prefixes { get; set; }
        public List<MagicAffix> Suffixes { get; set; }
        public List<ItemShort> ItemAndTypeList { get; set; }
        public Dictionary<string, Item> Items { get; set; }
        public Dictionary<string, string> Strings { get; set; }
        public Dictionary<string, ItemShort> ItemsShort { get; set; }
        public Dictionary<string, ItemType> ItemTypes { get; set; }
        public Dictionary<string, string> TypeMap { get; set; }
        public Dictionary<int, string> ClassMap { get; set; }
        public Dictionary<int, string> SkillTabMap { get; set; }
        public List<string> PropOverrideList { get; set; }
        public StateContainer StateContainer { get; set; }
        public bool Loaded { get; set; }
        public CultureInfo Culture { get; set; }

        public DataManager(StateContainer stateContainer)
        {
            Loaded = false;
            StateContainer = stateContainer;
            Culture = CultureInfo.InvariantCulture;
            Recipes = ReadCsvToList<CubeMain>(stateContainer.DataFiles[DataFileTypes.CubeMain]);
            ISC = ReadCsvToDictISC(stateContainer.DataFiles[DataFileTypes.ItemStatCost]);
            Properties = ReadCsvToDictProp(stateContainer.DataFiles[DataFileTypes.Properties]);
            CompilePropertyStats();
            Prefixes = ReadCsvToListAffix(stateContainer.DataFiles[DataFileTypes.MagicPrefix]);
            Suffixes = ReadCsvToListAffix(stateContainer.DataFiles[DataFileTypes.MagicSuffix]);
            Items = new Dictionary<string, Item>();
            ReadItems(stateContainer.DataFiles[DataFileTypes.Armor]);
            ReadItems(stateContainer.DataFiles[DataFileTypes.Weapons]);
            ReadItems(stateContainer.DataFiles[DataFileTypes.Misc]);
            ItemsShort = ReadCsvToDictItemsShort(stateContainer.DataFiles[DataFileTypes.ItemShort]);
            ItemTypes = ReadCsvToDictItemType(stateContainer.DataFiles[DataFileTypes.ItemTypes]);
            BuildItemTypeTree();
            Strings = new Dictionary<string, string>();
            ReadStrings(stateContainer.DataFiles[DataFileTypes.PatchStrings]);
            ReadStrings(stateContainer.DataFiles[DataFileTypes.ExpStrings]);
            ReadStrings(stateContainer.DataFiles[DataFileTypes.Strings]);
            ConvertRecipes();
            GetItemStrings();
            CheckPropGroups();
            SetupAffixItemTypes(Prefixes);
            SetupAffixItemTypes(Suffixes);
            PrepareAffixMods(Prefixes);
            PrepareAffixMods(Suffixes);
            InitItemsAndTypesList();
            Loaded = true;
        }

        public static List<T> ReadCsvToList<T>(string file, bool hasHeader = true)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeader,
                Delimiter = "\t"
            };
            byte[] byteArray = Encoding.ASCII.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<T>();
            return records.ToList();
        }

        public static List<MagicAffix> ReadCsvToListAffix(string file, bool hasHeader = true)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeader,
                Delimiter = "\t"
            };
            byte[] byteArray = Encoding.ASCII.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<MagicAffix>();
            var list = new List<MagicAffix>();
            foreach (var record in records)
            {
                if (!string.IsNullOrEmpty(record.Name) && record.Spawnable.GetValueOrDefault() && record.Frequency.GetValueOrDefault() != 0)
                {
                    list.Add(record);
                }
            }
            return list;
        }

        public static Dictionary<string, ItemStatCost> ReadCsvToDictISC(string file, bool hasHeader = true)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeader,
                Delimiter = "\t"
            };
            byte[] byteArray = Encoding.ASCII.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<ItemStatCost>();
            Dictionary<string, ItemStatCost> dict = new Dictionary<string, ItemStatCost>();
            foreach (var record in records)
            {
                if (!dict.ContainsKey(record.Stat) && !string.IsNullOrEmpty(record.Stat))
                {
                    dict.Add(record.Stat, record);
                }
            }
            return dict;
        }

        public static Dictionary<string, ItemType> ReadCsvToDictItemType(string file, bool hasHeader = true)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeader,
                Delimiter = "\t",
                HeaderValidated = null
            };
            byte[] byteArray = Encoding.ASCII.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<ItemType>();
            Dictionary<string, ItemType> dict = new Dictionary<string, ItemType>();
            foreach (var record in records)
            {
                if (!dict.ContainsKey(record.Code) && !string.IsNullOrEmpty(record.Code))
                {
                    dict.Add(record.Code, record);
                }
            }
            return dict;
        }

        public void ReadItems(string file, bool hasHeader = true)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeader,
                Delimiter = "\t",
                HeaderValidated = null,
                MissingFieldFound = null
            };
            byte[] byteArray = Encoding.ASCII.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Item>();
            foreach (var record in records)
            {
                if (!Items.ContainsKey(record.Code) && !string.IsNullOrEmpty(record.Code))
                {
                    Items.Add(record.Code, record);
                }
            }
        }

        public static Dictionary<string, Properties> ReadCsvToDictProp(string file, bool hasHeader = true)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeader,
                Delimiter = "\t"
            };
            byte[] byteArray = Encoding.ASCII.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Properties>();
            Dictionary<string, Properties> dict = new Dictionary<string, Properties>();
            foreach (var record in records)
            {
                if (!dict.ContainsKey(record.Code) && !string.IsNullOrEmpty(record.Code))
                {
                    dict.Add(record.Code, record);
                }
            }
            return dict;
        }

        public static Dictionary<string, ItemShort> ReadCsvToDictItemsShort(string file, bool hasHeader = true)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeader,
                Delimiter = "\t"
            };
            byte[] byteArray = Encoding.ASCII.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<ItemShort>();
            Dictionary<string, ItemShort> dict = new Dictionary<string, ItemShort>();
            foreach (var record in records)
            {
                if (!dict.ContainsKey(record.NameStr) && !string.IsNullOrEmpty(record.NameStr))
                {
                    dict.Add(record.NameStr, record);
                }
            }
            return dict;
        }

        public void ReadStrings(string file)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = "\t"
            };
            byte[] byteArray = Encoding.ASCII.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<StringTbl>();
            foreach (var record in records)
            {
                if (!Strings.ContainsKey(record.Name))
                {
                    Strings.Add(record.Name, record.Content);
                }
            }
        }

        public void GetItemStrings()
        {
            foreach (ItemShort item in ItemsShort.Values)
            {
                if (string.IsNullOrEmpty(item.NameStr))
                {
                    continue;
                }
                if (Strings.ContainsKey(item.NameStr))
                {
                    item.FullName = Strings[item.NameStr];
                    item.FullNameWithType = item.FullName;
                    item.IsType = false;
                }
            }
        }

        private void BuildItemTypeTree()
        {
            foreach (ItemType item in ItemTypes.Values)
            {
                if (string.IsNullOrEmpty(item.Code))
                {
                    continue;
                }
                item.Parents = new List<ItemType>();
                if (!string.IsNullOrEmpty(item.Parent1))
                {
                    if (ItemTypes.ContainsKey(item.Parent1))
                    {
                        var parent = ItemTypes[item.Parent1];
                        item.Parents.Add(parent);
                        if (parent.Children == null)
                        {
                            parent.Children = new List<ItemType>();
                        }
                        parent.Children.Add(item);
                    }
                }
                if (!string.IsNullOrEmpty(item.Parent2))
                {
                    if (ItemTypes.ContainsKey(item.Parent2))
                    {
                        var parent = ItemTypes[item.Parent2];
                        item.Parents.Add(parent);
                        if (parent.Children == null)
                        {
                            parent.Children = new List<ItemType>();
                        }
                        parent.Children.Add(item);
                    }
                }
            }
        }

        public void SetupAffixItemTypes(List<MagicAffix> affixes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var affix in affixes)
            {
                affix.ValidTypes = new HashSet<ItemType>();
                var invalidTypes = new List<string>
                {
                    affix.EType1,
                    affix.EType2,
                    affix.EType3,
                    affix.EType4,
                    affix.EType5
                };
                affix.ValidTypes.UnionWith(GetValidItemTypes(affix.IType1, invalidTypes));
                affix.ValidTypes.UnionWith(GetValidItemTypes(affix.IType2, invalidTypes));
                affix.ValidTypes.UnionWith(GetValidItemTypes(affix.IType3, invalidTypes));
                affix.ValidTypes.UnionWith(GetValidItemTypes(affix.IType4, invalidTypes));
                affix.ValidTypes.UnionWith(GetValidItemTypes(affix.IType5, invalidTypes));
                affix.ValidTypes.UnionWith(GetValidItemTypes(affix.IType6, invalidTypes));
                affix.ValidTypes.UnionWith(GetValidItemTypes(affix.IType7, invalidTypes));
                foreach (var validType in affix.ValidTypes)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(GetTypeFromMap(validType.Code));
                }
                affix.FullTypeString = sb.ToString();
                sb.Clear();
            }
        }

        public HashSet<ItemType> GetValidItemTypes(string typeCode, List<string> invalidTypes = null)
        {
            HashSet<ItemType> validTypes = new HashSet<ItemType>();
            if (ItemTypes.ContainsKey(typeCode))
            {
                if (IsValidType(typeCode, invalidTypes))
                {
                    validTypes.Add(ItemTypes[typeCode]);
                    if (ItemTypes[typeCode].Children == null)
                    {
                        return validTypes;
                    }
                    foreach (var child in ItemTypes[typeCode].Children)
                    {
                        var childTypes = GetValidItemTypes(child.Code, invalidTypes);
                        if (childTypes != null && childTypes.Count > 0)
                        {
                            validTypes.UnionWith(childTypes);
                        }
                    }
                }
            }
            return validTypes;
        }

        private static bool IsValidType(string typeCode, List<string> invalidTypes = null)
        {
            if (invalidTypes == null)
            {
                return true;
            }
            if (invalidTypes.Contains(typeCode))
            {
                return false;
            }
            return true;
        }

        public void CheckPropGroups()
        {
            if (ISCGroups == null)
            {
                InitISCGroups(ISC);
            }
            foreach (var prop in Properties.Values) {
                Dictionary<string, int> stats = prop.Stats;
                List<int> groups = new List<int>();
                foreach (var stat in stats.Keys)
                {
                    if (ISCGroups.ContainsKey(stat))
                    {
                        groups.Add(ISCGroups[stat].DescGroup.GetValueOrDefault());
                    }
                }
                if (groups.Count > 0)
                {
                    foreach (var group in groups)
                    {
                        CheckPropGroup(prop, group);
                    }
                }
            }
        }

        private void CheckPropGroup(Properties prop, int group)
        {
            var fullGroup = ISCGroups.Where(x => x.Value.DescGroup == group).ToList();
            var tempStats = prop.Stats;
            bool isFullGroup = false;
            for (int i = 0; i < fullGroup.Count; i++)
            {
                if (!tempStats.ContainsKey(fullGroup[i].Key))
                {
                    return;
                } else
                {
                    if (i == fullGroup.Count - 1)
                    {
                        isFullGroup = true;
                    }
                }
            }
            if (isFullGroup)
            {
                prop.Stats = tempStats;
                for (int i = 0; i < fullGroup.Count; i++)
                {
                    if (i == 0)
                    {
                        tempStats[fullGroup[i].Key] = 2;
                    }
                    else
                    {
                        tempStats[fullGroup[i].Key] = 1;
                    }
                }
            }
            return;
        }

        public List<Mod> GetStringFromMod(Mod mod)
        {
            List<Mod> returnList = new List<Mod>();
            if (mod.FullMod != "")
            {
                return null;
            }
            if (PropOverrideList == null)
            {
                InitPropOverrideList();
            }
            //Override hardcoded functions
            if (PropOverrideList.Contains(mod.Name))
            {
                returnList.Add(GetStringFromDescOverride(mod));
                return returnList;
            }
            //Get Mods from Property
            if (Properties.ContainsKey(mod.Name))
            {
                Properties prop = Properties[mod.Name];
                Dictionary<string, int> stats = prop.Stats;
                foreach (var stat in stats)
                {
                    if (stat.Value == 1)
                    {
                        continue;
                    }
                    //Get Mod from ItemStatCost
                    if (ISC.ContainsKey(stat.Key))
                    {
                        ItemStatCost isc = ISC[stat.Key];
                        //Get String for mod
                        string str;
                        if (mod.Max >= 0)
                        {
                            if (stat.Value == 2)
                            {
                                str = GetStringFromDesc(isc.DescGroupStrPos);
                            } else
                            {
                                str = GetStringFromDesc(isc.DescStrPos);
                            }
                        } else
                        {
                            if (stat.Value == 2)
                            {
                                str = GetStringFromDesc(isc.DescGroupStrNeg);
                            }
                            else
                            {
                                str = GetStringFromDesc(isc.DescStrNeg);
                            }
                        }
                        string str2 = "";
                        if (stat.Value == 2)
                        {
                            if (isc.DescGroupStr2 != null && isc.DescGroupStr2 != "")
                            {
                                str2 = GetStringFromDesc(isc.DescGroupStr2);
                            }
                        }
                        else
                        {
                            if (isc.DescStr2 != null && isc.DescStr2 != "")
                            {
                                str2 = GetStringFromDesc(isc.DescStr2);
                            }
                        }
                        string value = "(" + mod.Min + "-" + mod.Max + ")";
                        if (mod.Min == mod.Max)
                        {
                            value = mod.Max.ToString();
                        }
                        float newMin;
                        float newMax;
                        bool ignoreStr = false;
                        bool useStr2 = false;
                        int switchVal;
                        if (stat.Value == 2)
                        {
                            switchVal = isc.DescGroupFunc.GetValueOrDefault();
                        } else
                        {
                            switchVal = isc.DescFunc.GetValueOrDefault();
                        }
                        switch (switchVal)
                        {
                            case 12:
                            case 1:
                                value = "+" + value;
                                break;
                            case 2:
                                value += "%";
                                break;
                            case 3:
                                break;
                            case 4:
                                value = "+" + value + "%";
                                break;
                            case 5:
                                newMin = (mod.Min * 100) / 128;
                                newMax = (mod.Max * 100) / 128;
                                value = "(" + newMin + "-" + newMax + ")%";
                                break;
                            case 6:
                                value = "+" + value;
                                useStr2 = true;
                                break;
                            case 7:
                                value += "%";
                                useStr2 = true;
                                break;
                            case 8:
                                value = "+" + value + "%";
                                useStr2 = true;
                                break;
                            case 9:
                                useStr2 = true;
                                break;
                            case 10:
                                newMin = (mod.Min * 100) / 128;
                                newMax = (mod.Max * 100) / 128;
                                value = "(" + newMin + "-" + newMax + ")%";
                                useStr2 = true;
                                break;
                            case 11:
                                mod.Min = Math.Max(mod.Min, 1);
                                mod.Max = Math.Max(mod.Max, 1);
                                newMin = 100 / mod.Min;
                                newMax = 100 / mod.Max;
                                value = "(" + newMax + "-" + newMin + ")";
                                ignoreStr = true;
                                str = "Repairs 1 Durability In " + value + " Seconds";
                                break;
                            case 13:
                                if (ClassMap == null)
                                {
                                    InitClassMap();
                                }
                                if (ClassMap.ContainsKey(prop.Val1.GetValueOrDefault()))
                                {
                                    str = "+" + value + " to " + ClassMap[prop.Val1.GetValueOrDefault()] + " Skill Levels";
                                } else
                                {
                                    str = "+" + value + " to Unknown Class Skill Levels";
                                }
                                ignoreStr = true;
                                break;
                            case 14:
                                if (SkillTabMap == null)
                                {
                                    InitSkillTabMap();
                                }
                                int tabId;
                                if (int.TryParse(mod.Param, out tabId))
                                {
                                    if (SkillTabMap.ContainsKey(tabId))
                                    {
                                        str = "+" + value + " to " + SkillTabMap[tabId];
                                    }
                                }
                                else
                                {
                                    str = "+" + value + " to Unknown Tab Levels";
                                }
                                ignoreStr = true;
                                break;
                            case 15:
                                str = str.Replace("%d%%", mod.Min.ToString() + "%");
                                str = str.Replace("%d", mod.Max.ToString());
                                str = str.Replace("%s", mod.Param);
                                ignoreStr = true;
                                break;
                            case 16:
                                str = "Level " + value + " " + mod.Param + " Aura When Equipped";
                                ignoreStr = true;
                                break;
                            case 19:
                                str = str.Replace("%d", value);
                                ignoreStr = true;
                                break;
                            case 20:
                                value = "-" + value + "%";
                                break;
                            case 21:
                                value = "-" + value;
                                break;
                            case 23:
                                //Fix
                                value += "%";
                                break;
                            case 24:
                                str = "Level " + Math.Abs(mod.Max) + " " + mod.Param + " " + str.Replace("%d", Math.Abs(mod.Min).ToString());
                                ignoreStr = true;
                                break;
                            case 27:
                                //Fix
                                value = "+" + value;
                                str = value + " to " + mod.Param + "(Class only)";
                                ignoreStr = true;
                                break;
                            case 28:
                                value = "+" + value;
                                str = value + " to " + mod.Param;
                                ignoreStr = true;
                                break;

                        }
                        if (!ignoreStr)
                        {
                            if (stat.Value == 2)
                            {
                                if (isc.DescGroupVal.GetValueOrDefault() == 1)
                                {
                                    str = value + " " + str;
                                }
                                else if (isc.DescGroupVal.GetValueOrDefault() == 2)
                                {
                                    str += " " + value;
                                }
                            }
                            else
                            {
                                if (isc.DescVal.GetValueOrDefault() == 1)
                                {
                                    str = value + " " + str;
                                }
                                else if (isc.DescVal.GetValueOrDefault() == 2)
                                {
                                    str += " " + value;
                                }
                            }
                            if (useStr2)
                            {
                                str += " " + str2;
                            }
                        }
                        mod.FullMod = str;
                        var newMod = new Mod(mod.Name, mod.Param, mod.Min, mod.Max)
                        {
                            FullMod = mod.FullMod
                        };
                        returnList.Add(newMod);
                    }
                }
            }
            return returnList;
        }

        public string GetStringFromDesc(string desc)
        {
            if (Strings.ContainsKey(desc))
            {
                string str = Strings[desc];
                str = RemoveBetween(str, '\\', ';');
                str = str.Replace("\\n", string.Empty);
                return str;
            } else
            {
                return "";
            }
        }

        public static Mod GetStringFromDescOverride(Mod mod)
        {
            string str = "";
            string value = "(" + mod.Min + "-" + mod.Max + ")";
            if (mod.Min == mod.Max)
            {
                value = mod.Max.ToString();
            }
            float length;
            switch (mod.Name)
            {
                //Sockets display
                case "sock":
                    if (string.IsNullOrEmpty(mod.Param))
                    {
                        str = "Sockets " + value;
                    }
                    else
                    {
                        str = "Sockets (" + mod.Param + ")";
                    }
                    break;
                //Fire damage
                case "dmg-fire":
                    str = "Adds " + value + " fire damage";
                    break;
                //Cold damage
                case "dmg-cold":
                    if (!float.TryParse(mod.Param, out length))
                    {
                        length = 0;
                    } else
                    {
                        length /= 25;
                    }
                    str = "Adds " + value + " cold damage (" + length + " seconds)";
                    break;
                //Lightning damage
                case "dmg-ltng":
                    str = "Adds " + value + " lightning damage";
                    break;
                //Poison damage
                case "dmg-pois":
                    if (!float.TryParse(mod.Param, out length))
                    {
                        length = 0;
                    }
                    int damage = (int)(((float)mod.Max / 256) * length);
                    str = "Adds " + damage + " poison damage over " + (int)(length / 25) + " seconds";
                    break;
                //Magic damage
                case "dmg-mag":
                    str = "Adds " + value + " magic damage";
                    break;
                //Phys damage
                case "dmg-norm":
                case "dmg-throw":
                    str = "Adds " + value + " damage";
                    break;
            }
            mod.FullMod = str;
            return mod;
        }

        public static string RemoveBetween(string s, char begin, char end)
        {
            Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(s, string.Empty);
        }

        public void ConvertRecipes()
        {
            foreach (var recipe in Recipes)
            {
                TextInfo descInfo = Culture.TextInfo;
                recipe.Description = descInfo.ToTitleCase(recipe.Description);
                recipe.ItemType = GetTypeFromMap(recipe.Input1);
                recipe.InputTypes = new string[3];
                recipe.InputTypes[0] = recipe.Input2;
                recipe.InputTypes[1] = recipe.Input3;
                recipe.InputTypes[2] = recipe.Input4;
                recipe.InputNames = new string[3];
                recipe.InputNames[0] = GetTypeFromMap(recipe.Input2);
                recipe.InputNames[1] = GetTypeFromMap(recipe.Input3);
                recipe.InputNames[2] = GetTypeFromMap(recipe.Input4);
                recipe.Mods = new List<Mod>
                {
                    new Mod(recipe.Mod1, recipe.Mod1Param, recipe.Mod1Min.GetValueOrDefault(), recipe.Mod1Max.GetValueOrDefault()),
                    new Mod(recipe.Mod2, recipe.Mod2Param, recipe.Mod2Min.GetValueOrDefault(), recipe.Mod2Max.GetValueOrDefault()),
                    new Mod(recipe.Mod3, recipe.Mod3Param, recipe.Mod3Min.GetValueOrDefault(), recipe.Mod3Max.GetValueOrDefault()),
                    new Mod(recipe.Mod4, recipe.Mod4Param, recipe.Mod4Min.GetValueOrDefault(), recipe.Mod4Max.GetValueOrDefault())
                };
                var newMods = new List<Mod>();
                foreach (var mod in recipe.Mods)
                {
                    newMods.AddRange(GetStringFromMod(mod));
                }
                recipe.Mods = newMods;
            }
        }

        public string GetTypeFromMap(string type)
        {
            if (TypeMap == null)
            {
                InitTypeMap();
            }
            if (TypeMap.ContainsKey(type))
            {
                return TypeMap[type];
            } else
            {
                if (ItemTypes.ContainsKey(type))
                {
                    return ItemTypes[type].Type;
                }
            }
            return "";
        }

        public void PrepareAffixMods(List<MagicAffix> affixDict)
        {
            foreach (var affix in affixDict)
            {
                affix.Mods = new List<Mod>
                {
                    new Mod(affix.Mod1Code, affix.Mod1Param, affix.Mod1Min.GetValueOrDefault(), affix.Mod1Max.GetValueOrDefault()),
                    new Mod(affix.Mod2Code, affix.Mod2Param, affix.Mod2Min.GetValueOrDefault(), affix.Mod2Max.GetValueOrDefault()),
                    new Mod(affix.Mod3Code, affix.Mod3Param, affix.Mod3Min.GetValueOrDefault(), affix.Mod3Max.GetValueOrDefault())
                };
                var newMods = new List<Mod>();
                foreach (var mod in affix.Mods)
                {
                    newMods.AddRange(GetStringFromMod(mod));
                }
                affix.Mods = newMods;
            }
        }

        public void CompilePropertyStats()
        {
            foreach (var property in Properties.Values)
            {
                property.Stats = new Dictionary<string, int>();
                AddIfNotBlank(property.Stats, property.Stat1);
                AddIfNotBlank(property.Stats, property.Stat2);
                AddIfNotBlank(property.Stats, property.Stat3);
                AddIfNotBlank(property.Stats, property.Stat4);
                AddIfNotBlank(property.Stats, property.Stat5);
                AddIfNotBlank(property.Stats, property.Stat6);
                AddIfNotBlank(property.Stats, property.Stat7);
            }
        }

        public static void AddIfNotBlank(Dictionary<string, int> list, string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                list.Add(str, 0);
            }
        }

        public void InitItemsAndTypesList()
        {
            ItemAndTypeList = new List<ItemShort>();
            ItemAndTypeList.AddRange(ItemsShort.Values);
            foreach (var type in ItemTypes.Values)
            {
                var itemShort = new ItemShort();
                itemShort.Init(type.Code, GetTypeFromMap(type.Code));
                ItemAndTypeList.Add(itemShort);
            }
        }

        public void InitISCGroups(Dictionary<string, ItemStatCost> iscDict)
        {
            ISCGroups = new Dictionary<string, ItemStatCost>();
            foreach (var isc in iscDict.Values)
            {
                if (isc.DescGroup.GetValueOrDefault() != 0)
                {
                    ISCGroups.Add(isc.Stat, isc);
                }
            }
        }

        public void InitClassMap()
        {
            ClassMap = new Dictionary<int, string>
            {
                { 0, "Amazon" },
                { 1, "Sorceress" },
                { 2, "Necromancer" },
                { 3, "Paladin" },
                { 4, "Barbarian" },
                { 5, "Druid" },
                { 6, "Assassin" }
            };
        }

        public void InitPropOverrideList()
        {
            PropOverrideList = new List<string>
            {
                "sock",
                "dmg-fire",
                "dmg-ltng",
                "dmg-mag",
                "dmg-cold",
                "dmg-pois",
                "dmg-throw",
                "dmg-norm"
            };
        }

        public void InitSkillTabMap()
        {
            SkillTabMap = new Dictionary<int, string>
            {
                { 0, "Bow and Crossbow Skills (Amazon Only)" },
                { 1, "Passive and Magic Skills (Amazon Only)" },
                { 2, "Javelin and Spear Skills (Amazon Only)" },
                { 3, "Fire Spells (Sorceress Only)" },
                { 4, "Lightning Spells (Sorceress Only)" },
                { 5, "Cold Spells (Sorceress Only)" },
                { 6, "Curses (Necromancer Only)" },
                { 7, "Poison and Bone Spells (Necromancer Only)" },
                { 8, "Summoning Spells (Necromancer Only)" },
                { 9, "Combat Skills (Paladin Only)" },
                { 10, "Offensive Auras (Paladin Only)" },
                { 11, "Defensive Auras (Paladin Only)" },
                { 12, "Combat Skills (Barbarian Only)" },
                { 13, "Masteries (Barbarian Only)" },
                { 14, "Warcries (Barbarian Only)" },
                { 15, "Summoning Skills (Druid Only)" },
                { 16, "Shape Shifting (Druid Only)" },
                { 17, "Elemental Skills (Druid Only)" },
                { 18, "Traps (Assassin Only)" },
                { 19, "Shadow Disciplines (Assassin Only)" },
                { 20, "Martial Arts (Assassin Only)" },
            };
        }

        public void InitTypeMap()
        {
            TypeMap = new Dictionary<string, string>
            {
                { "shie", "Non-class Shield" },
                { "tors", "Body Armor" },
                { "bowq", "Arrows" },
                { "xboq", "Bolts" },
                { "knif", "Dagger" },
                { "tkni", "Throwing Dagger" },
                { "armo", "Armor" },
                { "shld", "Shield" },
                { "misc", "Misc" },
                { "sock", "Socketable" },
                { "seco", "Off Hand" },
                { "rod", "Rods" },
                { "blun", "Blunt Weapon" },
                { "h2h", "Claw" },
                { "orb", "Orb" },
                { "head", "Voodoo Head" },
                { "ashd", "Auric Shield" },
                { "mcha", "Large Charm" },
                { "lcha", "Grand Charm" },
                { "asm", "Dagger/Claw" },
                { "corr", "Worldstone Shard" },
                { "r03g", "Tir Rune" },
                { "r04g", "Nef Rune" },
                { "r05g", "Eth Rune" },
                { "r06g", "Ith Rune" },
                { "r07g", "Tal Rune" },
                { "r08g", "Ral Rune" },
                { "r09g", "Ort Rune" },
                { "r10g", "Thul Rune" },
                { "r11g", "Amn Rune" },
                { "r12g", "Sol Rune" },
                { "gg4a", "Perfect Amethyst" },
                { "gg4e", "Perfect Emerald" },
                { "gg4r", "Perfect Ruby" },
                { "gg4s", "Perfect Sapphire" },
            };
        }
    }
}
