using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft.Providers
{
	public interface IAircraftProvider
	{
		SafeZone FighterSafeZone { get; }

		IList<Vector2> FindBomberPatrolPoints(float cruisingAltitude);
		IList<Vector2> FindFighterPatrolPoints(float cruisingAltitude);
	}
}
