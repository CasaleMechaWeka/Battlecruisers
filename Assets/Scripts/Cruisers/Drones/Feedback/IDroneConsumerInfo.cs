using UnityEngine;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public interface IDroneConsumerInfo
    {
        IDroneConsumer DroneConsumer { get; }
        Vector2 Position { get; }
        Vector2 Size { get; }
    }
}