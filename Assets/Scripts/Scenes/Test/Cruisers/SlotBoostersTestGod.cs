using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Cruisers
{
    public class SlotBoostersTestGod : MonoBehaviour
    {
        private void Start()
        {
            ICruiser parentCruiser = Substitute.For<ICruiser>();
            ReadOnlyCollection<Slot> neighbouringSlots = new ReadOnlyCollection<Slot>(new List<Slot>());
            IBoostProvider boostProvider = Substitute.For<IBoostProvider>();

            Slot[] slots = FindObjectsOfType<Slot>();

            foreach (Slot slot in slots)
            {
                slot.Initialise(parentCruiser, neighbouringSlots);
                slot.BoostProviders.Add(boostProvider);
                slot.BoostProviders.Add(boostProvider);
            }
        }
    }
}