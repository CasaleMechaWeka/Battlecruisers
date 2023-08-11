using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPNukeController :
        PvPProjectileWithTrail<PvPTargetProviderActivationArgs<IPvPNukeStats>, IPvPNukeStats>,
        IPvPTargetProvider
    {
        private IPvPNukeStats _nukeStats;
        private IPvPFlightPointsProvider _flightPointsProvider;

        public IPvPTarget Target { get; private set; }

        public override void Activate(PvPTargetProviderActivationArgs<IPvPNukeStats> activationArgs)
        {
            base.Activate(activationArgs);

            _nukeStats = activationArgs.ProjectileStats;
            _flightPointsProvider = _factoryProvider.FlightPointsProviderFactory.NukeFlightPointsProvider;

            Target = activationArgs.Target;
        }

        public void Launch()
        {
            IPvPVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(_nukeStats.MaxVelocityInMPerS);
            IPvPTargetProvider targetProvider = this;

            MovementController
                = _factoryProvider.MovementControllerFactory.CreateRocketMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider,
                    _nukeStats.CruisingAltitudeInM,
                    _flightPointsProvider);
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

        private void Awake()
        {
            InitialiseTril();

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
