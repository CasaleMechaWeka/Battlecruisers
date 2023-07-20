using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    /// <summary>
    /// The RocketController wants the behaviour of both:
    /// 1. ProjectileController
    /// 2. Target
    /// But it can only subclass one of these.  Hence subclass ProjectileController, and
    /// have a child game object deriving of Target, to get both behaviours.
    /// </summary>
    public class PvPRocketController :
        PvPProjectileWithTrail<PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>, IPvPCruisingProjectileStats>,
        IPvPTargetProvider
    {
        private PvPRocketTarget _rocketTarget;

        public IPvPTarget Target { get; private set; }

        public override void Initialise(ILocTable commonStrings, IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(commonStrings, factoryProvider);

            _rocketTarget = GetComponentInChildren<PvPRocketTarget>();
            Assert.IsNotNull(_rocketTarget);
        }

        public override void Activate(PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            Target = activationArgs.Target;

            IPvPVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            IPvPTargetProvider targetProvider = this;
            IPvPFlightPointsProvider flightPointsProvider
                = activationArgs.ProjectileStats.IsAccurate ?
                    _factoryProvider.FlightPointsProviderFactory.RocketFlightPointsProvider :
                    _factoryProvider.FlightPointsProviderFactory.InaccurateRocketFlightPointsProvider;

            MovementController
                = _factoryProvider.MovementControllerFactory.CreateRocketMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider,
                    activationArgs.ProjectileStats.CruisingAltitudeInM,
                    flightPointsProvider);

            _rocketTarget.GameObject.SetActive(true);
            SetRocketTargetVisibleClientRpc(true);
            _rocketTarget.Initialise(_commonStrings, activationArgs.Parent.Faction, _rigidBody, this);
        }

        protected override void OnImpactCleanUp()
        {
            base.OnImpactCleanUp();
            _rocketTarget.GameObject.SetActive(false);
            SetRocketTargetVisibleClientRpc(false);
        }

        // Sava added these fields and methods
        protected override bool needToTeleport => true;
        protected override float timeToActiveTrail => 0.2f;
        private PvPSoundType _type;
        private string _name;
        private Vector3 _pos;

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


        // PlayExplosionSound
        protected override void OnPlayExplosionSound(PvPSoundType type, string name, Vector3 position)
        {
            OnPlayExplosionSoundClientRpc(type, name, position);
        }

        private async void PlayExplosionSound()
        {
            await PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayer.PlaySoundAsync(new PvPSoundKey(_type, _name), _pos);
        }

        // should be called by client

        private void Awake()
        {
            InitialiseTril();
            // rocket
            _rocketTarget = GetComponentInChildren<PvPRocketTarget>();
            Assert.IsNotNull(_rocketTarget);
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

        //----------------------------- Rpcs -----------------------------

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
        private void OnPlayExplosionSoundClientRpc(PvPSoundType type, string name, Vector3 position)
        {
            _type = type;
            _name = name;
            _pos = position;
            Invoke("PlayExplosionSound", 0.05f);
        }

        // rocket
        [ClientRpc]
        private void SetRocketTargetVisibleClientRpc(bool visible)
        {
            _rocketTarget.gameObject.SetActive(visible);
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