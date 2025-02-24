using BattleCruisers.Cruisers.Drones.Feedback;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public interface IPvPSpawnPositionFinder
    {
        Vector2 FindSpawnPosition(IDroneConsumerInfo droneConsumerInfo);
    }
}