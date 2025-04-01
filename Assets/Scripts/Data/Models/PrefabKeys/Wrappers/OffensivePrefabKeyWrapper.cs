namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public class OffensivePrefabKeyWrapper : BasePrefabKeyWrapper
    {
        protected override BuildingKey[] GetBuildOrder(BuildingKey[] offensiveBuildOrder,
                                                        BuildingKey[] antiAirBuildOrder,
                                                        BuildingKey[] antiNavalBuildOrder)
        {
            return offensiveBuildOrder;
        }
    }
}
