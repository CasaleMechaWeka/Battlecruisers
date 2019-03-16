using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class MissileSpawner : ProjectileSpawner
	{
        public MissileController missilePrefab;
        protected override ProjectileController ProjectilePrefab => missilePrefab;

		public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter)
		{
            MissileController missile = Instantiate(missilePrefab, transform.position, new Quaternion());
            Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.InitialVelocityInMPerS);
            missile.Initialise(_projectileStats, missileVelocity, targetFilter, target, _factoryProvider, _parent);
		}
	}
}
