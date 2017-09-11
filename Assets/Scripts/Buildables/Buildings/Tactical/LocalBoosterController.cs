using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class LocalBoosterController : Building
    {
        private IBoostProvider _boostProvider;
        private IList<ISlot> _neighbouringSlots;

        public float boostRadiusInM;
        public float boostMultiplier;
        public LayerMask slotLayerMask;

        protected override void OnInitialised()
        {
            base.OnInitialised();

            _boostProvider = new BoostProvider(boostMultiplier);
			_neighbouringSlots = new List<ISlot>();
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            AddSlotsWithinRange();

            Logging.Log(Tags.LOCAL_BOOSTER, "About to boost " + _neighbouringSlots.Count + " slots :D");

            foreach (ISlot slot in _neighbouringSlots)
            {
                slot.BoostProviders.Add(_boostProvider);
            }
		}

        private void AddSlotsWithinRange()
        {
            foreach (ISlot slot in _parentCruiser.SlotWrapper.Slots)
            {
                if (Vector2.Distance(Position, slot.Position) <= boostRadiusInM)
                {
                    _neighbouringSlots.Add(slot);
                }
            }
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            foreach (ISlot slot in _neighbouringSlots)
            {
                slot.BoostProviders.Remove(_boostProvider);
            }
        }
    }
}
