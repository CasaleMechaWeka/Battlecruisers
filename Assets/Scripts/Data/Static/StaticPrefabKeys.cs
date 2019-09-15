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
			public static BuildingKey AirFactory { get; } = new BuildingKey(BuildingCategory.Factory, "AirFactory");
			public static BuildingKey NavalFactory { get; } = new BuildingKey(BuildingCategory.Factory, "NavalFactory");
			public static BuildingKey DroneStation { get; } = new BuildingKey(BuildingCategory.Factory, "EngineeringBay");

			// Tactical
			public static BuildingKey ShieldGenerator { get; } = new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator");
            public static BuildingKey StealthGenerator { get; } = new BuildingKey(BuildingCategory.Tactical, "StealthGenerator");
            public static BuildingKey SpySatelliteLauncher { get; } = new BuildingKey(BuildingCategory.Tactical, "SpySatelliteLauncher");
			public static BuildingKey LocalBooster { get; } = new BuildingKey(BuildingCategory.Tactical, "LocalBooster");
            public static BuildingKey ControlTower { get; } = new BuildingKey(BuildingCategory.Tactical, "ControlTower");

			// Defence
			public static BuildingKey AntiShipTurret { get; } = new BuildingKey(BuildingCategory.Defence, "AntiShipTurret");
			public static BuildingKey AntiAirTurret { get; } = new BuildingKey(BuildingCategory.Defence, "AntiAirTurret");
			public static BuildingKey Mortar { get; } = new BuildingKey(BuildingCategory.Defence, "Mortar");
			public static BuildingKey SamSite { get; } = new BuildingKey(BuildingCategory.Defence, "SamSite");
			public static BuildingKey TeslaCoil { get; } = new BuildingKey(BuildingCategory.Defence, "TeslaCoil");

			// Offence
			public static BuildingKey Artillery { get; } = new BuildingKey(BuildingCategory.Offence, "Artillery");
			public static BuildingKey RocketLauncher { get; } = new BuildingKey(BuildingCategory.Offence, "RocketLauncher");
			public static BuildingKey Railgun { get; } = new BuildingKey(BuildingCategory.Offence, "Railgun");

			// Ultras
			public static BuildingKey DeathstarLauncher { get; } = new BuildingKey(BuildingCategory.Ultra, "DeathstarLauncher");
			public static BuildingKey NukeLauncher { get; } = new BuildingKey(BuildingCategory.Ultra, "NukeLauncher");
			public static BuildingKey Ultralisk { get; } = new BuildingKey(BuildingCategory.Ultra, "Ultralisk");
            public static BuildingKey KamikazeSignal { get; } = new BuildingKey(BuildingCategory.Ultra, "KamikazeSignal");
            public static BuildingKey Broadsides { get; } = new BuildingKey(BuildingCategory.Ultra, "Broadsides");
        }

        public static class Units
        {
            // Aircraft
            public static UnitKey Bomber { get; } = new UnitKey(UnitCategory.Aircraft, "Bomber");
            public static UnitKey Fighter { get; } = new UnitKey(UnitCategory.Aircraft, "Fighter");
            public static UnitKey Gunship { get; } = new UnitKey(UnitCategory.Aircraft, "Gunship");
            public static UnitKey TestAircraft { get; } = new UnitKey(UnitCategory.Aircraft, "TestAircraft");

            // Ships
            public static UnitKey AttackBoat { get; } = new UnitKey(UnitCategory.Naval, "AttackBoat");
            public static UnitKey Frigate { get; } = new UnitKey(UnitCategory.Naval, "Frigate");
            public static UnitKey Destroyer { get; } = new UnitKey(UnitCategory.Naval, "Destroyer");
            public static UnitKey ArchonBattleship { get; } = new UnitKey(UnitCategory.Naval, "ArchonBattleship");
		}

		public static class Hulls
		{
			public static HullKey Bullshark { get; } = new HullKey("Bullshark");
			public static HullKey Eagle { get; } = new HullKey("Eagle");
			public static HullKey Hammerhead { get; } = new HullKey("Hammerhead");
			public static HullKey Longbow { get; } = new HullKey("Longbow");
			public static HullKey Megalodon { get; } = new HullKey("Megalodon");
			public static HullKey Raptor { get; } = new HullKey("Raptor");
			public static HullKey Rockjaw { get; } = new HullKey("Rockjaw");
			public static HullKey Trident { get; } = new HullKey("Trident");
		}

        public static class UI
        {
            public static UIKey DeleteCountdown { get; } = new UIKey("DeleteCountdown");
        }

        public static class Effects
        {
            public static EffectKey BuilderDrone { get; } = new EffectKey("BuilderDrone");
        }

        public static class Explosions
        {
            public static ExplosionKey BulletImpact { get; } = new ExplosionKey("BulletImpact");
            public static ExplosionKey Explosion75 { get; } = new ExplosionKey("Explosion0.75");
            public static ExplosionKey Explosion100 { get; } = new ExplosionKey("Explosion1.0");
            public static ExplosionKey Explosion150 { get; } = new ExplosionKey("Explosion1.5");
            public static ExplosionKey Explosion500 { get; } = new ExplosionKey("Explosion5.0");
        }

        public static class Projectiles
        {
            public static ProjectileKey Bullet { get; } = new ProjectileKey("Bullet");
            public static ProjectileKey ShellSmall { get; } = new ProjectileKey("ShellSmall");
            public static ProjectileKey ShellLarge { get; } = new ProjectileKey("ShellLarge");

            public static ProjectileKey MissileSmall { get; } = new ProjectileKey("MissileSmall");
            public static ProjectileKey MissileMedium { get; } = new ProjectileKey("MissileMedium");
            public static ProjectileKey MissileLarge { get; } = new ProjectileKey("MissileLarge");

            public static ProjectileKey Bomb { get; } = new ProjectileKey("Bomb");
            public static ProjectileKey Nuke { get; } = new ProjectileKey("Nuke");
            public static ProjectileKey Rocket { get; } = new ProjectileKey("Rocket");
        }
	}
}
