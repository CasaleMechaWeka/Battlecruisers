using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPProjectileController : PvPProjectileControllerBase<PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        private PvPSoundType _type;
        private string _name;
        private Vector3 _pos;

        // Set Position
        protected override void OnSetPosition_Visible(Vector3 position, bool visible)
        {
            OnSetPosition_VisibleClientRpc(position, visible);
        }
        // PlayExplosionSound
        protected override void OnPlayExplosionSound(PvPSoundType type, string name, Vector3 position)
        {
            OnPlayExplosionSoundClientRpc(type, name, position);
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

        [ClientRpc]
        private void OnPlayExplosionSoundClientRpc(PvPSoundType type, string name, Vector3 position)
        {
            _type = type;
            _name = name;
            _pos = position;
            Invoke("PlayExplosionSound", 0.05f);
        }

        private async void PlayExplosionSound()
        {
            await PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayer.PlaySoundAsync(new PvPSoundKey(_type, _name), _pos);
        }

    }
}