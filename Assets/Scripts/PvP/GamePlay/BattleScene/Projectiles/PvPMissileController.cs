using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPMissileController :
        PvPProjectileWithTrail<PvPTargetProviderActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>,
        IPvPTargetProvider
    {
        private IPvPDeferrer _deferrer;
        private IPvPMovementController _dummyMovementController;

        private const float MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S = 2;
        protected override float timeToActiveTrail => 0.1f;
        protected override bool needToTeleport => true;
        public SpriteRenderer missile;

        protected override float TrailLifetimeInS => 3;
        public IPvPTarget Target { get; private set; }

        public override void Initialise(ILocTable commonStrings, IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(commonStrings, factoryProvider);
            Assert.IsNotNull(missile);
        }

        public override void Activate(PvPTargetProviderActivationArgs<IPvPProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            Logging.Log(Tags.MISSILE, $"Rotation: {transform.rotation.eulerAngles}");

            Target = activationArgs.Target;
            _deferrer = _factoryProvider.DeferrerProvider.Deferrer;

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

            activationArgs.Target.Destroyed += Target_Destroyed;
        }

        private void Target_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            // Let missile keep current velocity
            MovementController = _dummyMovementController;

            // Destroy missile eventually (in case it does not hit a matching target)
            _deferrer.Defer(ConditionalDestroy, MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S);
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
            Target.Destroyed -= Target_Destroyed;
            base.DestroyProjectile();
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

        private async void PlayExplosionSound()
        {
            await PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayer.PlaySoundAsync(new PvPSoundKey(_type, _name), _pos);
        }

        // should be called by client

        private void Awake()
        {
            InitialiseTril();
        }

        protected override void ShowAllEffects()
        {
            if (IsClient)
                base.ShowAllEffects();
            if (IsServer)
                ShowAllEffectsClientRpc();
        }

        protected override void HideEffects()
        {
            if (IsClient)
                base.HideEffects();
            if (IsServer)
                HideEffectsClientRpc();
        }
        //----------------------------- Rpcs -----------------------------

        [ClientRpc]
        private void OnSetPosition_VisibleClientRpc(Vector3 position, bool visible)
        {
            //   transform.position = position;
            //   GetComponent<NetworkTransform>().Teleport(position,transform.rotation, transform.localScale);
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
        private void ShowAllEffectsClientRpc()
        {
            ShowAllEffects();
        }

        [ClientRpc]
        private void HideEffectsClientRpc()
        {
            HideEffects();
        }
    }
}