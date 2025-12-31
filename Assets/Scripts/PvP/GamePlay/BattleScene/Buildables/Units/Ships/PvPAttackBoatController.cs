using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPAttackBoatController : PvPAnimatedShipController
    {
        protected override Vector2 MaskHighlightableSize
        {
            get
            {
                return new Vector2(base.MaskHighlightableSize.x * 1.5f,
                                   base.MaskHighlightableSize.y * 2);
            }
        }
    }
}
