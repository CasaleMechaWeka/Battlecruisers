using BattleCruisers.Buildables.Boost;
using BattleCruisers.UI.BattleScene.Manager;
namespace BattleCruisers.Buildables.Buildings.Tactical
{
	public class ControlTowerController : TacticalBuilding
	{
		private IBoostProvider _boostProvider;


		public float boostMultiplier;

		public override void Initialise(UIManager uiManager)
		{
			base.Initialise(uiManager);
			_boostProvider = new BoostProvider(boostMultiplier);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();
			_cruiserSpecificFactories.GlobalBoostProviders.AircraftBoostProviders.Add(_boostProvider);
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			_cruiserSpecificFactories.GlobalBoostProviders.AircraftBoostProviders.Remove(_boostProvider);
		}
	}
}
