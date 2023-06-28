using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using Unity.Netcode;
using UnityEngine;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions
{
    public class PvPExplosionController : PvPParticleSystemGroupInitialiser
    {

        private PvPExplosion _explosion;
        public virtual IPvPExplosion Initialise()
        {
            _explosion = new PvPExplosion(
                    this,
                    GetParticleSystems(),
                    GetSynchronizedSystems());

            return _explosion;
        }

        protected override void Awake()
        {
            base.Awake();
            _explosion = new PvPExplosion(
                       this,
                       GetParticleSystems(),
                       GetSynchronizedSystems());
        }

        protected override void CallRpc_SetPosition(Vector3 position)
        {
            SetPositionClientRpc(position);
        }

        protected override void CallRpc_SetVisible(bool isVisible)
        {
            SetVisibleClientRpc(isVisible);
        }

        [ClientRpc]
        private void SetPositionClientRpc(Vector3 position)
        {
            Position = position;

        }

        [ClientRpc]
        private void SetVisibleClientRpc(bool isVisible)
        {
            IsVisible = isVisible;
            if (IsVisible)
                _explosion.Play();
        }
    }

}
