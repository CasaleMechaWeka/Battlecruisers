using System.Collections.Generic;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.UI.Sound;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class SpySatelliteController : AircraftController
    {
		// TEMP  Use satellite sound once we have it :)
        protected override ISoundKey EngineSoundKey => SoundKeys.Engines.Bomber;

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
