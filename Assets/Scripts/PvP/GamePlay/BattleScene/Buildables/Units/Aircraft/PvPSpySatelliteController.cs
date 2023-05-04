using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPSpySatelliteController : PvPSatelliteController
    {
        public override PvPTargetType TargetType => PvPTargetType.Satellite;

        protected override IList<IPvPPatrolPoint> GetPatrolPoints()
        {
            IList<Vector2> patrolPositions = _aircraftProvider.FindSpySatellitePatrolPoints(transform.position, cruisingAltitudeInM);

            IList<IPvPPatrolPoint> patrolPoints = new List<IPvPPatrolPoint>(patrolPositions.Count)
            {
                new PvPPatrolPoint(patrolPositions[0], removeOnceReached: true)
            };

            for (int i = 1; i < patrolPositions.Count; ++i)
            {
                patrolPoints.Add(new PvPPatrolPoint(patrolPositions[i]));
            }

            return patrolPoints;
        }
    }
}