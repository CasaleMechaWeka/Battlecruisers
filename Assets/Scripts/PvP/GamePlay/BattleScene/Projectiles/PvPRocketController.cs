using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Projectiles.FlightPoints;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Projectiles;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Movement.Velocity.Homing;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    /// <summary>
    /// The RocketController wants the behaviour of both:
    /// 1. ProjectileController
    /// 2. Target
    /// But it can only subclass one of these.  Hence subclass ProjectileController, and
    /// have a child game object deriving of Target, to get both behaviours.
    /// </summary>
    public class PvPRocketController : PvPProjectileWithTrail, ITargetProvider
    {
        private PvPRocketTarget _rocketTarget;

        public ITarget Target { get; private set; }

        public GameObject rocketSprite; //for making more complicated rocket sprites disappear on detonation


        public override void Initialise()
        {
            base.Initialise();

            _rocketTarget = GetComponentInChildren<PvPRocketTarget>();
            Assert.IsNotNull(_rocketTarget);

            if (rocketSprite != null)
            {
                rocketSprite.SetActive(true); // Enable the sprite on initialization
            }
        }

        public override void Activate(ProjectileActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            Target = activationArgs.Target;
            if (Target.GameObject.GetComponent<PvPBuilding>() != null)
            {
                ulong objectId = (ulong)Target.GameObject.GetComponent<PvPBuilding>()._parent.GetComponent<NetworkObject>().NetworkObjectId;
                OnAddMoveControllerToClientRpc(objectId, activationArgs.ProjectileStats.MaxVelocityInMPerS, activationArgs.ProjectileStats.IsAccurate, activationArgs.ProjectileStats.CruisingAltitudeInM);
            }
            else if (Target.GameObject.GetComponent<PvPUnit>() != null)
            {
                ulong objectId = (ulong)Target.GameObject.GetComponent<PvPUnit>()._parent.GetComponent<NetworkObject>().NetworkObjectId;
                OnAddMoveControllerToClientRpc(objectId, activationArgs.ProjectileStats.MaxVelocityInMPerS, activationArgs.ProjectileStats.IsAccurate, activationArgs.ProjectileStats.CruisingAltitudeInM);
            }
            else if (Target.GameObject.GetComponents<PvPCruiser>() != null)
            {
                ulong objectId = (ulong)Target.GameObject.GetComponent<PvPCruiser>().GetComponent<NetworkObject>().NetworkObjectId;
                OnAddMoveControllerToClientRpc(objectId, activationArgs.ProjectileStats.MaxVelocityInMPerS, activationArgs.ProjectileStats.IsAccurate, activationArgs.ProjectileStats.CruisingAltitudeInM);
            }

            IVelocityProvider maxVelocityProvider = new StaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            ITargetProvider targetProvider = this;
            IFlightPointsProvider flightPointsProvider
                = activationArgs.ProjectileStats.IsAccurate ?
                    PvPFactoryProvider.FlightPointsProviderFactory.RocketFlightPointsProvider :
                    PvPFactoryProvider.FlightPointsProviderFactory.InaccurateRocketFlightPointsProvider;

            MovementController
                = new RocketMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider,
                    activationArgs.ProjectileStats.CruisingAltitudeInM,
                    flightPointsProvider);

            _rocketTarget.GameObject.SetActive(true);
            isVisible = true;
            timeStamp = Time.time;
            SetRocketVisibleClientRpc(true);
            _rocketTarget.Initialise(activationArgs.Parent.Faction, _rigidBody, this);

            if (rocketSprite != null)
            {
                rocketSprite.SetActive(true);
            }
        }

        protected override void OnActiveClient(Vector3 velocity, float gravityScale, bool isAlive)
        {
            OnActiveClientRpc(velocity, gravityScale, isAlive);
        }

        /*
        protected override void DestroyProjectile()
        {
            enabled = false;
            _rocketTarget.GameObject.SetActive(false);
            if (IsHost)
                SetRocketVisibleClientRpc(false);
            base.DestroyProjectile();
            DestroyProjectileClientRpc();
        }
        */

        protected override void OnImpactCleanUp()
        {
            base.OnImpactCleanUp();
            _rocketTarget.GameObject.SetActive(false);
            SetRocketVisibleClientRpc(false);

            if (rocketSprite != null)
            {
                rocketSprite.SetActive(false);
            }

            isVisible = false;
            timeStamp = Time.time;
        }

        // Sava added these fields and methods
        protected override bool needToTeleport => true;
        protected override float timeToActiveTrail => 0.2f;
        private SoundType _type;
        private string _name;
        private Vector3 _pos;
        private bool isVisible = false;
        private float timeStamp = 0f;

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                PvPBattleSceneGodClient.Instance.AddNetworkObject(GetComponent<NetworkObject>());
            }
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
            await SoundPlayer.PlaySoundAsync(new SoundKey(_type, _name), _pos);
        }

        private void Update()
        {
            if (isVisible)
            {
                if (Time.time - timeStamp > 15f)
                    _rocketTarget.GameObject.SetActive(false);
            }
        }
        // should be called by client

        private void Awake()
        {
            InitialiseTril();
            // rocket
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
        private void OnActiveClientRpc(Vector2 velocity, float gravityScale, bool isAlive)
        {
            if (!IsHost)
            {
                _rigidBody.velocity = velocity;
                _rigidBody.gravityScale = gravityScale;
                _isActiveAndAlive = isAlive;

                if (_rigidBody.velocity != Vector2.zero)
                {
                    transform.right = _rigidBody.velocity;
                }
                /*                if (velocity == Vector2.zero && gravityScale == 0f)
                                    base.OnImpactCleanUp();*/
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
        private void OnAddMoveControllerToClientRpc(ulong objectID, float MaxVelocityInMPerS, bool IsAccurate, float CruisingAltitudeInM)
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
                    ITargetProvider targetProvider = this;
                    IVelocityProvider maxVelocityProvider = new StaticVelocityProvider(MaxVelocityInMPerS);

                    IFlightPointsProvider flightPointsProvider
                        = IsAccurate ?
                            PvPFactoryProvider.FlightPointsProviderFactory.RocketFlightPointsProvider :
                            PvPFactoryProvider.FlightPointsProviderFactory.InaccurateRocketFlightPointsProvider;
                    MovementController
                        = new RocketMovementController(
                            _rigidBody,
                            maxVelocityProvider,
                            targetProvider,
                            CruisingAltitudeInM,
                            flightPointsProvider);
                }
            }
        }

        [ClientRpc]
        private void OnSetPosition_VisibleClientRpc(Vector3 position, bool visible)
        {
            transform.position = position;
            if (!visible)
                gameObject.SetActive(false);
            else
                Invoke("iSetActive", timeToActiveTrail);
        }


        private void iSetActive()
        {
            gameObject.SetActive(true);
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
        [ClientRpc]
        private void OnPlayExplosionSoundClientRpc(SoundType type, string name, Vector3 position)
        {
            _type = type;
            _name = name;
            _pos = position;
            Invoke("PlayExplosionSound", 0.05f);
        }

        // rocket
        [ClientRpc]
        private void SetRocketVisibleClientRpc(bool visible)
        {
            if (!IsHost)
            {
                _rocketTarget.gameObject.SetActive(visible);
                if (visible)
                {
                    isVisible = true;
                    timeStamp = Time.time;
                }
                else
                {
                    isVisible = false;
                    timeStamp = Time.time;
                }
            }
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