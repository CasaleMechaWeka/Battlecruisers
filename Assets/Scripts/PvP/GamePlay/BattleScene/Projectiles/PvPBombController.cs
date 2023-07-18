using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPBombController : PvPProjectileWithTrail<PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        protected override float TrailLifetimeInS => 3;

        public override void Activate(PvPProjectileActivationArgs<IPvPProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);
            MovementController = _factoryProvider.MovementControllerFactory.CreateDummyMovementController();
        }

        protected override void OnImpactCleanUp()
        {
            base.OnImpactCleanUp();
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.gravityScale = 0;
        }


        // Sava added these fields and methods

        private PvPSoundType _type;
        private string _name;
        private Vector3 _pos;

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

        // should be called by client
        private void Awake()
        {
            Initialise();
        }
        protected override void OnActiveClient(Vector3 velocity, float gravityScale)
        {
            OnActiveClientRpc(velocity, gravityScale);
        }

        private async void PlayExplosionSound()
        {
            await PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayer.PlaySoundAsync(new PvPSoundKey(_type, _name), _pos);
        }

        //----------------------------- Rpcs -----------------------------

        [ClientRpc]
        private void OnSetPosition_VisibleClientRpc(Vector3 position, bool visible)
        {
            transform.position = position;
            gameObject.SetActive(visible);
        }
        [ClientRpc]
        private void OnPlayExplosionSoundClientRpc(PvPSoundType type, string name, Vector3 position)
        {
            _type = type;
            _name = name;
            _pos = position;
            Invoke("PlayExplosionSound", 0.05f);
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