using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class AttackBoatController : ShipController
	{
		private IBarrelWrapper _directFireAntiSea;

        protected override float EnemyDetectionRangeInM { get { return _directFireAntiSea.TurretStats.rangeInM; } }

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

			_directFireAntiSea = gameObject.GetComponentInChildren<IBarrelWrapper>();
			Assert.IsNotNull(_directFireAntiSea);
			turrets.Add(_directFireAntiSea);

            return turrets;
        }

        protected override void OnInitialised()
		{
			base.OnInitialised();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            _directFireAntiSea.Initialise(_factoryProvider, enemyFaction, _attackCapabilities);
		}
	}
}
