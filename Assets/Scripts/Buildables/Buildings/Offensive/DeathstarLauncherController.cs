using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
    public class DeathstarLauncherController : SatelliteLauncherController
	{
		protected override Vector3 SpawnPositionAdjustment => new Vector3(0.003f, 0.21f, 0);
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;
        public override TargetValue TargetValue => TargetValue.High;

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_factoryProvider.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        protected override void OnStaticInitialised()
        {
            base.OnStaticInitialised();

            // Need satellite to be initialised to be able to access damage capabilities.
            satellitePrefab.Initialise();

            foreach (IDamageCapability damageCapability in satellitePrefab.Buildable.DamageCapabilities)
            {
                AddDamageStats(damageCapability);
            }
        }
	}
}
