using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.DataStrctures;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class ShieldGenerator : TacticalBuilding
    {
        private ShieldController _shieldController;

        protected override ISoundKey DeathSoundKey { get { return SoundKeys.Deaths.Building5; } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.Shields; } }
        public override TargetValue TargetValue { get { return TargetValue.Medium; } }

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<IObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_factoryProvider.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders);
        }

        protected override void OnStaticInitialised()
        {
            base.OnStaticInitialised();

            _shieldController = GetComponentInChildren<ShieldController>(includeInactive: true);
            Assert.IsNotNull(_shieldController);
            _shieldController.StaticInitialise();
        }

		protected override void OnInitialised()
		{
			base.OnInitialised();

			_shieldController.Initialise(Faction, _factoryProvider.Sound.BuildableEffectsSoundPlayer);
			_shieldController.gameObject.SetActive(false);

            _localBoosterBoostableGroup.AddBoostable(_shieldController.Stats);
            _localBoosterBoostableGroup.AddBoostProvidersList(_factoryProvider.GlobalBoostProviders.ShieldRechargeRateBoostProviders);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_shieldController.gameObject.SetActive(true);
		}
    }
}
