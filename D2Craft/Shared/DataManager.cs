using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static D2Craft.Shared.Constants;

namespace D2Craft.Shared
{
    public class DataManager
    {

        public List<CubeMain> Recipes { get; set; }
        public Dictionary<string, ItemStatCost> ISC { get; set; }
        public Dictionary<string, Properties> Properties { get; set; }
        public List<MagicAffix> Prefixes { get; set; }
        public List<MagicAffix> Suffixes { get; set; }
        public Dictionary<string, string> Strings { get; set; }
        public Dictionary<string, string> TypeMap { get; set; }
        public StateContainer StateContainer { get; set; }
        public bool Loaded { get; set; }

        public DataManager(StateContainer stateContainer)
        {
            Loaded = false;
            StateContainer = stateContainer;
            Recipes = ReadCsvToList<CubeMain>(stateContainer.DataFiles[DataFileTypes.CubeMain]);
            ISC = ReadCsvToDictISC(stateContainer.DataFiles[DataFileTypes.ItemStatCost]);
            Properties = ReadCsvToDictProp(stateContainer.DataFiles[DataFileTypes.Properties]);
            CompilePropertyStats();
            Prefixes = ReadCsvToList<MagicAffix>(stateContainer.DataFiles[DataFileTypes.MagicPrefix]);
            Suffixes = ReadCsvToList<MagicAffix>(stateContainer.DataFiles[DataFileTypes.MagicSuffix]);
            Strings = new Dictionary<string, string>();
            ReadStrings(stateContainer.DataFiles[DataFileTypes.PatchStrings]);
            ReadStrings(stateContainer.DataFiles[DataFileTypes.ExpStrings]);
            ReadStrings(stateContainer.DataFiles[DataFileTypes.Strings]);
            ConvertRecipes();
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
                if (!dict.ContainsKey(record.Stat))
                {
                    dict.Add(record.Stat, record);
                }
            }
            return dict;
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
                if (!dict.ContainsKey(record.Code))
                {
                    dict.Add(record.Code, record);
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

        public void GetStringFromMod(Mod mod)
        {
            if (mod.FullMod != "")
            {
                return;
            }
            //Get Mods from Property
            if (Properties.ContainsKey(mod.Name))
            {
                List<string> stats = Properties[mod.Name].Stats;
                foreach (var stat in stats)
                {
                    if (ISC.ContainsKey(stat))
                    {
                        string statStr = ISC[stat].DescStrPos;
                        if (Strings.ContainsKey(statStr))
                        {
                            string str = Strings[statStr];
                            str += " (" + mod.Min + "-" + mod.Max + ")";
                            mod.FullMod = str;
                        }
                    }
                }
            }
            //mod.FullMod = mod.Name + " (" + mod.Min + "-" + mod.Max + ")";
        }

        public void ConvertRecipes()
        {
            if (TypeMap == null)
            {
                InitTypeMap();
            }
            foreach (var recipe in Recipes)
            {
                recipe.ItemType = TypeMap[recipe.Input1];
                recipe.InputTypes = new string[3];
                recipe.InputTypes[0] = recipe.Input2;
                recipe.InputTypes[1] = recipe.Input3;
                recipe.InputTypes[2] = recipe.Input4;
                recipe.InputNames = new string[3];
                recipe.InputNames[0] = TypeMap[recipe.Input2];
                recipe.InputNames[1] = TypeMap[recipe.Input3];
                recipe.InputNames[2] = TypeMap[recipe.Input4];
                recipe.Mods = new Mod[4];
                recipe.Mods[0] = new Mod(recipe.Mod1, recipe.Mod1Param, recipe.Mod1Min.GetValueOrDefault(), recipe.Mod1Max.GetValueOrDefault());
                recipe.Mods[1] = new Mod(recipe.Mod2, recipe.Mod2Param, recipe.Mod2Min.GetValueOrDefault(), recipe.Mod2Max.GetValueOrDefault());
                recipe.Mods[2] = new Mod(recipe.Mod3, recipe.Mod3Param, recipe.Mod3Min.GetValueOrDefault(), recipe.Mod3Max.GetValueOrDefault());
                recipe.Mods[3] = new Mod(recipe.Mod4, recipe.Mod4Param, recipe.Mod4Min.GetValueOrDefault(), recipe.Mod4Max.GetValueOrDefault());
                foreach (var mod in recipe.Mods)
                {
                    GetStringFromMod(mod);
                }
            }
        }

        public void CompilePropertyStats()
        {
            foreach (var property in Properties.Values)
            {
                property.Stats = new List<string>();
                AddIfNotBlank(property.Stats, property.Stat1);
                AddIfNotBlank(property.Stats, property.Stat2);
                AddIfNotBlank(property.Stats, property.Stat3);
                AddIfNotBlank(property.Stats, property.Stat4);
                AddIfNotBlank(property.Stats, property.Stat5);
                AddIfNotBlank(property.Stats, property.Stat6);
                AddIfNotBlank(property.Stats, property.Stat7);
            }
        }

        public static void AddIfNotBlank(List<string> list, string str)
        {
            if (str != "" && str != null)
            {
                list.Add(str);
            }
        }

        public void InitTypeMap()
        {
            TypeMap = new Dictionary<string, string>
            {
                { "amul", "Amulet" },
                { "belt", "Belt" },
                { "boot", "Boots" },
                { "glov", "Gloves" },
                { "helm", "Helm" },
                { "ring", "Ring" },
                { "shld", "Shield" },
                { "tors", "Chest" },
                { "weap", "Weapon" },
                { "jew", "Jewel" },
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
