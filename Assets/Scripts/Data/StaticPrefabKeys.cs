using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.PrefabKeys;

namespace BattleCruisers.Data
{
	public static class StaticPrefabKeys
	{
		public static class Buildings
		{
			// Factories
			public static BuildingKey AirFactory { get { return new BuildingKey(BuildingCategory.Factory, "AirFactory"); } }
			public static BuildingKey NavalFactory { get { return new BuildingKey(BuildingCategory.Factory, "NavalFactory"); } }
			public static BuildingKey DroneStation { get { return new BuildingKey(BuildingCategory.Factory, "EngineeringBay"); } }

			// Tactical
			public static BuildingKey ShieldGenerator { get { return new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator"); } }

			// Defence
			public static BuildingKey AntiShipTurret { get { return new BuildingKey(BuildingCategory.Defence, "AntiShipTurret"); } }
			public static BuildingKey AntiAirTurret { get { return new BuildingKey(BuildingCategory.Defence, "AntiAirTurret"); } }
			public static BuildingKey Mortar { get { return new BuildingKey(BuildingCategory.Defence, "Mortar"); } }
			public static BuildingKey TeslaCoil { get { return new BuildingKey(BuildingCategory.Defence, "TeslaCoil"); } }

			// Offence
			public static BuildingKey Artillery { get { return new BuildingKey(BuildingCategory.Offence, "Artillery"); } }
			public static BuildingKey RocketLauncher { get { return new BuildingKey(BuildingCategory.Offence, "RocketLauncher"); } }
			public static BuildingKey Railgun { get { return new BuildingKey(BuildingCategory.Offence, "Railgun"); } }

			// Ultras
			public static BuildingKey DeathstarLauncher { get { return new BuildingKey(BuildingCategory.Ultra, "DeathstarLauncher"); } }
			public static BuildingKey NukeLauncher { get { return new BuildingKey(BuildingCategory.Ultra, "NukeLauncher"); } }
		}
	}
}