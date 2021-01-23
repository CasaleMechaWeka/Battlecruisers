using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Proxy;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners.Beams
{
    public class BeamCollisionDetector : IBeamCollisionDetector
    {
        private readonly ContactFilter2D _contactFilter;
        private readonly ITargetFilter _targetFilter;

        private const int NUM_OF_COLLIDERS_TO_RAYCAST = 25;

        public BeamCollisionDetector(ContactFilter2D contactFilter, ITargetFilter targetFilter)
        {
            Assert.IsNotNull(targetFilter);

            _contactFilter = contactFilter;
            _targetFilter = targetFilter;
        }

        public IBeamCollision FindCollision(Vector2 source, float angleInDegrees, bool isSourceMirrored)
        {
            Logging.VerboseMethod(Tags.BEAM);

            Vector2 beamDirection = FindBeamDirection(angleInDegrees, isSourceMirrored);

            RaycastHit2D[] results = new RaycastHit2D[NUM_OF_COLLIDERS_TO_RAYCAST];
            int numOfResults = Physics2D.Raycast(source, beamDirection, _contactFilter, results);
            Logging.Verbose(Tags.BEAM, $"Physics2D.Raycast():  source: {source}  isSourceMirrored: {isSourceMirrored}  beamDirection: {beamDirection}  Results: {numOfResults}");

            return GetMatchingTarget(results, numOfResults);
        }

        private Vector2 FindBeamDirection(float angleInDegrees, bool isSourceMirrored)
        {
            float directionMultiplier = isSourceMirrored ? -1 : 1;
            float xComponent = Mathf.Cos(Mathf.Deg2Rad * angleInDegrees) * directionMultiplier;
            float yComponent = Mathf.Sin(Mathf.Deg2Rad * angleInDegrees);
            return new Vector2(xComponent, yComponent);
        }

        private IBeamCollision GetMatchingTarget(RaycastHit2D[] results, int numOfResults)
        {
            Logging.Verbose(Tags.BEAM, $"Number of collisions: {numOfResults}");

            for (int i = 0; i < numOfResults; i++)
            {
                RaycastHit2D result = results[i];
                Assert.IsNotNull(result.collider);

                ITarget target = result.collider.gameObject.GetComponent<ITargetProxy>()?.Target;

                if (target != null 
                    && !target.IsDestroyed
                    && _targetFilter.IsMatch(target))
                {
                    return new BeamCollision(target, result.point);
                }
            }

            return null;
        }
    }
}
