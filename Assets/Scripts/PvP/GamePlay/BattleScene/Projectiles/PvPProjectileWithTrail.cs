using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Trails;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
// using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using Unity.Netcode.Components;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    /// <summary>
    /// Projectiles with trails (eg: rockets) should not have the trail disappear on impact.
    /// Instead, the projectile should be inert, but set the trail hang around and dissipate
    /// before deactivating completely and being recycled.
    /// </summary>
    public abstract class PvPProjectileWithTrail<TPvPActivationArgs, TPvPStats> : PvPProjectileControllerBase<TPvPActivationArgs, TPvPStats>,
        IPvPRemovable,
        IPvPPoolable<TPvPActivationArgs>
            where TPvPActivationArgs : PvPProjectileActivationArgs<TPvPStats>
            where TPvPStats : IPvPProjectileStats
    {
        private Collider2D _collider;
        private IPvPDeferrer _deferrer;
        private IPvPProjectileTrail _trail;
        protected virtual float timeToActiveTrail { get => 0.05f; }
        protected virtual float TrailLifetimeInS { get => 10; }

        public override void Initialise(ILocTable commonStrings, IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(commonStrings, factoryProvider);

            _deferrer = factoryProvider.DeferrerProvider.Deferrer;

            _collider = GetComponent<Collider2D>();
            Assert.IsNotNull(_collider);

            _trail = GetComponentInChildren<IPvPProjectileTrail>();
            Assert.IsNotNull(_trail);
            _trail.Initialise();
        }

        public override void Activate(TPvPActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
            _collider.enabled = true;
            _trail.ShowAllEffects();
            ShowAllEffectsOfClient();
        }



        protected virtual void ShowAllEffectsOfClient()
        {
            if (IsClient)
            {
                _trail.ShowAllEffects();
            }
        }
        public override void Initialise()
        {
            base.Initialise();
            _collider = GetComponent<Collider2D>();
            Assert.IsNotNull(_collider);
        }
        public void InitialiseTril()
        {
            _trail = GetComponentInChildren<IPvPProjectileTrail>();
            Assert.IsNotNull(_trail);
            _trail.Initialise();
        }

        protected override void DestroyProjectile()
        {
            // Logging.LogMethod(Tags.SHELLS);

            ShowExplosion();
            OnImpactCleanUp();
            InvokeDestroyed();
            _deferrer.Defer(OnTrailsDoneCleanup, TrailLifetimeInS);
        }

        protected virtual void OnImpactCleanUp()
        {
            // Logging.LogMethod(Tags.SHELLS);

            // Dummy movement controller doesn't set velocity to 0, so need to set rigid body velocity manually.
            _rigidBody.velocity = Vector2.zero;
            MovementController = null;
            _collider.enabled = false;
            _trail.HideEffects();
            HideEffectsOfClient();
        }

        protected virtual void HideEffectsOfClient()
        {
            if (IsClient)
            {
                _trail.HideEffects();
            }
        }



        private void OnTrailsDoneCleanup()
        {
            // Logging.LogMethod(Tags.SHELLS);

            OnSetPosition_Visible(Position, false);
            gameObject.SetActive(false);
            InvokeDeactivated();
        }
    }
}