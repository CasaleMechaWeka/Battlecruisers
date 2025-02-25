using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Utils.BattleScene.Pools;
using Unity.Netcode;
using UnityEngine;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions
{
    public class PvPExplosionController : PvPParticleSystemGroupInitialiser
    {

        private PvPExplosion _explosion;
        private AudioSource _audioSource;
        public virtual IPoolable<Vector3> Initialise()
        {
            _explosion = new PvPExplosion(
                    this,
                    GetParticleSystems(),
                    GetSynchronizedSystems());

            return _explosion;
        }

        protected override void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
            if (_audioSource is not null)
                _audioSource.playOnAwake = false;
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
            if (!IsHost)
                Position = position;
        }

        [ClientRpc]
        private void SetVisibleClientRpc(bool isVisible)
        {
            if (!IsHost)
            {
                IsVisible = isVisible;
            }
            if (isVisible)
            {
                _explosion.Play();
                if (_audioSource is not null)
                    _audioSource.Play();
            }
        }
    }
}
