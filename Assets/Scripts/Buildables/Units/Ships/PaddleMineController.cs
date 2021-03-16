using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class PaddleMineController : AnimatedShipController
    {
        public override float OptimalArmamentRangeInM => 0.5f;

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
