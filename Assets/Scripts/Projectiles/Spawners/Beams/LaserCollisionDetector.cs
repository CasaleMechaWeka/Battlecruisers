using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners.Beams
{
    public class LaserCollisionDetector : ILaserCollisionDetector
    {
        private readonly ContactFilter2D _contactFilter;
        private readonly ITargetFilter _targetFilter;

        private const int NUM_OF_COLLIDERS_TO_RAYCAST = 25;

        public LaserCollisionDetector(ContactFilter2D contactFilter, ITargetFilter targetFilter)
        {
            Assert.IsNotNull(targetFilter);

            _contactFilter = contactFilter;
            _targetFilter = targetFilter;
        }

        public ILaserCollision FindCollision(Vector2 source, float angleInDegrees, bool isSourceMirrored)
        {
            Logging.VerboseMethod(Tags.LASER);

            Vector2 laserDirection = FindLaserDirection(angleInDegrees, isSourceMirrored);

            RaycastHit2D[] results = new RaycastHit2D[NUM_OF_COLLIDERS_TO_RAYCAST];
            int numOfResults = Physics2D.Raycast(source, laserDirection, _contactFilter, results);
            Logging.Verbose(Tags.LASER, $"Physics2D.Raycast():  source: {source}  isSourceMirrored: {isSourceMirrored}  laserDirection: {laserDirection}  Results: {numOfResults}");

            return GetMatchingTarget(results, numOfResults);
        }

        private Vector2 FindLaserDirection(float angleInDegrees, bool isSourceMirrored)
        {
            float directionMultiplier = isSourceMirrored ? -1 : 1;
            float xComponent = Mathf.Cos(Mathf.Deg2Rad * angleInDegrees) * directionMultiplier;
            float yComponent = Mathf.Sin(Mathf.Deg2Rad * angleInDegrees);
            return new Vector2(xComponent, yComponent);
        }

        private ILaserCollision GetMatchingTarget(RaycastHit2D[] results, int numOfResults)
        {
            Logging.Verbose(Tags.LASER, $"Number of collisions: {numOfResults}");

            for (int i = 0; i < numOfResults; i++)
            {
                RaycastHit2D result = results[i];
                Assert.IsNotNull(result.collider);

                ITarget target = result.collider.gameObject.GetComponent<ITargetProxy>()?.Target;

                if (target != null 
                    && !target.IsDestroyed
                    && _targetFilter.IsMatch(target))
                {
                    return new LaserCollision(target, result.point);
                }
            }

            return null;
        }
    }
}
