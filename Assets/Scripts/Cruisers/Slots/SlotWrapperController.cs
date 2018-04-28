using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotWrapperController : MonoBehaviour
    {
        public ISlotWrapper Initialise(ICruiser parentCruiser, ISlotFilter slotFilter)
        {
            Helper.AssertIsNotNull(parentCruiser, slotFilter);

            IList<ISlot> slots = GetComponentsInChildren<ISlot>(includeInactive: true).ToList();
            return new SlotWrapper(parentCruiser, slots, slotFilter);
        }
    }
}
