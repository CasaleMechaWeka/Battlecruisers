namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    public interface IPvPUnitFilter
    {
        bool IsBuildableAcceptable(int buildableDroneNum, int droneManagerDroneNum);
    }
}
