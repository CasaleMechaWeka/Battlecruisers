namespace BattleCruisers.AI.FactoryManagers
{
    public class AffordableUnitFilter : IUnitFilter
	{
        public bool IsBuildableAcceptable(int buildableDroneNum, int droneManagerDroneNum)
        {
            return buildableDroneNum <= droneManagerDroneNum;
        }
	}
}
