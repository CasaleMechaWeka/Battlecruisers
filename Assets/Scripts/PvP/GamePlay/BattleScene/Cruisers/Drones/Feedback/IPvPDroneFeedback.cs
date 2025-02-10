using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public interface IPvPDroneFeedback : IManagedDisposable
    {
        IPvPDroneConsumer DroneConsumer { get; }
    }
}
