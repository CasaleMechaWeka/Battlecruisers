using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class ArchonBattleshipController : ShipController
    {
        private IBarrelWrapper _directFireAntiSea, _directFireAntiAir1, _directFireAntiAir2, _missileLauncherFront, _missileLauncherRear;

        protected override float EnemyDetectionRangeInM
        {
            get
            {
                // Rear missile launcher and direct fire anti sea will both also be in range.
                return _missileLauncherFront.RangeInM;
            }
        }

        public override void StaticInitialise()
        {
            base.StaticInitialise();
            _attackCapabilities.Add(TargetType.Aircraft);
        }

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

        protected override void OnInitialised()
        {
            base.OnInitialised();

            Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            IList<TargetType> nonAirTargets = new List<TargetType>() { TargetType.Buildings, TargetType.Cruiser, TargetType.Ships };
            _directFireAntiSea.Initialise(_factoryProvider, enemyFaction, nonAirTargets);
            _missileLauncherFront.Initialise(_factoryProvider, enemyFaction, nonAirTargets);
            _missileLauncherRear.Initialise(_factoryProvider, enemyFaction, nonAirTargets);

            IList<TargetType> airTargets = new List<TargetType>() { TargetType.Aircraft };
            _directFireAntiAir1.Initialise(_factoryProvider, enemyFaction, airTargets);
            _directFireAntiAir2.Initialise(_factoryProvider, enemyFaction, airTargets);
        }
    }
}
