using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public class PvPSlotWrapperController : MonoBehaviour, IPvPSlotNumProvider
    {
        private IList<PvPSlot> _slots;
        public Dictionary<string, IPvPSlot> _slotsByName = new Dictionary<string, IPvPSlot>();
        // For out of battle scene use
        public void StaticInitialise()
        {
            _slots = GetComponentsInChildren<PvPSlot>(includeInactive: true).ToList();
        }

        // For in battle scene use
        public IPvPSlotAccessor Initialise(IPvPCruiser parentCruiser)
        {
            Assert.IsNotNull(parentCruiser);

            IPvPBuildingPlacer buildingPlacer
                = new PvPBuildingPlacer(
                    new PvPBuildingPlacerCalculator());
            IPvPSlotInitialiser slotInitialiser = new PvPSlotInitialiser();
            for (int i = 0; i < _slots.Count; ++i)
            {
                 _slotsByName.Add(_slots[i].gameObject.name, _slots[i]);
            }
                IDictionary<PvPSlotType, ReadOnlyCollection<PvPSlot>> typeToSlots = slotInitialiser.InitialiseSlots(parentCruiser, _slots, buildingPlacer);

            return new PvPSlotAccessor(typeToSlots);
        }

        public int GetSlotCount(PvPSlotType type)
        {
            return _slots.Count(slot => slot.Type == type);
        }

        static PvPSlotWrapperController slotWrapperController;
        public static PvPSlotWrapperController Instance
        {
            get
            {
                if (slotWrapperController == null)
                {
                    slotWrapperController = FindObjectOfType<PvPSlotWrapperController>();
                }
                if (slotWrapperController == null)
                {
                    Debug.LogError("No ServerSingleton in scene, did you run this from the bootstrap scene?");
                    return null;
                }
                return slotWrapperController;
            }
        }
    }
}
