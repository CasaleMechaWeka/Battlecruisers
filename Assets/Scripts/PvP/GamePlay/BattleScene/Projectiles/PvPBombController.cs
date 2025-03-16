using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.UI.Sound;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPBombController : PvPProjectileWithTrail<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        protected override float TrailLifetimeInS => 3;

        public override void Activate(ProjectileActivationArgs<IProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);
            MovementController = _factoryProvider.MovementControllerFactory.CreateDummyMovementController();
        }

        protected override void OnImpactCleanUp()
        {
            base.OnImpactCleanUp();
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.gravityScale = 0;
            OnActiveClient(Vector2.zero, 0f, false);
        }


        // Sava added these fields and methods

        private SoundType _type;
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
        protected override void OnPlayExplosionSound(SoundType type, string name, Vector3 position)
        {
            OnPlayExplosionSoundClientRpc(type, name, position);
        }

        // should be called by client
        private void Awake()
        {
            Initialise();
            InitialiseTril();
        }
        protected override void OnActiveClient(Vector3 velocity, float gravityScale, bool isAlive)
        {
            OnActiveClientRpc(velocity, gravityScale, isAlive);
        }

        private async void PlayExplosionSound()
        {
            await PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayer.PlaySoundAsync(new BattleCruisers.UI.Sound.SoundKey(_type, _name), _pos);
        }


        protected override void HideEffectsOfClient()
        {
            if (IsClient)
                base.HideEffectsOfClient();
            else
                HideEffectsOfClientRpc();
        }

        protected override void ShowAllEffectsOfClient()
        {
            if (IsClient)
                base.ShowAllEffectsOfClient();
            else
                ShowAllEffectsOfClientRpc();
        }

        protected override void OnActiveClient_PositionVisible(Vector3 velocity, float gravityScale, bool isAlive, Vector3 position, bool visible)
        {
            OnActiveClient_PositionVisibleClientRpc(velocity, gravityScale, isAlive, position, visible);
        }
        private void iSetActive_Rigidbody()
        {
            gameObject.SetActive(true);
            _rigidBody.velocity = temp_velocity;
            _rigidBody.gravityScale = temp_gravityScale;
            _isActiveAndAlive = temp_isAlive;

            if (_rigidBody.velocity != Vector2.zero)
            {
                transform.right = _rigidBody.velocity;
            }
            /*                if (velocity == Vector2.zero && gravityScale == 0f)
                                base.OnImpactCleanUp();*/
        }
        //----------------------------- Rpcs -----------------------------

        Vector3 temp_velocity;
        float temp_gravityScale;
        bool temp_isAlive;
        [ClientRpc]
        private void OnActiveClient_PositionVisibleClientRpc(Vector3 velocity, float gravityScale, bool isAlive, Vector3 position, bool visible)
        {
            if (!IsHost)
            {
                transform.position = position;
                if (!visible)
                    gameObject.SetActive(false);
                else
                {
                    temp_velocity = velocity;
                    temp_gravityScale = gravityScale;
                    temp_isAlive = isAlive;
                    Invoke("iSetActive_Rigidbody", timeToActiveTrail);
                }
            }
        }
        [ClientRpc]
        private void OnSetPosition_VisibleClientRpc(Vector3 position, bool visible)
        {
            transform.position = position;
            gameObject.SetActive(visible);
        }
        [ClientRpc]
        private void OnPlayExplosionSoundClientRpc(SoundType type, string name, Vector3 position)
        {
            _type = type;
            _name = name;
            _pos = position;
            Invoke("PlayExplosionSound", 0.05f);
        }

        [ClientRpc]
        private void OnActiveClientRpc(Vector2 velocity, float gravityScale, bool isAlive)
        {
            if (!IsHost)
            {
                _rigidBody.velocity = velocity;
                _rigidBody.gravityScale = gravityScale;
                _isActiveAndAlive = isAlive;
                if (velocity == Vector2.zero && gravityScale == 0f)
                    base.OnImpactCleanUp();
            }
        }

        [ClientRpc]
        protected void HideEffectsOfClientRpc()
        {
            HideEffectsOfClient();
        }

        [ClientRpc]
        protected void ShowAllEffectsOfClientRpc()
        {
            ShowAllEffectsOfClient();
        }
    }
}