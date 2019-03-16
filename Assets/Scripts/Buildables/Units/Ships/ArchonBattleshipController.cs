using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class ArchonBattleshipController : ShipController
    {
        private IBarrelWrapper _directFireAntiSea, _directFireAntiAir1, _directFireAntiAir2, _missileLauncherFront, _missileLauncherRear;

        public override bool IsUltra => true;

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_factoryProvider.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        public override float OptimalArmamentRangeInM
        {
            get
            {
                // Rear missile launcher and direct fire anti sea will both also be in range.
                return _missileLauncherFront.RangeInM;
            }
        }

        protected override ISoundKey EngineSoundKey => SoundKeys.Engines.Archon;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            _directFireAntiSea = transform.FindNamedComponent<IBarrelWrapper>("GravityAffectedAntiSea");
            turrets.Add(_directFireAntiSea);

            // Missile launchers
            _missileLauncherFront = transform.FindNamedComponent<IBarrelWrapper>("MissileLauncherFront");
            turrets.Add(_missileLauncherFront);

            _missileLauncherRear = transform.FindNamedComponent<IBarrelWrapper>("MissileLauncherRear");
            turrets.Add(_missileLauncherRear);

            // Anti air
            _directFireAntiAir1 = transform.FindNamedComponent<IBarrelWrapper>("DirectBurstFireAntiAir1");
            turrets.Add(_directFireAntiAir1);

            _directFireAntiAir2 = transform.FindNamedComponent<IBarrelWrapper>("DirectBurstFireAntiAir2");
            turrets.Add(_directFireAntiAir2);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            _directFireAntiSea.Initialise(this, _factoryProvider, enemyFaction, SoundKeys.Firing.BigCannon);
            _missileLauncherFront.Initialise(this, _factoryProvider, enemyFaction);
            _missileLauncherRear.Initialise(this, _factoryProvider, enemyFaction);
            _directFireAntiAir1.Initialise(this, _factoryProvider, enemyFaction, SoundKeys.Firing.AntiAir);
            _directFireAntiAir2.Initialise(this, _factoryProvider, enemyFaction, SoundKeys.Firing.AntiAir);
        }
    }
}
