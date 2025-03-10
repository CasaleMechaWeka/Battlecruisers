using System.Collections.Generic;
using BattleCruisers.Movement.Velocity;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class SpySatelliteController : SatelliteController
    {
        public override TargetType TargetType => TargetType.Satellite;

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            IList<Vector2> patrolPositions = _aircraftProvider.FindSpySatellitePatrolPoints(transform.position, cruisingAltitudeInM);

            IList<IPatrolPoint> patrolPoints = new List<IPatrolPoint>(patrolPositions.Count)
            {
                new PatrolPoint(patrolPositions[0], removeOnceReached: true)
            };

            for (int i = 1; i < patrolPositions.Count; ++i)
            {
                patrolPoints.Add(new PatrolPoint(patrolPositions[i]));
            }

            return patrolPoints;
        }
    }
}