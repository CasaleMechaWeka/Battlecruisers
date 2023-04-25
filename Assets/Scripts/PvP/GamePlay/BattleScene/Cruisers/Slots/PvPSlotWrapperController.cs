using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
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
                    new PvPBuildingPlacerCalculator());
            IPvPSlotInitialiser slotInitialiser = new PvPSlotInitialiser();
            IDictionary<PvPSlotType, ReadOnlyCollection<IPvPSlot>> typeToSlots = slotInitialiser.InitialiseSlots(parentCruiser, _slots, buildingPlacer);

            return new PvPSlotAccessor(typeToSlots);
        }

        public int GetSlotCount(PvPSlotType type)
        {
            return _slots.Count(slot => slot.Type == type);
        }
    }
}
