using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class AttackBoatController : ShipController
	{
		private IBarrelWrapper _directFireAntiSea;

        public override float Damage { get { return _directFireAntiSea.TurretStats.DamagePerS; } }
        protected override float EnemyDetectionRangeInM { get { return _directFireAntiSea.TurretStats.rangeInM; } }

        public override void StaticInitialise()
		{
			base.StaticInitialise();

            _directFireAntiSea = gameObject.GetComponentInChildren<IBarrelWrapper>();
            Assert.IsNotNull(_directFireAntiSea);
            _directFireAntiSea.StaticInitialise();
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            _directFireAntiSea.Initialise(_factoryProvider, enemyFaction, _attackCapabilities);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();
            _directFireAntiSea.StartAttackingTargets();
		}
	}
}
