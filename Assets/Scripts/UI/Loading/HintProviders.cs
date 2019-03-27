using BattleCruisers.Utils;
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
                "Buildings can be deleted by clicking them and then selecting the little trash can in the building details panel.",
                "Clicking on a cruiser, unit or building will open details panel about that item.",
                "Build ship turrets and mortars at the front of your cruiser so they are in range of enemy ships."
            };
        }

        private IList<string> CreateAdvancedHints()
        {
            return new List<string>()
            {
                "The “Target” button in an enemy building’s details panel makes all your units and buildings attack that building.  The shortcut for this is to double click the enemy building.",
                "The “Builders” button in one of your building’s details panel makes all your builders try to work on that building.  The shortcut for this is to double click your building.",
                "Each cruiser has a unique benefit and a unique slot arrangement.  Choose your cruiser wisely!",
                "Frequently check on the enemy cruiser to avoid nasty surprises!",
                "Local boosters significantly increase adjacent turret fire rates, shield recharge rates and factory production rates.  They are well worth building!",
                "It makes more sense to construct new builder bays one at a time instead of at the same time, so you get the benefit of a completed builder bay as soon as possible."
            };
        }
    }
}