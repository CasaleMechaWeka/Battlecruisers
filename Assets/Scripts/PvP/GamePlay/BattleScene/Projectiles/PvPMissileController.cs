using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Movement.Velocity.Homing;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPMissileController :
        PvPProjectileWithTrail<TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>,
        ITargetProvider
    {
        private IDeferrer _deferrer;
        private IMovementController _dummyMovementController;

        private const float SELF_DETONATION_TIMER = 1.75f;
        private const float SELF_DETONATION_VARIANCE = .5f;

        //---> CODE BY ANUJ
        private PvPRocketTarget _rocketTarget;
        //<---        

        public SpriteRenderer missile;
        protected override float TrailLifetimeInS => 3;
        public ITarget Target { get; private set; }

        public override void Initialise()
        {
            base.Initialise();

            //---> CODE BY ANUJ
            _rocketTarget = GetComponentInChildren<PvPRocketTarget>();
            Assert.IsNotNull(_rocketTarget);
            //<---
            Assert.IsNotNull(missile);
        }

        public override void Activate(TargetProviderActivationArgs<ProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            //Debug.Log("[PvPMissileController] Activate() called. Rotation: " + transform.rotation.eulerAngles + ", Target: " + activationArgs.Target);

            Target = activationArgs.Target;
            _deferrer = PvPFactoryProvider.DeferrerProvider.Deferrer;

            if (Target.GameObject.GetComponent<PvPBuilding>() != null)
            {
                ulong objectId = (ulong)Target.GameObject.GetComponent<PvPBuilding>()._parent.GetComponent<NetworkObject>().NetworkObjectId;
                OnAddMoveControllerToClientRpc(objectId, activationArgs.ProjectileStats.MaxVelocityInMPerS);
            }
            else if (Target.GameObject.GetComponent<PvPUnit>() != null)
            {
                ulong objectId = (ulong)Target.GameObject.GetComponent<PvPUnit>()._parent.GetComponent<NetworkObject>().NetworkObjectId;
                OnAddMoveControllerToClientRpc(objectId, activationArgs.ProjectileStats.MaxVelocityInMPerS);
            }

            IVelocityProvider maxVelocityProvider = new StaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            ITargetProvider targetProvider = this;

            MovementController
                = new MissileMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider);

            //Debug.Log("[PvPMissileController] Movement controller created. Missile sprite enabled: " + missile.enabled);

            _dummyMovementController = new DummyMovementController();
            missile.enabled = true;

            //---> CODE BY ANUJ
            _rocketTarget.GameObject.SetActive(true);
            //<---
            SetMissileVisibleClientRpc(true);
            //---> CODE BY ANUJ
            _rocketTarget.Initialise(activationArgs.Parent.Faction, _rigidBody, this);
            //<---
            activationArgs.Target.Destroyed += Target_Destroyed;
        }

        private void Target_Destroyed(object sender, DestroyedEventArgs e)
        {
            // Let missile keep current velocity
            MovementController = _dummyMovementController;
            // Destroy missile eventually (in case it does not hit a matching target)
            _deferrer.Defer(ConditionalDestroy, Random.Range(SELF_DETONATION_TIMER, SELF_DETONATION_TIMER + SELF_DETONATION_VARIANCE));
            OnTargetDestroyedClientRpc();
        }

        private void ConditionalDestroy()
        {
            if (gameObject != null && gameObject.activeSelf)
            {
                DestroyProjectile();
            }
        }

        protected override void DestroyProjectile()
        {
            missile.enabled = false;
            //---> CODE BY ANUJ
            _rocketTarget.GameObject.SetActive(false);
            //<---
            if (IsHost)
                SetMissileVisibleClientRpc(false);
            Target.Destroyed -= Target_Destroyed;
            base.DestroyProjectile();
            DestroyProjectileClientRpc();
        }


        // Sava added these fields and methods

        protected override float timeToActiveTrail => 0.2f;
        protected override bool needToTeleport => true;
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

        protected override void OnActiveClient_PositionVisible(Vector3 velocity, float gravityScale, bool isAlive, Vector3 position, bool visible)
        {
            OnActiveClient_PositionVisibleClientRpc(velocity, gravityScale, isAlive, position, visible);
        }

        // PlayExplosionSound
        protected override void OnPlayExplosionSound(SoundType type, string name, Vector3 position)
        {
            OnPlayExplosionSoundClientRpc(type, name, position);
        }

        private async void PlayExplosionSound()
        {
            Debug.Log("[PvPMissileController] PlayExplosionSound invoked with Type: " + _type + ", Name: " + _name + ", Position: " + _pos);
            await SoundPlayer.PlaySoundAsync(new SoundKey(_type, _name), _pos);
        }

        // should be called by client

        private void Awake()
        {
            InitialiseTril();
            _rocketTarget = GetComponentInChildren<PvPRocketTarget>();
            Assert.IsNotNull(_rocketTarget);
            _rigidBody = GetComponent<Rigidbody2D>();
            _isActiveAndAlive = false;
        }

        protected override void HideEffectsOfClient()
        {
            if (IsServer)
                HideEffectsOfClientRpc();
            else
                base.HideEffectsOfClient();
        }

        protected override void ShowAllEffectsOfClient()
        {
            if (IsServer)
                ShowAllEffectsOfClientRpc();
            else
                base.ShowAllEffectsOfClient();
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
        [ClientRpc]
        private void DestroyProjectileClientRpc()
        {
            if (!IsHost)
            {
                _rigidBody.velocity = Vector2.zero;
                MovementController = null;
            }
        }

        [ClientRpc]
        private void OnTargetDestroyedClientRpc()
        {
            MovementController = _dummyMovementController;
            //    _factoryProvider.DeferrerProvider.Deferrer.Defer(ConditionalDestroy, MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S);
        }
        [ClientRpc]
        private void OnAddMoveControllerToClientRpc(ulong objectID, float MaxVelocityInMPerS)
        {
            if (!IsHost)
            {
                Debug.Log("[PvPMissileController] OnAddMoveControllerToClientRpc called with objectID: " + objectID + ", MaxVelocity: " + MaxVelocityInMPerS);
                NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(objectID);
                if (obj != null)
                {
                    ITarget target = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPBuilding>>()?.Buildable?.Parse<ITarget>();
                    if (target == null)
                    {
                        target = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPUnit>>()?.Buildable?.Parse<ITarget>();
                    }
                    if (target == null)
                    {
                        target = obj.gameObject.GetComponent<PvPCruiser>()?.Parse<ITarget>();
                    }
                    Target = target;
                    IVelocityProvider maxVelocityProvider = new StaticVelocityProvider(MaxVelocityInMPerS);
                    ITargetProvider targetProvider = this;

                    MovementController
                        = new MissileMovementController(
                            _rigidBody,
                            maxVelocityProvider,
                            targetProvider);

                    _dummyMovementController = new DummyMovementController();
                }
            }
        }

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
            if (!visible)
                gameObject.SetActive(false);
            else
                Invoke("iSetActive", timeToActiveTrail);
        }

        private void iSetActive()
        {
            gameObject.SetActive(true);
        }

        [ClientRpc]
        private void OnPlayExplosionSoundClientRpc(SoundType type, string name, Vector3 position)
        {
            _type = type;
            _name = name;
            _pos = position;
            Invoke("PlayExplosionSound", 0.05f);
        }

        // missile
        [ClientRpc]
        private void SetMissileVisibleClientRpc(bool visible)
        {
            missile.enabled = visible;
            _rocketTarget.GameObject.SetActive(visible);
            if (!visible)
                base.HideEffectsOfClient();
            //    Target.Destroyed -= Target_Destroyed;
        }

        [ClientRpc]
        protected void HideEffectsOfClientRpc()
        {
            if (!IsHost)
                HideEffectsOfClient();
        }

        [ClientRpc]
        protected void ShowAllEffectsOfClientRpc()
        {
            if (!IsHost)
                ShowAllEffectsOfClient();
        }
    }
}