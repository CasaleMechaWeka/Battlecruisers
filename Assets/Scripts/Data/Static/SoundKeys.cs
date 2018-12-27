using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data.Static
{
    public static class SoundKeys
    {
        public static class Deaths
        {
            public static ISoundKey Aircraft { get { return new SoundKey(SoundType.Deaths, "aircraft"); } }
            public static ISoundKey Ship { get { return new SoundKey(SoundType.Deaths, "ship"); } }
            public static ISoundKey Cruiser { get { return new SoundKey(SoundType.Deaths, "cruiser"); } }
            public static ISoundKey Building1 { get { return new SoundKey(SoundType.Deaths, "building1"); } }
            public static ISoundKey Building2 { get { return new SoundKey(SoundType.Deaths, "building2"); } }
            public static ISoundKey Building3 { get { return new SoundKey(SoundType.Deaths, "building3"); } }
            public static ISoundKey Building4 { get { return new SoundKey(SoundType.Deaths, "building4"); } }
            public static ISoundKey Building5 { get { return new SoundKey(SoundType.Deaths, "building5"); } }
        }

        public static class Engines
        {
            // Ships
			public static ISoundKey AtatckBoat{ get { return new SoundKey(SoundType.Engines, "attack-boat"); } }
			public static ISoundKey Frigate { get { return new SoundKey(SoundType.Engines, "frigate"); } }
			public static ISoundKey Destroyer { get { return new SoundKey(SoundType.Engines, "destroyer"); } }
            public static ISoundKey Archon { get { return new SoundKey(SoundType.Engines, "archon"); } }

            // Aircraft
            public static ISoundKey Bomber { get { return new SoundKey(SoundType.Engines, "bomber"); } }
            public static ISoundKey Gunship { get { return new SoundKey(SoundType.Engines, "gunship"); } }
            public static ISoundKey Fighter { get { return new SoundKey(SoundType.Engines, "fighter"); } }
        }

        public static class Firing
        {
            public static ISoundKey AntiAir { get { return new SoundKey(SoundType.Firing, "anti-air"); } }
            public static ISoundKey Artillery { get { return new SoundKey(SoundType.Firing, "artillery"); } }
            public static ISoundKey Broadsides { get { return new SoundKey(SoundType.Firing, "broadsides"); } }
			public static ISoundKey BigCannon { get { return new SoundKey(SoundType.Firing, "big-cannon"); } }
            public static ISoundKey Laser { get { return new SoundKey(SoundType.Firing, "laser"); } }
        }

        public static class Explosions
        {
            public static ISoundKey Bomb { get { return new SoundKey(SoundType.Explosions, "bomb"); } }
            public static ISoundKey Default { get { return new SoundKey(SoundType.Explosions, "default"); } }
        }

        public static class Completed
        {
            // Buildings
            public static ISoundKey AirFactory { get { return new SoundKey(SoundType.Completed, "air-factory"); } }
            public static ISoundKey AntiAirTurret { get { return new SoundKey(SoundType.Completed, "anti-air"); } }
            public static ISoundKey AntiShipTurret { get { return new SoundKey(SoundType.Completed, "anti-surface"); } }
            public static ISoundKey Artillery { get { return new SoundKey(SoundType.Completed, "artillery"); } }
            public static ISoundKey Booster { get { return new SoundKey(SoundType.Completed, "booster"); } }
            public static ISoundKey DroneStation { get { return new SoundKey(SoundType.Completed, "builders"); } }
            public static ISoundKey ControlTower { get { return new SoundKey(SoundType.Completed, "control-tower"); } }
            public static ISoundKey Mortar { get { return new SoundKey(SoundType.Completed, "mortar"); } }
            public static ISoundKey NavalFactory { get { return new SoundKey(SoundType.Completed, "naval-factory"); } }
            public static ISoundKey Railgun { get { return new SoundKey(SoundType.Completed, "railgun"); } }
            public static ISoundKey RocketLauncher { get { return new SoundKey(SoundType.Completed, "rocket-launcher"); } }
            public static ISoundKey SamSite { get { return new SoundKey(SoundType.Completed, "sam-site"); } }
            public static ISoundKey Shields { get { return new SoundKey(SoundType.Completed, "shields"); } }
            public static ISoundKey StealthGenerator { get { return new SoundKey(SoundType.Completed, "stealth-field"); } }
            public static ISoundKey TeslaCoil { get { return new SoundKey(SoundType.Completed, "tesla-coil"); } }

            // Units
            public static ISoundKey AttackBoat { get { return new SoundKey(SoundType.Completed, "attack-boat"); } }
            public static ISoundKey Frigate { get { return new SoundKey(SoundType.Completed, "frigate"); } }
            public static ISoundKey Destroyer { get { return new SoundKey(SoundType.Completed, "destroyer"); } }
            public static ISoundKey Bomber { get { return new SoundKey(SoundType.Completed, "bomber"); } }
            public static ISoundKey Gunship { get { return new SoundKey(SoundType.Completed, "gunship"); } }
            public static ISoundKey Fighter { get { return new SoundKey(SoundType.Completed, "fighter"); } }

            // Other
            public static ISoundKey SpySatellite { get { return new SoundKey(SoundType.Completed, "satellite"); } }
            public static ISoundKey Ultra { get { return new SoundKey(SoundType.Completed, "ultra"); } }
        }

        public static class Events
        {
            // Cruiser
            public static ISoundKey CruiserUnderAttack { get { return new SoundKey(SoundType.Events, "cruiser-under-attack"); } }
            public static ISoundKey CruiserSignificantlyDamaged { get { return new SoundKey(SoundType.Events, "cruiser-significantly-damaged"); } }
            
            // Drones
            public static ISoundKey DronesNewDronesReady { get { return new SoundKey(SoundType.Events, "drones-new-drones-ready"); } }
            public static ISoundKey DronesIdle { get { return new SoundKey(SoundType.Events, "drones-idle"); } }
            public static ISoundKey DronesNotEnoughDronesToBuild { get { return new SoundKey(SoundType.Events, "drones-not-enough-drones-to-build"); } }
            public static ISoundKey DronesNotEnoughDronesToFocus { get { return new SoundKey(SoundType.Events, "drones-not-enough-drones-to-focus"); } }
            public static ISoundKey DronesFocusing { get { return new SoundKey(SoundType.Events, "drones-focusing"); } }
            public static ISoundKey DronesAllFocused { get { return new SoundKey(SoundType.Events, "drones-all-focused"); } }
            public static ISoundKey DronesDispersing { get { return new SoundKey(SoundType.Events, "drones-dispersing"); } }

            // Other
            public static ISoundKey EnemyStartedUltra { get { return new SoundKey(SoundType.Events, "enemy-started-ultra"); } }
            public static ISoundKey ShieldsDown { get { return new SoundKey(SoundType.Events, "shields-down"); } }
            public static ISoundKey TargettingNewTarget { get { return new SoundKey(SoundType.Events, "targeting-new-target"); } }
            public static ISoundKey TargettingDeselected { get { return new SoundKey(SoundType.Events, "targeting-untargeted"); } }
        }

        public static class Music
        {
            public static ISoundKey MainTheme { get { return new SoundKey(SoundType.Music, "main-theme"); } }
            public static ISoundKey Danger { get { return new SoundKey(SoundType.Music, "danger"); } }
            public static ISoundKey Victory { get { return new SoundKey(SoundType.Music, "victory"); } }
            
            public static class Background
            {
                public static ISoundKey Kentient { get { return new SoundKey(SoundType.Music, "kentient"); } }
                public static ISoundKey Experimental { get { return new SoundKey(SoundType.Music, "experimental"); } }
                public static ISoundKey Bobby { get { return new SoundKey(SoundType.Music, "bobby"); } }
            }
        }
    }
}
