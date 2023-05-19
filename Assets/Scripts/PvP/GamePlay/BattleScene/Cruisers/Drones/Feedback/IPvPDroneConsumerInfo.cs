using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public interface IPvPDroneConsumerInfo
    {
        IPvPDroneConsumer DroneConsumer { get; }
        Vector2 Position { get; }
        Vector2 Size { get; }
    }
}