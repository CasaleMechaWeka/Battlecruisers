using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace BattleCruisers.Buildables.Buildings.Tactical
{
	public class ControlTowerController : TacticalBuilding
	{
		private IBoostProvider _boostProvider;

		protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.ControlTower;

		public float boostMultiplier;

		protected override void AddBuildRateBoostProviders(
			GlobalBoostProviders globalBoostProviders,
			IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
		{
			base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
			buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.MastStructureProviders);
		}

		public override void Initialise(IUIManager uiManager)
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
