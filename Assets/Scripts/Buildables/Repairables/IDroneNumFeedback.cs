using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Repairables
{
    // FELIX  Remove :)
    /// <summary>
    /// Provides feedback to the user about a drone consumer's number of drones.
    /// </summary>
    public interface IDroneNumFeedback : IManagedDisposable
    {
        IDroneConsumer DroneConsumer { get; }
    }
}
