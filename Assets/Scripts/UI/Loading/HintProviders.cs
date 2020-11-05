using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Loading
{
    public class HintProviders
    {
        public IHintProvider BasicHints { get; }
        public IHintProvider AdvancedHints { get; }

        public HintProviders(IRandomGenerator random)
        {
            Assert.IsNotNull(random);

            BasicHints = new HintProvider(CreateBasicHints(), random);
            AdvancedHints = new HintProvider(CreateAdvancedHints(), random);
        }

        private IList<string> CreateBasicHints()
        {
            return new List<string>()
            {
                "Build factories to produce units.",
                "Builders automatically repair damaged buildings and your cruiser.",
                "Buildings can be deleted by clicking them and then selecting the DEMOLISH button.",
                "Click on a cruiser, unit or building to show more details.",
                "Too easy or too hard?  Change the difficulty from the options screen.",
                "Want to change your cruiser?  Head to the loadout screen.",
                "Frequently check on the enemy cruiser to avoid nasty surprises!",
                "Use the in game question mark (bottom right) to show help labels."
            };
        }

        private IList<string> CreateAdvancedHints()
        {
            IList<string> hints = new List<string>()
            {
                "The TARGET button for an enemy building makes everyone attack that building.  The shortcut is to double click the enemy building.",
                "The BUILDERS button for one of your building’s makes all your builders try to work on that building.  The shortcut is to double click your building.",
                "Each cruiser has a unique benefit and slot arrangement.  Choose your cruiser wisely!",
                "Local boosters are well worth building.",
                "Construct buildings one at a time instead of all at once, to get their benefits sooner.",
                "You can pause and resume building units by clicking the factory and selecting the unit you want to pause or resume."
            };

            if (!SystemInfoBC.Instance.IsHandheld)
            {
                hints.Add("Want to become more efficient?  Check out the hotkeys in the options screen.");
            }

            return hints;
        }
    }
}