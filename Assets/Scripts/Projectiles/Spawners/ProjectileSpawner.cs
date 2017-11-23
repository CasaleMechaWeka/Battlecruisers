using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class ProjectileSpawner : MonoBehaviour
	{
        protected IProjectileStats _projectileStats;
        protected abstract ProjectileController ProjectilePrefab { get; }

        protected IFactoryProvider _factoryProvider;

        public void Initialise(IProjectileStats projectileStats, IFactoryProvider factoryProvider)
        {
            Helper.AssertIsNotNull(ProjectilePrefab, projectileStats, factoryProvider);

            _projectileStats = projectileStats;
            _factoryProvider = factoryProvider;
        }

		protected Vector2 FindProjectileVelocity(float angleInDegrees, bool isSourceMirrored, float velocityInMPerS)
		{
			float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

			int xDirectionMultiplier = isSourceMirrored ? -1 : 1;

			float velocityX = velocityInMPerS * Mathf.Cos(angleInRadians) * xDirectionMultiplier;
			float velocityY = velocityInMPerS * Mathf.Sin(angleInRadians);

			Logging.Log(Tags.SHELL_SPAWNER, string.Format("angleInDegrees: {0}  isSourceMirrored: {1}  =>  velocityX: {2}  velocityY: {3}",
				angleInDegrees, isSourceMirrored, velocityX, velocityY));

			return new Vector2(velocityX, velocityY);
		}
	}
}
