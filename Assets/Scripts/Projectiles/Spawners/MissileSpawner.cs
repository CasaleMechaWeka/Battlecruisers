using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class MissileSpawner : ProjectileSpawner
	{
        private IFactoryProvider _factoryProvider;

        public MissileController missilePrefab;
        protected override ProjectileController ProjectilePrefab { get { return missilePrefab; } }

        public void Initialise(IProjectileStats missileStats, IFactoryProvider factoryProvider)
		{
            base.Initialise(missileStats, factoryProvider.DamageApplierFactory);

            _factoryProvider = factoryProvider;
		}

		public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter)
		{
            MissileController missile = Instantiate(missilePrefab, transform.position, new Quaternion());
            Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.InitialVelocityInMPerS);
            missile.Initialise(_projectileStats, missileVelocity, targetFilter, target, _factoryProvider);
		}
	}
}
