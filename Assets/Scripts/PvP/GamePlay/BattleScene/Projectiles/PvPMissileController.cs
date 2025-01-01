using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPMissileController :
        PvPProjectileWithTrail<PvPTargetProviderActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>,
        IPvPTargetProvider
    {
        private IDeferrer _deferrer;
        private IPvPMovementController _dummyMovementController;

        private const float SELF_DETONATION_TIMER = 1.75f;
        private const float SELF_DETONATION_VARIANCE = .5f;

        //---> CODE BY ANUJ
        private PvPRocketTarget _rocketTarget;
        //<---        

        public SpriteRenderer missile;
        protected override float TrailLifetimeInS => 3;
        public ITarget Target { get; private set; }

        public override void Initialise(ILocTable commonStrings, IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(commonStrings, factoryProvider);

            //---> CODE BY ANUJ
            _rocketTarget = GetComponentInChildren<PvPRocketTarget>();
            Assert.IsNotNull(_rocketTarget);
            //<---
            Assert.IsNotNull(missile);
        }

        public override void Activate(PvPTargetProviderActivationArgs<IPvPProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            Logging.Log(Tags.MISSILE, $"Rotation: {transform.rotation.eulerAngles}");

            Target = activationArgs.Target;
            _deferrer = _factoryProvider.DeferrerProvider.Deferrer;

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

            IPvPVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            IPvPTargetProvider targetProvider = this;

            MovementController
                = _factoryProvider.MovementControllerFactory.CreateMissileMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider,
                    _factoryProvider.TargetPositionPredictorFactory);

            _dummyMovementController = _factoryProvider.MovementControllerFactory.CreateDummyMovementController();
            missile.enabled = true;

            //---> CODE BY ANUJ
            _rocketTarget.GameObject.SetActive(true);
            //<---
            SetMissileVisibleClientRpc(true);
            //---> CODE BY ANUJ
            _rocketTarget.Initialise(_commonStrings, activationArgs.Parent.Faction, _rigidBody, this);
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
            if (gameObject.activeSelf)
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
            if (!IsHost)
                _factoryProvider = PvPBattleSceneGodClient.Instance.factoryProvider;
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
            await PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayer.PlaySoundAsync(new SoundKey(_type, _name), _pos);
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
                    IPvPVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(MaxVelocityInMPerS);
                    IPvPTargetProvider targetProvider = this;

                    MovementController
                        = _factoryProvider.MovementControllerFactory.CreateMissileMovementController(
                            _rigidBody,
                            maxVelocityProvider,
                            targetProvider,
                            _factoryProvider.TargetPositionPredictorFactory);

                    _dummyMovementController = _factoryProvider.MovementControllerFactory.CreateDummyMovementController();
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