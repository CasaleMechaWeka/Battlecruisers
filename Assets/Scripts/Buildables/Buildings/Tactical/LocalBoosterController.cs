using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Slots;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class LocalBoosterController : Building
    {
        private IBoostProvider _boostProvider;

        private const int BOOST_RADIUS_IN_M = 1;

        public float boostMultiplier;
        public LayerMask slotLayerMask;

        protected override void OnInitialised()
        {
            base.OnInitialised();

            _boostProvider = new BoostProvider(boostMultiplier);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            // Provide boost to nearby slots
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, BOOST_RADIUS_IN_M, slotLayerMask);

            foreach (Collider2D collider in colliders)
            {
                ISlot slot = collider.GetComponent<ISlot>();
                Assert.IsNotNull(slot, "All colliders in the slots layer should contain an ISlot component :D");

                slot.BoostConsumer.AddBoostProvider(_boostProvider);
            }
		}

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            _boostProvider.ClearBoostConsumers();
        }
    }
}
