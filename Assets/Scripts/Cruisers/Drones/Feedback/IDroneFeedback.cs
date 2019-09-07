using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public interface IDroneFeedback : IManagedDisposable
    {
        IDroneConsumer DroneConsumer { get; }
    }
}