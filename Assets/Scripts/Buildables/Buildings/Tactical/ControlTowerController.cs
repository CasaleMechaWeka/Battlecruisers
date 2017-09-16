using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class ControlTowerController : Building
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
            _factoryProvider.BoostProvidersManager.AircraftBoostProviders.Add(_boostProvider);
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
            _factoryProvider.BoostProvidersManager.AircraftBoostProviders.Remove(_boostProvider);
		}
	}
}
