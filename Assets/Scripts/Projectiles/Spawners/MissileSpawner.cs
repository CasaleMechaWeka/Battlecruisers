using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class MissileSpawner : ProjectileSpawner<MissileController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>
    {
        //private void Awake()
        //{
        //	Debug.Log("MissileSpawner: Awake called. Checking _projectileStats initialization.");
        //	if (_projectileStats == null)
        //	{
        //		Debug.LogError("MissileSpawner: _projectileStats is not initialized in Awake.");
        //	}
        //}

        public void SetProjectileStats(ProjectileStats stats)
        {
            _projectileStats = stats;
            Debug.Log($"_projectileStats set to: {_projectileStats}");
        }

        public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter)
        {
            Debug.Log($"Attempting to spawn missile with _projectileStats: {_projectileStats}");

            if (_projectileStats == null)
            {
                //    Debug.LogError("MissileSpawner: _projectileStats is null");
                return;
            }
            if (_parent == null)
            {
                //    Debug.LogError("MissileSpawner: _parent is null.");
                return;
            }
            if (_impactSound == null)
            {
                //   Debug.LogError("MissileSpawner: _impactSound is null.");
                return;
            }
            if (target == null)
            {
                //   Debug.LogError("MissileSpawner: target is null.");
                return;
            }
            if (targetFilter == null)
            {
                //   Debug.LogError("MissileSpawner: targetFilter is null.");
                return;
            }

            Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.InitialVelocityInMPerS);
            TargetProviderActivationArgs<ProjectileStats> activationArgs
                = new TargetProviderActivationArgs<ProjectileStats>(
                    transform.position,
                    _projectileStats,
                    missileVelocity,
                    targetFilter,
                    _parent,
                    _impactSound,
                    target);

            Debug.Log($"MissileSpawner: Position={activationArgs.Position}, InitialVelocity={activationArgs.InitialVelocityInMPerS}");

            base.SpawnProjectile(activationArgs);
        }
    }
}
