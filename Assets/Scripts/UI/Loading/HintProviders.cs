using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.UI.Loading
{
    public class HintProviders
    {
        public IHintProvider BasicHints { get; }
        public IHintProvider AdvancedHints { get; }

        public HintProviders(IRandomGenerator random)
        {
            Helper.AssertIsNotNull(random);

            BasicHints = new HintProvider(CreateBasicHints(), random);
            AdvancedHints = new HintProvider(CreateAdvancedHints(), random);
        }

        private IList<string> CreateBasicHints()
        {
            IList<string> keys = new List<string>()
            {
                "Hints/Factories",
                "Hints/BuilderAutomaticRepair",
                "Hints/DifficultyIsChangeable",
                "Hints/HowToChangeCruiser",
                "Hints/CheckEnemyCruiser"
            };
            IList<string> hints = GetStrings(keys);

            string buildingsAreDeleteableBase = LocTableCache.CommonTable.GetString("Hints/BuildingsAreDeletable");
            string deleteButtonText = LocTableCache.CommonTable.GetString("UI/Informator/DemolishButton");
            hints.Add(string.Format(buildingsAreDeleteableBase, deleteButtonText));

            return hints;
        }

        private IList<string> CreateAdvancedHints()
        {
            IList<string> keys = new List<string>()
            {
                "Hints/CruisersAreUnique",
                "Hints/ConstructBuildingsSequentially",
                "Hints/PauseUnitProduction"
            };

            if (!SystemInfoBC.Instance.IsHandheld)
            {
                keys.Add("Hints/Hotkeys");
            }

            IList<string> hints = GetStrings(keys);

            string targetButtonBase = LocTableCache.CommonTable.GetString("Hints/TargetButton");
            string targetButtonText = LocTableCache.CommonTable.GetString("UI/Informator/TargetButton");
            hints.Add(string.Format(targetButtonBase, targetButtonText));

            string buildersButtonBase = LocTableCache.CommonTable.GetString("Hints/BuildersButton");
            string buildersButtonText = LocTableCache.CommonTable.GetString("Common/Builders");
            hints.Add(string.Format(buildersButtonBase, buildersButtonText));

            return hints;
        }

        private IList<string> GetStrings(IList<string> keys)
        {
            return
                keys
                    .Select(key => LocTableCache.CommonTable.GetString(key))
                    .ToList();
        }
    }
}