using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class SlotsTestGod : MonoBehaviour
    {
        private void Start()
        {
            ICruiser parentCruiser = Substitute.For<ICruiser>();
            ReadOnlyCollection<ISlot> neighbouringSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>());
            IBuildingPlacer buildingPlacer = Substitute.For<IBuildingPlacer>();

            Slot[] slots = FindObjectsOfType<Slot>();

            foreach (Slot slot in slots)
            {
                slot.Initialise(parentCruiser, neighbouringSlots, buildingPlacer);
            }
        }
    }
}