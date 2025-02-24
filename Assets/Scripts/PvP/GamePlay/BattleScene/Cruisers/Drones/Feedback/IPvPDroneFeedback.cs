using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public interface IPvPDroneFeedback : IManagedDisposable
    {
        IDroneConsumer DroneConsumer { get; }
    }
}
