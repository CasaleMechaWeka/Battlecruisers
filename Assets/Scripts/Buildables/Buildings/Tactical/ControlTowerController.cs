using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class ControlTowerController : TacticalBuilding
    {
		private IBoostProvider _boostProvider;

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.ControlTower; } }

        public float boostMultiplier;

		protected override void OnInitialised()
		{
			base.OnInitialised();
            _boostProvider = _factoryProvider.BoostFactory.CreateBoostProvider(boostMultiplier);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();
            _factoryProvider.GlobalBoostProviders.AircraftBoostProviders.Add(_boostProvider);
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
            _factoryProvider.GlobalBoostProviders.AircraftBoostProviders.Remove(_boostProvider);
		}
	}
}
