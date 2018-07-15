using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotWrapperController : MonoBehaviour, ISlotNumProvider
    {
        private IList<ISlot> _slots;

        // For out of battle scene use
        public void StaticInitialise()
        {
            _slots = GetComponentsInChildren<ISlot>(includeInactive: true).ToList();
        }

        // For in battle scene use
        public ISlotWrapper Initialise(ICruiser parentCruiser, ISlotFilter highlightableFilter)
        {
            Helper.AssertIsNotNull(parentCruiser, highlightableFilter);

            IBuildingPlacer buildingPlacer = new BuildingPlacer();

            return new SlotWrapper(parentCruiser, _slots, highlightableFilter, buildingPlacer);
        }

        public int GetSlotCount(SlotType type)
        {
            return _slots.Count(slot => slot.Type == type);
        }
    }
}
