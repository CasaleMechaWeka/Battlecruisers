using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class ProjectileSpawner<TProjectile, TProjectileArgs, TStats> : MonoBehaviour
        where TProjectile : ProjectileControllerBase<TProjectileArgs, TStats>
        where TProjectileArgs : ProjectileActivationArgs<TStats>
        where TStats : IProjectileStats
	{
        protected ITarget _parent;
        protected IProjectileStats _projectileStats;
		protected IFactoryProvider _factoryProvider;
        protected IPool<TProjectile, TProjectileArgs> _projectilePool;

        protected IAudioClipWrapper _impactSound;
        public AudioClip impactSound;

        public void Initialise(IProjectileSpawnerArgs args)
        {
            Helper.AssertIsNotNull(impactSound, args);

            _impactSound = new AudioClipWrapper(impactSound);
            _parent = args.Parent;
            _projectileStats = args.ProjectileStats;
            _factoryProvider = args.FactoryProvider;

            IProjectilePoolChooser<TProjectile, TProjectileArgs, TStats> poolChooser = GetComponent<IProjectilePoolChooser<TProjectile, TProjectileArgs, TStats>>();
            Assert.IsNotNull(poolChooser);
            _projectilePool = poolChooser.ChoosePool(args.FactoryProvider.PoolProviders.ProjectilePoolProvider);
        }

		protected Vector2 FindProjectileVelocity(float angleInDegrees, bool isSourceMirrored, float velocityInMPerS)
		{
			float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

			int xDirectionMultiplier = isSourceMirrored ? -1 : 1;

			float velocityX = velocityInMPerS * Mathf.Cos(angleInRadians) * xDirectionMultiplier;
			float velocityY = velocityInMPerS * Mathf.Sin(angleInRadians);

            Logging.Log(Tags.PROJECTILE_SPAWNER, $"angleInDegrees: {angleInDegrees}  isSourceMirrored: {isSourceMirrored}  =>  velocityX: {velocityX}  velocityY: {velocityY}");

			return new Vector2(velocityX, velocityY);
		}
	}
}
