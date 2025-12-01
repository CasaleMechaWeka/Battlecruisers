using UnityEngine;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class AttackBoatController : AnimatedShipController
    {
        protected override Vector2 MaskHighlightableSize
        {
            get
            {
                return new Vector2(
                        base.MaskHighlightableSize.x * 1.5f,
                        base.MaskHighlightableSize.y * 2);
            }
        }
    }
}
