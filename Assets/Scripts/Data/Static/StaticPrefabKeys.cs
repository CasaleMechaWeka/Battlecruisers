using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;

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
            public static BuildingKey DroneStation4 { get; } = new BuildingKey(BuildingCategory.Factory, "EngineeringBay4");
            public static BuildingKey DroneStation8 { get; } = new BuildingKey(BuildingCategory.Factory, "EngineeringBay8");

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
            public static BuildingKey Coastguard { get; } = new BuildingKey(BuildingCategory.Defence, "Coastguard");//new

            // Offence
            public static BuildingKey Artillery { get; } = new BuildingKey(BuildingCategory.Offence, "Artillery");
            public static BuildingKey RocketLauncher { get; } = new BuildingKey(BuildingCategory.Offence, "RocketLauncher");
            public static BuildingKey Railgun { get; } = new BuildingKey(BuildingCategory.Offence, "Railgun");
            public static BuildingKey MLRS { get; } = new BuildingKey(BuildingCategory.Offence, "MLRS");
            public static BuildingKey GatlingMortar { get; } = new BuildingKey(BuildingCategory.Offence, "GatlingMortar");
            public static BuildingKey IonCannon { get; } = new BuildingKey(BuildingCategory.Offence, "IonCannon");//new
            public static BuildingKey MissilePod { get; } = new BuildingKey(BuildingCategory.Offence, "MissilePod");//new


            // Ultras
            public static BuildingKey DeathstarLauncher { get; } = new BuildingKey(BuildingCategory.Ultra, "DeathstarLauncher");
            public static BuildingKey NukeLauncher { get; } = new BuildingKey(BuildingCategory.Ultra, "NukeLauncher");
            public static BuildingKey Ultralisk { get; } = new BuildingKey(BuildingCategory.Ultra, "Ultralisk");
            public static BuildingKey KamikazeSignal { get; } = new BuildingKey(BuildingCategory.Ultra, "KamikazeSignal");
            public static BuildingKey Broadsides { get; } = new BuildingKey(BuildingCategory.Ultra, "Broadsides");
            public static BuildingKey NovaArtillery { get; } = new BuildingKey(BuildingCategory.Ultra, "NovaArtillery");//new

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        // Factories
                        AirFactory, NavalFactory, DroneStation, DroneStation4, DroneStation8,
                        // Tactical
                        ShieldGenerator, StealthGenerator, SpySatelliteLauncher, LocalBooster, ControlTower,
                        // Defence
                        AntiShipTurret, AntiAirTurret, Mortar, SamSite, TeslaCoil, Coastguard,
                        // Offence
                        Artillery, RocketLauncher, Railgun, MLRS, GatlingMortar, MissilePod, IonCannon, //railgun = LasCannon! 
                        // Ultras
                        DeathstarLauncher, NukeLauncher, Ultralisk, KamikazeSignal, Broadsides, NovaArtillery,
                    };
                }
            }
        }


        public static class Units
        {
            // Aircraft
            public static UnitKey Bomber { get; } = new UnitKey(UnitCategory.Aircraft, "Bomber");
            public static UnitKey Fighter { get; } = new UnitKey(UnitCategory.Aircraft, "Fighter");
            public static UnitKey Gunship { get; } = new UnitKey(UnitCategory.Aircraft, "Gunship");
            public static UnitKey SteamCopter { get; } = new UnitKey(UnitCategory.Aircraft, "SteamCopter");
            public static UnitKey Broadsword { get; } = new UnitKey(UnitCategory.Aircraft, "Broadsword");
            public static UnitKey TestAircraft { get; } = new UnitKey(UnitCategory.Aircraft, "TestAircraft");

            // Ships
            public static UnitKey AttackBoat { get; } = new UnitKey(UnitCategory.Naval, "AttackBoat");
            public static UnitKey AttackRIB { get; } = new UnitKey(UnitCategory.Naval, "AttackRIB");
            public static UnitKey Frigate { get; } = new UnitKey(UnitCategory.Naval, "Frigate");
            public static UnitKey Destroyer { get; } = new UnitKey(UnitCategory.Naval, "Destroyer");
            public static UnitKey ArchonBattleship { get; } = new UnitKey(UnitCategory.Naval, "ArchonBattleship");

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        // Aircraft
                        Bomber, Fighter, Gunship, SteamCopter, Broadsword, TestAircraft,
                        // Ships
                        AttackBoat, AttackRIB, Frigate, Destroyer, ArchonBattleship
                    };
                }
            }
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
            public static HullKey ManOfWarBoss { get; } = new HullKey("ManOfWarBoss");
            public static HullKey HuntressBoss { get; } = new HullKey("HuntressBoss");
            public static HullKey TasDevil { get; } = new HullKey("TasDevil");
            public static HullKey Yeti { get; } = new HullKey("Yeti");
            public static HullKey Rickshaw { get; } = new HullKey("Rickshaw");
            public static HullKey BlackRig { get; } = new HullKey("BlackRig");



            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        Bullshark, Eagle, Hammerhead, Longbow, Megalodon, Raptor, Rockjaw, Trident, ManOfWarBoss, HuntressBoss, BlackRig, Yeti, Rickshaw, TasDevil
                    };
                }
            }

            public static IList<HullKey> AllKeysExplicit
            {
                get
                {
                    return new List<HullKey>()
                    {
                        Bullshark, Eagle, Hammerhead, Longbow, Megalodon, Raptor, Rockjaw, Trident, BlackRig, Yeti, Rickshaw, TasDevil
                    };
                }
            }
        }

        public static class CaptainExos
        {
            // Captains
            public static CaptainExoKey CaptainExo000 { get; } = new CaptainExoKey("CaptainExo000");
            public static CaptainExoKey CaptainExo001 { get; } = new CaptainExoKey("CaptainExo001");
            public static CaptainExoKey CaptainExo002 { get; } = new CaptainExoKey("CaptainExo002");
            public static CaptainExoKey CaptainExo003 { get; } = new CaptainExoKey("CaptainExo003");
            public static CaptainExoKey CaptainExo004 { get; } = new CaptainExoKey("CaptainExo004");
            public static CaptainExoKey CaptainExo005 { get; } = new CaptainExoKey("CaptainExo005");
            public static CaptainExoKey CaptainExo006 { get; } = new CaptainExoKey("CaptainExo006");
            public static CaptainExoKey CaptainExo007 { get; } = new CaptainExoKey("CaptainExo007");
            public static CaptainExoKey CaptainExo008 { get; } = new CaptainExoKey("CaptainExo008");
            public static CaptainExoKey CaptainExo009 { get; } = new CaptainExoKey("CaptainExo009");
            public static CaptainExoKey CaptainExo010 { get; } = new CaptainExoKey("CaptainExo010");
            public static CaptainExoKey CaptainExo011 { get; } = new CaptainExoKey("CaptainExo011");
            public static CaptainExoKey CaptainExo012 { get; } = new CaptainExoKey("CaptainExo012");
            public static CaptainExoKey CaptainExo013 { get; } = new CaptainExoKey("CaptainExo013");
            public static CaptainExoKey CaptainExo014 { get; } = new CaptainExoKey("CaptainExo014");
            public static CaptainExoKey CaptainExo015 { get; } = new CaptainExoKey("CaptainExo015");

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        CaptainExo000, CaptainExo001, CaptainExo002, CaptainExo003, CaptainExo004, CaptainExo005,
                        CaptainExo006, CaptainExo007, CaptainExo008, CaptainExo009, CaptainExo010, CaptainExo011,
                        CaptainExo012, CaptainExo013, CaptainExo014, CaptainExo015
                    };
                }
            }
        }

        public static class Effects
        {
            public static EffectKey BuilderDrone { get; } = new EffectKey("BuilderDrone");
        }

        public static class Explosions
        {
            public static ExplosionKey BulletImpact { get; } = new ExplosionKey("BulletImpact");
            public static ExplosionKey HighCalibreBulletImpact { get; } = new ExplosionKey("HighCalibreBulletImpact");
            public static ExplosionKey TinyBulletImpact { get; } = new ExplosionKey("TinyBulletImpact");
            public static ExplosionKey NovaShellImpact { get; } = new ExplosionKey("NovaShellImpact");
            public static ExplosionKey RocketShellImpact { get; } = new ExplosionKey("RocketShellImpact");
            public static ExplosionKey BombExplosion { get; } = new ExplosionKey("ExplosionBomb");
            public static ExplosionKey FlakExplosion { get; } = new ExplosionKey("ExplosionSAM");
            public static ExplosionKey Explosion75 { get; } = new ExplosionKey("Explosion0.75");
            public static ExplosionKey Explosion100 { get; } = new ExplosionKey("Explosion1.0");
            public static ExplosionKey Explosion150 { get; } = new ExplosionKey("Explosion1.5");
            public static ExplosionKey Explosion500 { get; } = new ExplosionKey("Explosion5.0");

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        BulletImpact, HighCalibreBulletImpact, TinyBulletImpact, NovaShellImpact, RocketShellImpact, BombExplosion, FlakExplosion, Explosion75, Explosion100, Explosion150, Explosion500
                    };
                }
            }
        }

        public static class Projectiles
        {
            public static ProjectileKey Bullet { get; } = new ProjectileKey("Bullet");
            public static ProjectileKey HighCalibreBullet { get; } = new ProjectileKey("HighCalibreBullet");
            public static ProjectileKey TinyBullet { get; } = new ProjectileKey("TinyBullet");
            public static ProjectileKey ShellSmall { get; } = new ProjectileKey("ShellSmall");
            public static ProjectileKey ShellLarge { get; } = new ProjectileKey("ShellLarge");
            public static ProjectileKey NovaShell { get; } = new ProjectileKey("NovaShell");
            public static ProjectileKey RocketShell { get; } = new ProjectileKey("RocketShell");
            public static ProjectileKey MissileSmall { get; } = new ProjectileKey("MissileSmall");
            public static ProjectileKey MissileMedium { get; } = new ProjectileKey("MissileMedium");
            public static ProjectileKey MissileLarge { get; } = new ProjectileKey("MissileLarge");
            public static ProjectileKey MissileSmart { get; } = new ProjectileKey("MissileSmart");

            public static ProjectileKey Bomb { get; } = new ProjectileKey("Bomb");
            public static ProjectileKey Nuke { get; } = new ProjectileKey("Nuke");
            public static ProjectileKey Rocket { get; } = new ProjectileKey("Rocket");
            public static ProjectileKey RocketSmall { get; } = new ProjectileKey("RocketSmall");

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        Bullet, HighCalibreBullet, TinyBullet, ShellSmall, ShellLarge, NovaShell, RocketShell,
                        MissileSmall, MissileMedium, MissileLarge, MissileSmart,
                        Bomb, Nuke, Rocket, RocketSmall
                    };
                }
            }
        }

        public static class ShipDeaths
        {
            public static ShipDeathKey AttackBoat { get; } = new ShipDeathKey("AttackBoat");
            public static ShipDeathKey AttackRIB { get; } = new ShipDeathKey("AttackRIB");
            public static ShipDeathKey Frigate { get; } = new ShipDeathKey("Frigate");
            public static ShipDeathKey Destroyer { get; } = new ShipDeathKey("Destroyer");
            public static ShipDeathKey Archon { get; } = new ShipDeathKey("Archon");

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        AttackBoat,
                        Frigate,
                        Destroyer,
                        Archon,
                        AttackRIB
                    };
                }
            }
        }

        public static IPrefabKey AudioSource { get; } = new GenericKey("AudioSource", "UI/Sound");
    }
}
