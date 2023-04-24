

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public interface IDroneFeedback : IPvPManagedDisposable
    {
        IPvPDroneConsumer DroneConsumer { get; }
    }
}
