namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
	public class AntiAirPrefabKeyWrapper : BasePrefabKeyWrapper
	{
		protected override BuildingKey[] GetBuildOrder(BuildingKey[] offensiveBuildOrder,
														BuildingKey[] antiAirBuildOrder,
														BuildingKey[] antiNavalBuildOrder)
		{
			return antiAirBuildOrder;
		}
	}
}
