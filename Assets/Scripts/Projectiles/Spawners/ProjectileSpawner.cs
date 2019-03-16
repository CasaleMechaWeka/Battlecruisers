using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class ProjectileSpawner : MonoBehaviour
	{
        protected ITarget _parent;
        protected IProjectileStats _projectileStats;
		protected IFactoryProvider _factoryProvider;

        protected abstract ProjectileController ProjectilePrefab { get; }

        public void Initialise(IProjectileSpawnerArgs args)
        {
            Helper.AssertIsNotNull(ProjectilePrefab, args);

            _parent = args.Parent;
            _projectileStats = args.ProjectileStats;
            _factoryProvider = args.FactoryProvider;
        }

		protected Vector2 FindProjectileVelocity(float angleInDegrees, bool isSourceMirrored, float velocityInMPerS)
		{
			float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

			int xDirectionMultiplier = isSourceMirrored ? -1 : 1;

			float velocityX = velocityInMPerS * Mathf.Cos(angleInRadians) * xDirectionMultiplier;
			float velocityY = velocityInMPerS * Mathf.Sin(angleInRadians);

            Logging.Log(Tags.SHELL_SPAWNER, $"angleInDegrees: {angleInDegrees}  isSourceMirrored: {isSourceMirrored}  =>  velocityX: {velocityX}  velocityY: {velocityY}");

			return new Vector2(velocityX, velocityY);
		}
	}
}
