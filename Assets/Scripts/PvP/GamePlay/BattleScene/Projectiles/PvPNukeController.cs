using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetProviders;
using Unity.Netcode;
using UnityEngine;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPNukeController :
        PvPProjectileWithTrail<TargetProviderActivationArgs<INukeStats>, INukeStats>,
        ITargetProvider
    {
        private INukeStats _nukeStats;
        private IFlightPointsProvider _flightPointsProvider;

        public ITarget Target { get; private set; }

        public override void Activate(TargetProviderActivationArgs<INukeStats> activationArgs)
        {
            base.Activate(activationArgs);
            _nukeStats = activationArgs.ProjectileStats;
            _flightPointsProvider = _factoryProvider.FlightPointsProviderFactory.NukeFlightPointsProvider;
            Target = activationArgs.Target;
        }

        public void Launch()
        {
            IVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(_nukeStats.MaxVelocityInMPerS);
            ITargetProvider targetProvider = this;

            MovementController
                = _factoryProvider.MovementControllerFactory.CreateRocketMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider,
                    _nukeStats.CruisingAltitudeInM,
                    _flightPointsProvider);

            if (Target.GameObject.GetComponent<PvPBuilding>() != null)
            {
                ulong objectId = (ulong)Target.GameObject.GetComponent<PvPBuilding>()._parent.GetComponent<NetworkObject>().NetworkObjectId;
                OnAddMoveControllerToClientRpc(objectId, _nukeStats.MaxVelocityInMPerS, _nukeStats.CruisingAltitudeInM);
            }
            else if (Target.GameObject.GetComponent<PvPUnit>() != null)
            {
                ulong objectId = (ulong)Target.GameObject.GetComponent<PvPUnit>()._parent.GetComponent<NetworkObject>().NetworkObjectId;
                OnAddMoveControllerToClientRpc(objectId, _nukeStats.MaxVelocityInMPerS, _nukeStats.CruisingAltitudeInM);
            }
            else if (Target.GameObject.GetComponents<PvPCruiser>() != null)
            {
                ulong objectId = (ulong)Target.GameObject.GetComponent<PvPCruiser>().GetComponent<NetworkObject>().NetworkObjectId;
                OnAddMoveControllerToClientRpc(objectId, _nukeStats.MaxVelocityInMPerS, _nukeStats.CruisingAltitudeInM);
            }
        }

        // Sava added these fields and methods
        protected override bool needToTeleport => true;
        protected override float timeToActiveTrail => 0.2f;
        private SoundType _type;
        private string _name;
        private Vector3 _pos;

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                PvPBattleSceneGodClient.Instance.AddNetworkObject(GetComponent<NetworkObject>());
            }
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


        // PlayExplosionSound
        protected override void OnPlayExplosionSound(SoundType type, string name, Vector3 position)
        {
            OnPlayExplosionSoundClientRpc(type, name, position);
        }

        private async void PlayExplosionSound()
        {
            await PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayer.PlaySoundAsync(new BattleCruisers.UI.Sound.SoundKey(_type, _name), _pos);
        }

        private void Awake()
        {
            InitialiseTril();
            _rigidBody = GetComponent<Rigidbody2D>();
            _isActiveAndAlive = false;
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
        private void OnAddMoveControllerToClientRpc(ulong objectID, float MaxVelocityInMPerS, float CruisingAltitudeInM)
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
                    IVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(MaxVelocityInMPerS);
                    ITargetProvider targetProvider = this;
                    MovementController
                        = _factoryProvider.MovementControllerFactory.CreateRocketMovementController(
                            _rigidBody,
                            maxVelocityProvider,
                            targetProvider,
                            CruisingAltitudeInM,
                            _factoryProvider.FlightPointsProviderFactory.NukeFlightPointsProvider);
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
