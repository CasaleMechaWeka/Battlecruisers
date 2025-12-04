using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPAttackBoatController : PvPAnimatedShipController
    {
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

        protected override void StartMovementEffectsOfClient()
        {
            if (!IsHost)
                base.StartMovementEffectsOfClient();
            else
                StartMovementEffectsClientRpc();

        }

        protected override void StopMovementEffectsOfClient()
        {
            if (!IsHost)
                base.StopMovementEffectsOfClient();
            else
                StopMovementEffectsClientRpc();
        }

        protected override void ResetAndHideOfClient()
        {
            if (!IsHost)
                base.ResetAndHideOfClient();
            else
                ResetHideClientRpc();
        }

        //-------------------------------------- RPCs -------------------------------------------------//


        [ClientRpc]
        private void StartMovementEffectsClientRpc()
        {
            if (!IsHost)
                StartMovementEffectsOfClient();
        }
        [ClientRpc]
        private void StopMovementEffectsClientRpc()
        {
            if (!IsHost)
                StopMovementEffectsOfClient();
        }

        [ClientRpc]
        private void ResetHideClientRpc()
        {
            if (!IsHost)
                ResetAndHideOfClient();
        }
    }
}
