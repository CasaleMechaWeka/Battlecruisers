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
        public ISlotAccessor Initialise(ICruiser parentCruiser)
        {
            Assert.IsNotNull(parentCruiser);

            IBuildingPlacer buildingPlacer
                = new BuildingPlacer(
                    new BuildingPlacerCalculator());
            ISlotInitialiser slotInitialiser = new SlotInitialiser();
            IDictionary<SlotType, ReadOnlyCollection<ISlot>> typeToSlots = slotInitialiser.InitialiseSlots(parentCruiser, _slots, buildingPlacer);

            return new SlotAccessor(typeToSlots);
        }

        public int GetSlotCount(SlotType type)
        {
            return _slots.Count(slot => slot.Type == type);
        }
    }

}
