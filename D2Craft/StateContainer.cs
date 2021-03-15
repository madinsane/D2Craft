using D2Craft.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static D2Craft.Shared.Constants;

namespace D2Craft
{
    public class StateContainer
    {
        public string Property { get; set; } = "Initial value from StateContainer";

        public event Action OnChange;
        public DataManager DataManager { get; set; }
        public Dictionary<DataFileTypes, string> DataFiles { get; set; }

        public void SetProperty(string value)
        {
            Property = value;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public bool IsDataLoaded()
        {
            var dm = DataManager;
            if (dm != null)
            {
                return dm.Recipes != null && dm.ISC != null && dm.Properties != null && dm.Prefixes != null && dm.Suffixes != null && dm.Strings != null && dm.ItemDict != null && dm.Loaded;
            }
            return false;
        }
    }
}
