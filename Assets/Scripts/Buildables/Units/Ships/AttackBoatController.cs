using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class AttackBoatController : ShipController
	{
		private IBarrelWrapper _antiSeaTurret;
        public GameObject pistonAnimation;

        public override float OptimalArmamentRangeInM => _antiSeaTurret.RangeInM;
        protected override ISoundKey EngineSoundKey => SoundKeys.Engines.AtatckBoat;

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

        public override void StaticInitialise()
        {
            base.StaticInitialise();
            Assert.IsNotNull(pistonAnimation);
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
            _antiSeaTurret.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction, SoundKeys.Firing.BigCannon);
		}

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            pistonAnimation.SetActive(true);
        }

        protected override void Deactivate()
        {
            base.Deactivate();
            pistonAnimation.SetActive(false);
        }
    }
}
