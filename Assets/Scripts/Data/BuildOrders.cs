using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data
{
	public static class BuildOrders
	{
		public static IList<IPrefabKey> Balanced
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

		public static IList<IPrefabKey> AdvancedBalanced
		{
			get
			{
				return new List<IPrefabKey>()
				{
					StaticPrefabKeys.Buildings.DroneStation,
					StaticPrefabKeys.Buildings.DroneStation,
					StaticPrefabKeys.Buildings.ShieldGenerator,
					StaticPrefabKeys.Buildings.DroneStation,
					StaticPrefabKeys.Buildings.Artillery,
					StaticPrefabKeys.Buildings.ShieldGenerator,
					StaticPrefabKeys.Buildings.DroneStation,
					StaticPrefabKeys.Buildings.DroneStation,
					StaticPrefabKeys.Buildings.DroneStation,
					StaticPrefabKeys.Buildings.RocketLauncher,
					StaticPrefabKeys.Buildings.DeathstarLauncher,
					StaticPrefabKeys.Buildings.NukeLauncher
				};
			}
		}

        public static IList<IPrefabKey> BasicAntiAir
        {
            get
            {
                return new List<IPrefabKey>()
                {
                    StaticPrefabKeys.Buildings.AntiAirTurret,
                    StaticPrefabKeys.Buildings.AntiAirTurret,
                    StaticPrefabKeys.Buildings.AntiAirTurret,
                    StaticPrefabKeys.Buildings.AntiAirTurret
                };
            }
        }

		public static IList<IPrefabKey> AntiAir
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

		public static IList<IPrefabKey> BasicAntiNaval
		{
			get
			{
				return new List<IPrefabKey>()
				{
					StaticPrefabKeys.Buildings.AntiShipTurret,
					StaticPrefabKeys.Buildings.AntiShipTurret,
					StaticPrefabKeys.Buildings.AntiShipTurret,
					StaticPrefabKeys.Buildings.AntiShipTurret
				};
			}
		}

        public static IList<IPrefabKey> AntiNaval
        {
            get
            {
                return new List<IPrefabKey>()
                {
					StaticPrefabKeys.Buildings.AntiShipTurret,
					StaticPrefabKeys.Buildings.Mortar,
					StaticPrefabKeys.Buildings.Mortar,
					StaticPrefabKeys.Buildings.Mortar
                };
            }
        }

		public static IList<IPrefabKey> AntiRocketLauncher
		{
			get
			{
				return new List<IPrefabKey>()
				{
                    StaticPrefabKeys.Buildings.TeslaCoil
				};
			}
		}
	}
}
