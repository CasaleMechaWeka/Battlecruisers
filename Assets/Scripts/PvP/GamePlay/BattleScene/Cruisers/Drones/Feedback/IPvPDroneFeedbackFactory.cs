using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public interface IPvPDroneFeedbackFactory
    {
        IPvPDroneFeedback CreateFeedback(IPvPDroneConsumer droneConsumer, Vector2 position, Vector2 size);
        IPvPDroneFeedback CreateDummyFeedback();
    }
}
