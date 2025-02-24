using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public interface IPvPDroneFeedbackFactory
    {
        IDroneFeedback CreateFeedback(IDroneConsumer droneConsumer, Vector2 position, Vector2 size);
        IDroneFeedback CreateDummyFeedback();
    }
}
