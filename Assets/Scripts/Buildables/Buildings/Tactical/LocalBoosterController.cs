using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class LocalBoosterController : Building
    {
        private IBoostProvider _boostProvider;

        public float boostMultiplier;

        protected override void OnInitialised()
        {
            base.OnInitialised();

            _boostProvider = _factoryProvider.BoostFactory.CreateBoostProvider(boostMultiplier);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            Logging.Log(Tags.LOCAL_BOOSTER, "About to boost " + _parentSlot.NeighbouringSlots.Count + " slots :D");

            foreach (ISlot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Add(_boostProvider);
            }
		}

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            foreach (ISlot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Remove(_boostProvider);
            }
        }
    }
}
