namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    public class PvPAffordableUnitFilter : IPvPUnitFilter
    {
        public bool IsBuildableAcceptable(int buildableDroneNum, int droneManagerDroneNum)
        {
            return buildableDroneNum <= droneManagerDroneNum;
        }
    }
}
