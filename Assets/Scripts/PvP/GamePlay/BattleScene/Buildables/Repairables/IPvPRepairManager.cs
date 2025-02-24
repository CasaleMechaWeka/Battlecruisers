using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables
{
    public interface IPvPRepairManager : IManagedDisposable
    {
        void Repair(float deltaTimeInS);
        IDroneConsumer GetDroneConsumer(IRepairable repairable);

        //void AddCruiser(ICruiser cruiser);
    }
}
