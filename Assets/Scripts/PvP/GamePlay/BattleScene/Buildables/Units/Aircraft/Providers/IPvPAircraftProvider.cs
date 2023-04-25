using System.Collections.Generic;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.Providers
{
    public interface IPvPAircraftProvider
    {
        Rectangle FighterSafeZone { get; }

        IList<Vector2> FindBomberPatrolPoints(float cruisingAltitudeInM);
        IList<Vector2> FindGunshipPatrolPoints(float cruisingAltitudeInM);
        IList<Vector2> FindSteamCopterPatrolPoints(float cruisingAltitudeInM);
        IList<Vector2> FindFighterPatrolPoints(float cruisingAltitudeInM);
        IList<Vector2> FindDeathstarPatrolPoints(Vector2 deathstarPosition, float cruisingAltitudeInM);
        IList<Vector2> FindSpySatellitePatrolPoints(Vector2 satellitePosition, float cruisingAltitudeInM);
    }
}
