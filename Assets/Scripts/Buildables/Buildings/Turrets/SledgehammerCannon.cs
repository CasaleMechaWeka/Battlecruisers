using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Localisation;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class SledgehammerCannon : OffenseMultiTurret
    {
        protected override ISoundKey[] FiringSounds => new ISoundKey[]
        {
            SoundKeys.Firing.Missile,
            SoundKeys.Firing.Artillery
        };
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
        }
    }
}