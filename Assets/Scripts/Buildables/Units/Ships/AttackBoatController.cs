using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class AttackBoatController : ShipController
	{
		private IBarrelWrapper _antiSeaTurret;

        public override float OptimalArmamentRangeInM { get { return _antiSeaTurret.RangeInM; } }
        protected override ISoundKey EngineSoundKey { get { return SoundKeys.Engines.AtatckBoat; } }

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            _antiSeaTurret = gameObject.GetComponentInChildren<IBarrelWrapper>();
            Assert.IsNotNull(_antiSeaTurret);
            turrets.Add(_antiSeaTurret);

            return turrets;
        }

        protected override void OnInitialised()
		{
			base.OnInitialised();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            _antiSeaTurret.Initialise(this, _factoryProvider, enemyFaction, SoundKeys.Firing.BigCannon);
		}
	}
}
