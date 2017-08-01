using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.BuildOrders
{
	public static class AntiAir
	{
		public static IList<IPrefabKey> BuildOrder
		{
			get
			{
				return new List<IPrefabKey>()
				{
					StaticPrefabKeys.Buildings.AntiAirTurret,
					StaticPrefabKeys.Buildings.SamSite,
					StaticPrefabKeys.Buildings.SamSite,
					StaticPrefabKeys.Buildings.SamSite
				};
			}
		}
	}
}