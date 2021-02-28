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

        public HintProviders(IRandomGenerator random, ILocTable commonStrings)
        {
            Helper.AssertIsNotNull(random, commonStrings);

            BasicHints = new HintProvider(CreateBasicHints(commonStrings), random);
            AdvancedHints = new HintProvider(CreateAdvancedHints(commonStrings), random);
        }

        private IList<string> CreateBasicHints(ILocTable commonStrings)
        {
            IList<string> keys = new List<string>()
            {
                "Hints/Factories",
                "Hints/BuilderAutomaticRepair",
                "Hints/ClickItemForDetails",
                "Hints/DifficultyIsChangeable",
                "Hints/HowToChangeCruiser",
                "Hints/CheckEnemyCruiser",
                "Hints/HelpLabels"
            };
            IList<string> hints = GetStrings(commonStrings, keys);

            string buildingsAreDeleteableBase = commonStrings.GetString("Hints/BuildingsAreDeletable");
            string deleteButtonText = commonStrings.GetString("UI/Informator/DemolishButton");
            hints.Add(string.Format(buildingsAreDeleteableBase, deleteButtonText));

            return hints;
        }

        private IList<string> CreateAdvancedHints(ILocTable commonStrings)
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

            IList<string> hints = GetStrings(commonStrings, keys);

            string targetButtonBase = commonStrings.GetString("Hints/TargetButton");
            string targetButtonText = commonStrings.GetString("UI/Informator/TargetButton");
            hints.Add(string.Format(targetButtonBase, targetButtonText));

            string buildersButtonBase = commonStrings.GetString("Hints/BuildersButton");
            string buildersButtonText = commonStrings.GetString("UI/Informator/DronesButton");
            hints.Add(string.Format(buildersButtonBase, buildersButtonText));

            return hints;
        }

        private IList<string> GetStrings(ILocTable commonStrings, IList<string> keys)
        {
            return
                keys
                    .Select(key => commonStrings.GetString(key))
                    .ToList();
        }
    }
}