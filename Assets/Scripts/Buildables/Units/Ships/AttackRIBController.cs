using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class AttackRIBController : AnimatedShipController
    {
		public BarrelWrapper ak1, ak2;

        public override float OptimalArmamentRangeInM => ak1.RangeInM - 6;



        /*protected override Vector2 MaskHighlightableSize
        {
            get
            {
                return
                    new Vector2(
                        base.MaskHighlightableSize.x * 1.5f,
                        base.MaskHighlightableSize.y * 2);
            }
        }*/

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();
            turrets.Add(ak1);
            turrets.Add(ak2);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            AddExtraFriendDetectionRange(1);
            ak1.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.AttackBoat);
            ak2.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.AttackBoat);
		}

        protected override List<SpriteRenderer> GetNonTurretRenderers()
        {
            List<SpriteRenderer> renderers = base.GetNonTurretRenderers();

            Transform pistonsParent = transform.FindNamedComponent<Transform>("Pistons");
            SpriteRenderer[] pistonRenderers = pistonsParent.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
            renderers.AddRange(pistonRenderers);

            return renderers;
        }
    }
}
