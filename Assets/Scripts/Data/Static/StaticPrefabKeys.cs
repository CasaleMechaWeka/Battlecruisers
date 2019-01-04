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
			public static BuildingKey AirFactory { get { return new BuildingKey(BuildingCategory.Factory, "AirFactory"); } }
			public static BuildingKey NavalFactory { get { return new BuildingKey(BuildingCategory.Factory, "NavalFactory"); } }
			public static BuildingKey DroneStation { get { return new BuildingKey(BuildingCategory.Factory, "EngineeringBay"); } }

			// Tactical
			public static BuildingKey ShieldGenerator { get { return new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator"); } }
            public static BuildingKey StealthGenerator { get { return new BuildingKey(BuildingCategory.Tactical, "StealthGenerator"); } }
            public static BuildingKey SpySatelliteLauncher { get { return new BuildingKey(BuildingCategory.Tactical, "SpySatelliteLauncher"); } }
			public static BuildingKey LocalBooster { get { return new BuildingKey(BuildingCategory.Tactical, "LocalBooster"); } }
            public static BuildingKey ControlTower { get { return new BuildingKey(BuildingCategory.Tactical, "ControlTower"); } }

			// Defence
			public static BuildingKey AntiShipTurret { get { return new BuildingKey(BuildingCategory.Defence, "AntiShipTurret"); } }
			public static BuildingKey AntiAirTurret { get { return new BuildingKey(BuildingCategory.Defence, "AntiAirTurret"); } }
			public static BuildingKey Mortar { get { return new BuildingKey(BuildingCategory.Defence, "Mortar"); } }
			public static BuildingKey SamSite { get { return new BuildingKey(BuildingCategory.Defence, "SamSite"); } }
			public static BuildingKey TeslaCoil { get { return new BuildingKey(BuildingCategory.Defence, "TeslaCoil"); } }

			// Offence
			public static BuildingKey Artillery { get { return new BuildingKey(BuildingCategory.Offence, "Artillery"); } }
			public static BuildingKey RocketLauncher { get { return new BuildingKey(BuildingCategory.Offence, "RocketLauncher"); } }
			public static BuildingKey Railgun { get { return new BuildingKey(BuildingCategory.Offence, "Railgun"); } }

			// Ultras
			public static BuildingKey DeathstarLauncher { get { return new BuildingKey(BuildingCategory.Ultra, "DeathstarLauncher"); } }
			public static BuildingKey NukeLauncher { get { return new BuildingKey(BuildingCategory.Ultra, "NukeLauncher"); } }
			public static BuildingKey Ultralisk { get { return new BuildingKey(BuildingCategory.Ultra, "Ultralisk"); } }
            public static BuildingKey KamikazeSignal { get { return new BuildingKey(BuildingCategory.Ultra, "KamikazeSignal"); } }
            public static BuildingKey Broadsides { get { return new BuildingKey(BuildingCategory.Ultra, "Broadsides"); } }
        }

        public static class Units
        {
            // Aircraft
            public static UnitKey Bomber { get { return new UnitKey(UnitCategory.Aircraft, "Bomber"); } }
            public static UnitKey Fighter { get { return new UnitKey(UnitCategory.Aircraft, "Fighter"); } }
            public static UnitKey Gunship { get { return new UnitKey(UnitCategory.Aircraft, "Gunship"); } }

            // Ships
            public static UnitKey AttackBoat { get { return new UnitKey(UnitCategory.Naval, "AttackBoat"); } }
            public static UnitKey Frigate { get { return new UnitKey(UnitCategory.Naval, "Frigate"); } }
            public static UnitKey Destroyer { get { return new UnitKey(UnitCategory.Naval, "Destroyer"); } }
            public static UnitKey ArchonBattleship { get { return new UnitKey(UnitCategory.Naval, "ArchonBattleship"); } }
		}

		public static class Hulls
		{
			public static HullKey Bullshark { get { return new HullKey("Bullshark"); } }
			public static HullKey Eagle { get { return new HullKey("Eagle"); } }
			public static HullKey Hammerhead { get { return new HullKey("Hammerhead"); } }
			public static HullKey Longbow { get { return new HullKey("Longbow"); } }
			public static HullKey Megalodon { get { return new HullKey("Megalodon"); } }
			public static HullKey Raptor { get { return new HullKey("Raptor"); } }
			public static HullKey Rockjaw { get { return new HullKey("Rockjaw"); } }
			public static HullKey Trident { get { return new HullKey("Trident"); } }
		}

        public static class UI
        {
            public static UIKey DeleteCountdown { get { return new UIKey("DeleteCountdown"); } }
        }

        public static class Explosions
        {
            public static ExplosionKey CartoonExplosion75 { get { return new ExplosionKey("CartoonExplosion0.75"); } }
            public static ExplosionKey CartoonExplosion100 { get { return new ExplosionKey("CartoonExplosion1.0"); } }
            public static ExplosionKey CartoonExplosion150 { get { return new ExplosionKey("CartoonExplosion1.5"); } }
            public static ExplosionKey CartoonExplosion1000 { get { return new ExplosionKey("CartoonExplosion10.0"); } }
        }
	}
}
