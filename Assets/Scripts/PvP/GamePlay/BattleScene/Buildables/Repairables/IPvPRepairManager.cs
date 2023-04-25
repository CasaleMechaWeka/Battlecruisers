using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables
{
    public interface IPvPRepairManager : IPvPManagedDisposable
    {
        void Repair(float deltaTimeInS);
        IPvPDroneConsumer GetDroneConsumer(IPvPRepairable repairable);

        //void AddCruiser(ICruiser cruiser);
    }
}
