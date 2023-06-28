using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPProjectileController : PvPProjectileControllerBase<PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        // empty
        protected override void OnSetPosition_Visible(Vector3 position, bool visible)
        {
            OnSetPosition_VisibleClientRpc(position, visible);
        }



        private void Awake()
        {
            Initialise();            
        }

        protected override void OnActiveClient(Vector3 velocity, float gravityScale)
        {
            OnActiveClientRpc(velocity, gravityScale);
        }

        [ClientRpc]
        private void OnSetPosition_VisibleClientRpc(Vector3 position, bool visible)
        {
            transform.position = position;
            gameObject.SetActive(visible);
        }

        [ClientRpc]
        private void OnActiveClientRpc(Vector3 velocity, float gravityScale)
        {
            _rigidBody.velocity = velocity;
            _rigidBody.gravityScale = gravityScale;
            _isActiveAndAlive = true;
        }

    }
}