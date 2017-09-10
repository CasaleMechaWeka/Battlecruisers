using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Slots;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class LocalBoosterController : Building
    {
        private IBoostProvider _boostProvider;
        private IList<ISlot> _neighbouringSlots;

        private const int BOOST_RADIUS_IN_M = 1;

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

            // Provide boost to nearby slots (and own slot :P )
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, BOOST_RADIUS_IN_M, slotLayerMask);

            foreach (Collider2D collider in colliders)
            {
                ISlot slot = collider.GetComponent<ISlot>();
                Assert.IsNotNull(slot, "All colliders in the slots layer should contain an ISlot component :D");
                _neighbouringSlots.Add(slot);

                slot.BoostProviders.Add(_boostProvider);
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
