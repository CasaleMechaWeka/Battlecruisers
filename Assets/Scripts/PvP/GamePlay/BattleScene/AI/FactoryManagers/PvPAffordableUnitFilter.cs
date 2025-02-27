using BattleCruisers.AI.FactoryManagers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    public class PvPAffordableUnitFilter : IUnitFilter
    {
        public bool IsBuildableAcceptable(int buildableDroneNum, int droneManagerDroneNum)
        {
            return buildableDroneNum <= droneManagerDroneNum;
        }
    }
}
