using BattleCruisers.Cruisers.Drones;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public interface IPvPDroneConsumerInfo
    {
        IDroneConsumer DroneConsumer { get; }
        Vector2 Position { get; }
        Vector2 Size { get; }
    }
}