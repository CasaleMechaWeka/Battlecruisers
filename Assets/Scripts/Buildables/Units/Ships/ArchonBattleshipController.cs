using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class ArchonBattleshipController : ShipController
    {
        private IBarrelWrapper _directFireAntiSea, _directFireAntiAir1, _directFireAntiAir2, _missileLauncherFront, _missileLauncherRear;

        protected override float EnemyDetectionRangeInM
        {
            get
            {
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

            // Anti ship turret
            _directFireAntiSea = transform.Find("DirectFireAntiSea").gameObject.GetComponent<IBarrelWrapper>();
            Assert.IsNotNull(_directFireAntiSea);
            turrets.Add(_directFireAntiSea);

            // Missile launcher front
            _missileLauncherFront = transform.Find("MissileLauncherFront").gameObject.GetComponent<IBarrelWrapper>();
            Assert.IsNotNull(_missileLauncherFront);
            turrets.Add(_missileLauncherFront);

            // Missile launcher rear
            _missileLauncherRear = transform.Find("MissileLauncherRear").gameObject.GetComponent<IBarrelWrapper>();
            Assert.IsNotNull(_missileLauncherRear);
            turrets.Add(_missileLauncherRear);

            // Anti air turret 1
			_directFireAntiAir1 = transform.Find("DirectBurstFireAntiAir1").gameObject.GetComponent<IBarrelWrapper>();
			Assert.IsNotNull(_directFireAntiAir1);
			turrets.Add(_directFireAntiAir1);

            // Anti air turret 2
            _directFireAntiAir2 = transform.Find("DirectBurstFireAntiAir2").gameObject.GetComponent<IBarrelWrapper>();
            Assert.IsNotNull(_directFireAntiAir2);
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
