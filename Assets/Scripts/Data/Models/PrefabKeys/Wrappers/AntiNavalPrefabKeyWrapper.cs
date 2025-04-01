namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public class AntiNavalPrefabKeyWrapper : BasePrefabKeyWrapper
    {
        protected override BuildingKey[] GetBuildOrder(BuildingKey[] offensiveBuildOrder,
                                                        BuildingKey[] antiAirBuildOrder,
                                                        BuildingKey[] antiNavalBuildOrder)
        {
            return antiNavalBuildOrder;
        }
    }
}
