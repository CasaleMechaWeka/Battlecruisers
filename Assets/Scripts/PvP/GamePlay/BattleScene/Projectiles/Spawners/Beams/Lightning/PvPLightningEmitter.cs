using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using DigitalRuby.LightningBolt;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Lightning
{
    public class PvPLightningEmitter : PvPBeamEmitter
    {
        private float _damage;

        public LightningBoltScript lightningBolt;

        public void Initialise(IPvPTargetFilter targetFilter, float damage, IPvPTarget parent, ISettingsManager settingsManager)
        {
            base.Initialise(targetFilter, parent, settingsManager);

            Assert.IsTrue(damage > 0);
            _damage = damage;
        }

        protected override void HandleCollision(IPvPBeamCollision collision)
        {
            lightningBolt.StartPosition = transform.position;
            lightningBolt.EndPosition = collision.CollisionPoint;
            lightningBolt.Trigger();

            collision.Target.TakeDamage(_damage, _parent);

            _audioSource.Play();
        }
    }
}
