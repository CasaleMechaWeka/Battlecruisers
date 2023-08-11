using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using DigitalRuby.LightningBolt;
using UnityEngine.Assertions;
using Unity.Netcode;
using UnityEngine;

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
            HandleCollision_PvP(transform.position, collision.CollisionPoint);
            //  _audioSource.Play();
        }

        private void HandleCollision_PvP(Vector3 startPos, Vector3 endPos)
        {
            HandleCollisionClientRpc(startPos, endPos);
        }

        protected override void PlaySparks_PvP()
        {
            PlaySparksClientRpc();
        }
        protected override void StopSparks_PvP()
        {
            StopSparksClientRpc();
        }

        [ClientRpc]
        private void PlaySparksClientRpc()
        {
            constantSparks.Play();
        }

        [ClientRpc]
        private void StopSparksClientRpc()
        {
            constantSparks.Stop();
        }
        [ClientRpc]
        private void HandleCollisionClientRpc(Vector3 startPos, Vector3 endPos)
        {
            lightningBolt.StartPosition = startPos;
            lightningBolt.EndPosition = endPos;
            lightningBolt.Trigger();
            _audioSource.Play();
        }
    }
}
