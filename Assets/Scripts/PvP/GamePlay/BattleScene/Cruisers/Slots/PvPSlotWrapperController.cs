using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Slots
{
    public class PvPSlotWrapperController : MonoBehaviour, IPvPSlotNumProvider
    {
        private IList<IPvPSlot> _slots;

        // For out of battle scene use
        public void StaticInitialise()
        {
            _slots = GetComponentsInChildren<IPvPSlot>(includeInactive: true).ToList();
        }

        // For in battle scene use
        public IPvPSlotAccessor Initialise(IPvPCruiser parentCruiser)
        {
            Assert.IsNotNull(parentCruiser);

            IPvPBuildingPlacer buildingPlacer
                = new PvPBuildingPlacer(
                    new PVPBuildingPlacerCalculator());
            IPVPSlotInitialiser slotInitialiser = new PvPSlotInitialiser();
            IDictionary<SlotType, ReadOnlyCollection<IPvPSlot>> typeToSlots = slotInitialiser.InitialiseSlots(parentCruiser, _slots, buildingPlacer);

            return new PVPSlotAccessor(typeToSlots);
        }

        public int GetSlotCount(SlotType type)
        {
            return _slots.Count(slot => slot.Type == type);
        }
    }

}
