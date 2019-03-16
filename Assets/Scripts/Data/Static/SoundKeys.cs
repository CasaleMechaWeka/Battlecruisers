using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data.Static
{
    public static class SoundKeys
    {
        public static class Deaths
        {
            public static ISoundKey Aircraft => new SoundKey(SoundType.Deaths, "aircraft");
            public static ISoundKey Ship => new SoundKey(SoundType.Deaths, "ship");
            public static ISoundKey Cruiser => new SoundKey(SoundType.Deaths, "cruiser");
            public static ISoundKey Building1 => new SoundKey(SoundType.Deaths, "building1");
            public static ISoundKey Building2 => new SoundKey(SoundType.Deaths, "building2");
            public static ISoundKey Building3 => new SoundKey(SoundType.Deaths, "building3");
            public static ISoundKey Building4 => new SoundKey(SoundType.Deaths, "building4");
            public static ISoundKey Building5 => new SoundKey(SoundType.Deaths, "building5");
        }

        public static class Engines
        {
            // Ships
			public static ISoundKey AtatckBoat=> new SoundKey(SoundType.Engines, "attack-boat");
			public static ISoundKey Frigate => new SoundKey(SoundType.Engines, "frigate");
			public static ISoundKey Destroyer => new SoundKey(SoundType.Engines, "destroyer");
            public static ISoundKey Archon => new SoundKey(SoundType.Engines, "archon");

            // Aircraft
            public static ISoundKey Bomber => new SoundKey(SoundType.Engines, "bomber");
            public static ISoundKey Gunship => new SoundKey(SoundType.Engines, "gunship");
            public static ISoundKey Fighter => new SoundKey(SoundType.Engines, "fighter");
        }

        public static class Firing
        {
            public static ISoundKey AntiAir => new SoundKey(SoundType.Firing, "anti-air");
            public static ISoundKey Artillery => new SoundKey(SoundType.Firing, "artillery");
            public static ISoundKey Broadsides => new SoundKey(SoundType.Firing, "broadsides");
			public static ISoundKey BigCannon => new SoundKey(SoundType.Firing, "big-cannon");
            public static ISoundKey Laser => new SoundKey(SoundType.Firing, "laser");
        }

        public static class Explosions
        {
            public static ISoundKey Bomb => new SoundKey(SoundType.Explosions, "bomb");
            public static ISoundKey Default => new SoundKey(SoundType.Explosions, "default");
        }

        public static class Completed
        {
            // Buildings
            public static ISoundKey AirFactory => new SoundKey(SoundType.Completed, "air-factory");
            public static ISoundKey AntiAirTurret => new SoundKey(SoundType.Completed, "anti-air");
            public static ISoundKey AntiShipTurret => new SoundKey(SoundType.Completed, "anti-surface");
            public static ISoundKey Artillery => new SoundKey(SoundType.Completed, "artillery");
            public static ISoundKey Booster => new SoundKey(SoundType.Completed, "booster");
            public static ISoundKey DroneStation => new SoundKey(SoundType.Completed, "builders");
            public static ISoundKey ControlTower => new SoundKey(SoundType.Completed, "control-tower");
            public static ISoundKey Mortar => new SoundKey(SoundType.Completed, "mortar");
            public static ISoundKey NavalFactory => new SoundKey(SoundType.Completed, "naval-factory");
            public static ISoundKey Railgun => new SoundKey(SoundType.Completed, "railgun");
            public static ISoundKey RocketLauncher => new SoundKey(SoundType.Completed, "rocket-launcher");
            public static ISoundKey SamSite => new SoundKey(SoundType.Completed, "sam-site");
            public static ISoundKey Shields => new SoundKey(SoundType.Completed, "shields");
            public static ISoundKey StealthGenerator => new SoundKey(SoundType.Completed, "stealth-field");
            public static ISoundKey TeslaCoil => new SoundKey(SoundType.Completed, "tesla-coil");

            // Units
            public static ISoundKey AttackBoat => new SoundKey(SoundType.Completed, "attack-boat");
            public static ISoundKey Frigate => new SoundKey(SoundType.Completed, "frigate");
            public static ISoundKey Destroyer => new SoundKey(SoundType.Completed, "destroyer");
            public static ISoundKey Bomber => new SoundKey(SoundType.Completed, "bomber");
            public static ISoundKey Gunship => new SoundKey(SoundType.Completed, "gunship");
            public static ISoundKey Fighter => new SoundKey(SoundType.Completed, "fighter");

            // Other
            public static ISoundKey SpySatellite => new SoundKey(SoundType.Completed, "satellite");
            public static ISoundKey Ultra => new SoundKey(SoundType.Completed, "ultra");
        }

        public static class Events
        {
            // Cruiser
            public static ISoundKey CruiserUnderAttack => new SoundKey(SoundType.Events, "cruiser-under-attack");
            public static ISoundKey CruiserSignificantlyDamaged => new SoundKey(SoundType.Events, "cruiser-significantly-damaged");
            
            // Drones
            public static ISoundKey DronesNewDronesReady => new SoundKey(SoundType.Events, "drones-new-drones-ready");
            public static ISoundKey DronesIdle => new SoundKey(SoundType.Events, "drones-idle");
            public static ISoundKey DronesNotEnoughDronesToBuild => new SoundKey(SoundType.Events, "drones-not-enough-drones-to-build");
            public static ISoundKey DronesNotEnoughDronesToFocus => new SoundKey(SoundType.Events, "drones-not-enough-drones-to-focus");
            public static ISoundKey DronesFocusing => new SoundKey(SoundType.Events, "drones-focusing");
            public static ISoundKey DronesAllFocused => new SoundKey(SoundType.Events, "drones-all-focused");
            public static ISoundKey DronesDispersing => new SoundKey(SoundType.Events, "drones-dispersing");

            // Other
            public static ISoundKey EnemyStartedUltra => new SoundKey(SoundType.Events, "enemy-started-ultra");
            public static ISoundKey ShieldsDown => new SoundKey(SoundType.Events, "shields-down");
            public static ISoundKey TargettingNewTarget => new SoundKey(SoundType.Events, "targeting-new-target");
            public static ISoundKey TargettingDeselected => new SoundKey(SoundType.Events, "targeting-untargeted");
        }

        public static class Music
        {
            public static ISoundKey MainTheme => new SoundKey(SoundType.Music, "main-theme");
            public static ISoundKey Danger => new SoundKey(SoundType.Music, "danger");
            public static ISoundKey Victory => new SoundKey(SoundType.Music, "victory");
            
            public static class Background
            {
                public static ISoundKey Kentient => new SoundKey(SoundType.Music, "kentient");
                public static ISoundKey Experimental => new SoundKey(SoundType.Music, "experimental");
                public static ISoundKey Bobby => new SoundKey(SoundType.Music, "bobby");
            }
        }
    }
}
