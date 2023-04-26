using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPBombController : PvPProjectileWithTrail<PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        protected override float TrailLifetimeInS => 3;

        public override void Activate(PvPProjectileActivationArgs<IPvPProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);
            MovementController = _factoryProvider.MovementControllerFactory.CreateDummyMovementController();
        }

        protected override void OnImpactCleanUp()
        {
            base.OnImpactCleanUp();
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.gravityScale = 0;
        }
    }
}