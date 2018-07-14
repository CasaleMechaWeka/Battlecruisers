using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairManager : IManagedDisposable
    {
        void Repair(float deltaTimeInS);
        IDroneConsumer GetDroneConsumer(IRepairable repairable);
    }
}
