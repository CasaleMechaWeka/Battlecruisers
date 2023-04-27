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

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPMissileController :
        PvPProjectileWithTrail<PvPTargetProviderActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>,
        IPvPTargetProvider
    {
        private IPvPDeferrer _deferrer;
        private IPvPMovementController _dummyMovementController;

        private const float MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S = 2;

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
    }
}