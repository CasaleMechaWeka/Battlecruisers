using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotWrapperController : MonoBehaviour
    {
        public ISlotWrapper Initialise(ICruiser parentCruiser, ISlotFilter highlightableFilter)
        {
            Helper.AssertIsNotNull(parentCruiser, highlightableFilter);

            IList<ISlot> slots = GetComponentsInChildren<ISlot>(includeInactive: true).ToList();
            IBuildingPlacer buildingPlacer = new BuildingPlacer();

            return new SlotWrapper(parentCruiser, slots, highlightableFilter, buildingPlacer);
        }
    }
}
