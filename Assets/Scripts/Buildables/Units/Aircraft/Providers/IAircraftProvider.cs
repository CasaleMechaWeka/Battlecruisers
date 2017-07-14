using BattleCruisers.Movement.Velocity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft.Providers
{
	public interface IAircraftProvider
	{
		SafeZone FighterSafeZone { get; }

		IList<IPatrolPoint> FindBomberPatrolPoints(float cruisingAltitudeInM, Action onFirstPatrolPointReached);
		IList<IPatrolPoint> FindFighterPatrolPoints(float cruisingAltitudeInM);
		IList<IPatrolPoint> FindDeathstarPatrolPoints(Vector2 deathstarPosition, float cruisingAltitudeInM, Action onFirstPatrolPointReached);
	}
}
