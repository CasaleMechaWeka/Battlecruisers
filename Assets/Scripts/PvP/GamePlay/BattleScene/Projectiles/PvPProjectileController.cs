using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.UI.Sound;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPProjectileController : PvPProjectileControllerBase<PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        private SoundType _type;
        private string _name;
        private Vector3 _pos;

        // Set Position
        protected override void OnSetPosition_Visible(Vector3 position, bool visible)
        {
            OnSetPosition_VisibleClientRpc(position, visible);
        }
        // PlayExplosionSound
        protected override void OnPlayExplosionSound(SoundType type, string name, Vector3 position)
        {
            OnPlayExplosionSoundClientRpc(type, name, position);
        }
        private void Awake()
        {
            Initialise();
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
                PvPBattleSceneGodClient.Instance.AddNetworkObject(GetComponent<NetworkObject>());
        }
        public override void OnNetworkDespawn()
        {
            if (IsClient)
                PvPBattleSceneGodClient.Instance.RemoveNetworkObject(GetComponent<NetworkObject>());
        }
        protected override void OnActiveClient(Vector3 velocity, float gravityScale, bool isAlive)
        {
            OnActiveClientRpc(velocity, gravityScale, isAlive);
        }

        protected override void OnActiveClient_PositionVisible(Vector3 velocity, float gravityScale, bool isAlive, Vector3 position, bool visible)
        {
            OnActiveClient_PositionVisibleClientRpc(velocity, gravityScale, isAlive, position, visible);
        }

        [ClientRpc]
        private void OnActiveClient_PositionVisibleClientRpc(Vector3 velocity, float gravityScale, bool isAlive, Vector3 position, bool visible)
        {
            if (!IsHost)
            {
                transform.position = position;
                gameObject.SetActive(visible);
                _rigidBody.velocity = velocity;
                _rigidBody.gravityScale = gravityScale;
                _isActiveAndAlive = isAlive;
            }
        }

        [ClientRpc]
        private void OnSetPosition_VisibleClientRpc(Vector3 position, bool visible)
        {
            if (!IsHost)
            {
                transform.position = position;
                gameObject.SetActive(visible);
            }
        }

        [ClientRpc]
        private void OnActiveClientRpc(Vector3 velocity, float gravityScale, bool isAlive)
        {
            if (!IsHost)
            {
                _rigidBody.velocity = velocity;
                _rigidBody.gravityScale = gravityScale;
                _isActiveAndAlive = isAlive;
            }
        }

        [ClientRpc]
        private void OnPlayExplosionSoundClientRpc(SoundType type, string name, Vector3 position)
        {
            _type = type;
            _name = name;
            _pos = position;
            Invoke("PlayExplosionSound", 0.05f);
        }

        private async void PlayExplosionSound()
        {
            await PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayer.PlaySoundAsync(new SoundKey(_type, _name), _pos);
        }

    }
}