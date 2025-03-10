namespace BattleCruisers.AI.FactoryManagers
{
    public interface IUnitFilter
    {
        bool IsBuildableAcceptable(int buildableDroneNum, int droneManagerDroneNum);
    }
}
