using BattleCruisers.Drones;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Repairables
{
    /// <summary>
    /// Provides feedback to the user about a drone consumer's number of drones.
    /// </summary>
    public interface IDroneNumFeedback : IManagedDisposable
    {
        IDroneConsumer DroneConsumer { get; }
    }
}
