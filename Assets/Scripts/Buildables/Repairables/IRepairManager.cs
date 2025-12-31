using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
//using BattleCruisers.Cruisers;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairManager : IManagedDisposable
    {
        void Repair(float deltaTimeInS);
        IDroneConsumer GetDroneConsumer(IRepairable repairable);

        //void AddCruiser(ICruiser cruiser);
    }
}
