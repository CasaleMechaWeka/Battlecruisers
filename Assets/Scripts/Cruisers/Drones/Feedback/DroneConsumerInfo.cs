using UnityEngine;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneConsumerInfo : IDroneConsumerInfo
    {
        public IDroneConsumer DroneConsumer { get; }
        public Vector2 Position { get; }
        public Vector2 Size { get; }

        public DroneConsumerInfo(IDroneConsumer droneConsumer, Vector2 position, Vector2 size)
        {
            DroneConsumer = droneConsumer;
            Position = position;
            Size = size;
        }
    }
}