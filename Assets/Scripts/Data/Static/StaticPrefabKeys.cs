using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Static
{
	public static class StaticPrefabKeys
	{
		public static class Buildings
		{
			// Factories
			public static BuildingKey AirFactory => new BuildingKey(BuildingCategory.Factory, "AirFactory");
			public static BuildingKey NavalFactory => new BuildingKey(BuildingCategory.Factory, "NavalFactory");
			public static BuildingKey DroneStation => new BuildingKey(BuildingCategory.Factory, "EngineeringBay");

			// Tactical
			public static BuildingKey ShieldGenerator => new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator");
            public static BuildingKey StealthGenerator => new BuildingKey(BuildingCategory.Tactical, "StealthGenerator");
            public static BuildingKey SpySatelliteLauncher => new BuildingKey(BuildingCategory.Tactical, "SpySatelliteLauncher");
			public static BuildingKey LocalBooster => new BuildingKey(BuildingCategory.Tactical, "LocalBooster");
            public static BuildingKey ControlTower => new BuildingKey(BuildingCategory.Tactical, "ControlTower");

			// Defence
			public static BuildingKey AntiShipTurret => new BuildingKey(BuildingCategory.Defence, "AntiShipTurret");
			public static BuildingKey AntiAirTurret => new BuildingKey(BuildingCategory.Defence, "AntiAirTurret");
			public static BuildingKey Mortar => new BuildingKey(BuildingCategory.Defence, "Mortar");
			public static BuildingKey SamSite => new BuildingKey(BuildingCategory.Defence, "SamSite");
			public static BuildingKey TeslaCoil => new BuildingKey(BuildingCategory.Defence, "TeslaCoil");

			// Offence
			public static BuildingKey Artillery => new BuildingKey(BuildingCategory.Offence, "Artillery");
			public static BuildingKey RocketLauncher => new BuildingKey(BuildingCategory.Offence, "RocketLauncher");
			public static BuildingKey Railgun => new BuildingKey(BuildingCategory.Offence, "Railgun");

			// Ultras
			public static BuildingKey DeathstarLauncher => new BuildingKey(BuildingCategory.Ultra, "DeathstarLauncher");
			public static BuildingKey NukeLauncher => new BuildingKey(BuildingCategory.Ultra, "NukeLauncher");
			public static BuildingKey Ultralisk => new BuildingKey(BuildingCategory.Ultra, "Ultralisk");
            public static BuildingKey KamikazeSignal => new BuildingKey(BuildingCategory.Ultra, "KamikazeSignal");
            public static BuildingKey Broadsides => new BuildingKey(BuildingCategory.Ultra, "Broadsides");
        }

        public static class Units
        {
            // Aircraft
            public static UnitKey Bomber => new UnitKey(UnitCategory.Aircraft, "Bomber");
            public static UnitKey Fighter => new UnitKey(UnitCategory.Aircraft, "Fighter");
            public static UnitKey Gunship => new UnitKey(UnitCategory.Aircraft, "Gunship");

            // Ships
            public static UnitKey AttackBoat => new UnitKey(UnitCategory.Naval, "AttackBoat");
            public static UnitKey Frigate => new UnitKey(UnitCategory.Naval, "Frigate");
            public static UnitKey Destroyer => new UnitKey(UnitCategory.Naval, "Destroyer");
            public static UnitKey ArchonBattleship => new UnitKey(UnitCategory.Naval, "ArchonBattleship");
		}

		public static class Hulls
		{
			public static HullKey Bullshark => new HullKey("Bullshark");
			public static HullKey Eagle => new HullKey("Eagle");
			public static HullKey Hammerhead => new HullKey("Hammerhead");
			public static HullKey Longbow => new HullKey("Longbow");
			public static HullKey Megalodon => new HullKey("Megalodon");
			public static HullKey Raptor => new HullKey("Raptor");
			public static HullKey Rockjaw => new HullKey("Rockjaw");
			public static HullKey Trident => new HullKey("Trident");
		}

        public static class UI
        {
            public static UIKey DeleteCountdown => new UIKey("DeleteCountdown");
        }

        public static class Explosions
        {
            public static ExplosionKey HDExplosion75 => new ExplosionKey("HDExplosion0.75");
            public static ExplosionKey HDExplosion100 => new ExplosionKey("HDExplosion1.0");
            public static ExplosionKey HDExplosion150 => new ExplosionKey("HDExplosion1.5");
            public static ExplosionKey HDExplosion500 => new ExplosionKey("HDExplosion5.0");
        }
	}
}
