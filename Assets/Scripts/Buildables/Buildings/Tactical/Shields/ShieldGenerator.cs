using BattleCruisers.Buildables.ActivationArgs;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class ShieldGenerator : TacticalBuilding
    {
        private ShieldController _shieldController;

        protected override ISoundKey DeathSoundKey => SoundKeys.Deaths.Building5;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.Shields;
        public override TargetValue TargetValue => TargetValue.Medium;

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders);
        }

        public override void StaticInitialise(HealthBarController healthBar)
        {
            base.StaticInitialise(healthBar);

            _shieldController = GetComponentInChildren<ShieldController>(includeInactive: true);
            Assert.IsNotNull(_shieldController);
            _shieldController.StaticInitialise();
        }

        public override void Activate(BuildingActivationArgs activationArgs)
		{
            base.Activate(activationArgs);

			_shieldController.Initialise(Faction, _cruiserSpecificFactories.BuildableEffectsSoundPlayer);
			_shieldController.gameObject.SetActive(false);

            _localBoosterBoostableGroup.AddBoostable(_shieldController.Stats);
            _localBoosterBoostableGroup.AddBoostProvidersList(_cruiserSpecificFactories.GlobalBoostProviders.ShieldRechargeRateBoostProviders);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_shieldController.gameObject.SetActive(true);
		}
    }
}
