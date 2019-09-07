using UnityEngine;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public interface IDroneFeedbackFactory
    {
        IDroneFeedback CreateFeedback(IDroneConsumer droneConsumer, Vector2 position, Vector2 size);
    }
}