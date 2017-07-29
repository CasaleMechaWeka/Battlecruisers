using System.Collections.Generic;
using BattleCruisers.Data.PrefabKeys;

namespace BattleCruisers.Data.BuildOrders
{
	public static class Balanced
	{
		public static IList<IPrefabKey> BuildOrder
		{
			get
			{
				return new List<IPrefabKey>()
				{
					StaticPrefabKeys.Buildings.DroneStation,
					StaticPrefabKeys.Buildings.AntiAirTurret,
					StaticPrefabKeys.Buildings.AntiShipTurret,
					StaticPrefabKeys.Buildings.DroneStation,
                    StaticPrefabKeys.Buildings.SamSite,
					StaticPrefabKeys.Buildings.Mortar,
					StaticPrefabKeys.Buildings.ShieldGenerator,
					StaticPrefabKeys.Buildings.TeslaCoil,
					StaticPrefabKeys.Buildings.DroneStation,
					StaticPrefabKeys.Buildings.Artillery,
					StaticPrefabKeys.Buildings.ShieldGenerator,
					StaticPrefabKeys.Buildings.AntiAirTurret,
					StaticPrefabKeys.Buildings.Mortar,
					StaticPrefabKeys.Buildings.DroneStation,
					StaticPrefabKeys.Buildings.DroneStation,
					StaticPrefabKeys.Buildings.DroneStation,
					StaticPrefabKeys.Buildings.RocketLauncher,
					StaticPrefabKeys.Buildings.SamSite,
					StaticPrefabKeys.Buildings.Mortar,
					StaticPrefabKeys.Buildings.ShieldGenerator,
					StaticPrefabKeys.Buildings.SamSite,
					StaticPrefabKeys.Buildings.Mortar,
					StaticPrefabKeys.Buildings.SamSite,
					StaticPrefabKeys.Buildings.Mortar,
					StaticPrefabKeys.Buildings.DeathstarLauncher,
					StaticPrefabKeys.Buildings.SamSite,
					StaticPrefabKeys.Buildings.Mortar,
					StaticPrefabKeys.Buildings.SamSite,
					StaticPrefabKeys.Buildings.NukeLauncher
				};
			}
		}
	}
}