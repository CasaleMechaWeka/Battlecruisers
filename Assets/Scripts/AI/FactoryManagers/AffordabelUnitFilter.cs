namespace BattleCruisers.AI.FactoryManagers
{
    public class AffordableUnitFilter
    {
        public bool IsBuildableAcceptable(int buildableDroneNum, int droneManagerDroneNum)
        {
            return buildableDroneNum <= droneManagerDroneNum;
        }
    }
}
