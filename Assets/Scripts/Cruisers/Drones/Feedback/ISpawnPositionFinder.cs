using UnityEngine;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public interface ISpawnPositionFinder
    {
        Vector2 FindSpawnPosition(IDroneConsumerInfo droneConsumerInfo);
    }
}