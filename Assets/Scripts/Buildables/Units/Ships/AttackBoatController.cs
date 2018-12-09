using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class AttackBoatController : ShipController
	{
		private IBarrelWrapper _antiSeaTurret;

        public override float OptimalArmamentRangeInM { get { return _antiSeaTurret.RangeInM; } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Units.AttackBoat; } }
        protected override ISoundKey EngineSoundKey { get { return SoundKeys.Engines.AtatckBoat; } }

        protected override Vector2 MaskHighlightableSize
        {
            get
            {
                return
                    new Vector2(
                        base.MaskHighlightableSize.x * 1.5f,
                        base.MaskHighlightableSize.y * 2);
            }
        }

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            _antiSeaTurret = gameObject.GetComponentInChildren<IBarrelWrapper>();
            Assert.IsNotNull(_antiSeaTurret);
            turrets.Add(_antiSeaTurret);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            _antiSeaTurret.Initialise(this, _factoryProvider, enemyFaction, SoundKeys.Firing.BigCannon);
		}
	}
}
