using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams
{
    public class PvPBeamCollisionDetector : IPvPBeamCollisionDetector
    {
        private readonly ContactFilter2D _contactFilter;
        private readonly IPvPTargetFilter _targetFilter;

        private const int NUM_OF_COLLIDERS_TO_RAYCAST = 25;

        public PvPBeamCollisionDetector(ContactFilter2D contactFilter, IPvPTargetFilter targetFilter)
        {
            Assert.IsNotNull(targetFilter);

            _contactFilter = contactFilter;
            _targetFilter = targetFilter;
        }

        public IPvPBeamCollision FindCollision(Vector2 source, float angleInDegrees, bool isSourceMirrored)
        {
            // Logging.VerboseMethod(Tags.BEAM);

            Vector2 beamDirection = FindBeamDirection(angleInDegrees, isSourceMirrored);

            RaycastHit2D[] results = new RaycastHit2D[NUM_OF_COLLIDERS_TO_RAYCAST];
            int numOfResults = Physics2D.Raycast(source, beamDirection, _contactFilter, results);
            // Logging.Verbose(Tags.BEAM, $"Physics2D.Raycast():  source: {source}  isSourceMirrored: {isSourceMirrored}  beamDirection: {beamDirection}  Results: {numOfResults}");

            return GetMatchingTarget(results, numOfResults);
        }

        private Vector2 FindBeamDirection(float angleInDegrees, bool isSourceMirrored)
        {
            float directionMultiplier = isSourceMirrored ? -1 : 1;
            float xComponent = Mathf.Cos(Mathf.Deg2Rad * angleInDegrees) * directionMultiplier;
            float yComponent = Mathf.Sin(Mathf.Deg2Rad * angleInDegrees);
            return new Vector2(xComponent, yComponent);
        }

        private IPvPBeamCollision GetMatchingTarget(RaycastHit2D[] results, int numOfResults)
        {
            // Logging.Verbose(Tags.BEAM, $"Number of collisions: {numOfResults}");

            for (int i = 0; i < numOfResults; i++)
            {
                RaycastHit2D result = results[i];
                Assert.IsNotNull(result.collider);

                IPvPTarget target = result.collider.gameObject.GetComponent<IPvPTargetProxy>()?.Target;

                if (target != null
                    && !target.IsDestroyed
                    && _targetFilter.IsMatch(target))
                {
                    return new PvPBeamCollision(target, result.point);
                }
            }

            return null;
        }
    }
}
