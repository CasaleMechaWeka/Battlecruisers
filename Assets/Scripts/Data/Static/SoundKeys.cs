 using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data.Static
{
    public static class SoundKeys
    {
        public static class Deaths
        {
            public static ISoundKey Aircraft { get; } = new SoundKey(SoundType.Deaths, "aircraft");
            public static ISoundKey Ship { get; } = new SoundKey(SoundType.Deaths, "ship");
            public static ISoundKey Cruiser { get; } = new SoundKey(SoundType.Deaths, "cruiser");
            public static ISoundKey Building1 { get; } = new SoundKey(SoundType.Deaths, "building1");
            public static ISoundKey Building2 { get; } = new SoundKey(SoundType.Deaths, "building2");
            public static ISoundKey Building3 { get; } = new SoundKey(SoundType.Deaths, "building3");
            public static ISoundKey Building4 { get; } = new SoundKey(SoundType.Deaths, "building4");
            public static ISoundKey Building5 { get; } = new SoundKey(SoundType.Deaths, "building5");
        }

        public static class Engines
        {
            // Ships
			public static ISoundKey AtatckBoat{ get; } = new SoundKey(SoundType.Engines, "attack-boat");
			public static ISoundKey Frigate { get; } = new SoundKey(SoundType.Engines, "frigate");
			public static ISoundKey Destroyer { get; } = new SoundKey(SoundType.Engines, "destroyer");
            public static ISoundKey Archon { get; } = new SoundKey(SoundType.Engines, "archon");

            // Aircraft
            public static ISoundKey Bomber { get; } = new SoundKey(SoundType.Engines, "bomber");
            public static ISoundKey Gunship { get; } = new SoundKey(SoundType.Engines, "gunship");
            public static ISoundKey Fighter { get; } = new SoundKey(SoundType.Engines, "fighter");
        }

        public static class Firing
        {
            public static ISoundKey AntiAir { get; } = new SoundKey(SoundType.Firing, "anti-air");
            public static ISoundKey Artillery { get; } = new SoundKey(SoundType.Firing, "artillery");
            public static ISoundKey Broadsides { get; } = new SoundKey(SoundType.Firing, "broadsides");
			public static ISoundKey BigCannon { get; } = new SoundKey(SoundType.Firing, "big-cannon");
            public static ISoundKey Laser { get; } = new SoundKey(SoundType.Firing, "laser");
        }

        public static class Explosions
        {
            public static ISoundKey Bomb { get; } = new SoundKey(SoundType.Explosions, "bomb");
            public static ISoundKey Default { get; } = new SoundKey(SoundType.Explosions, "default");
        }

        public static class Completed
        {
            // Buildings
            public static ISoundKey AirFactory { get; } = new SoundKey(SoundType.Completed, "air-factory");
            public static ISoundKey AntiAirTurret { get; } = new SoundKey(SoundType.Completed, "anti-air");
            public static ISoundKey AntiShipTurret { get; } = new SoundKey(SoundType.Completed, "anti-surface");
            public static ISoundKey Artillery { get; } = new SoundKey(SoundType.Completed, "artillery");
            public static ISoundKey Booster { get; } = new SoundKey(SoundType.Completed, "booster");
            public static ISoundKey DroneStation { get; } = new SoundKey(SoundType.Completed, "builders");
            public static ISoundKey ControlTower { get; } = new SoundKey(SoundType.Completed, "control-tower");
            public static ISoundKey Mortar { get; } = new SoundKey(SoundType.Completed, "mortar");
            public static ISoundKey NavalFactory { get; } = new SoundKey(SoundType.Completed, "naval-factory");
            public static ISoundKey Railgun { get; } = new SoundKey(SoundType.Completed, "railgun");
            public static ISoundKey RocketLauncher { get; } = new SoundKey(SoundType.Completed, "rocket-launcher");
            public static ISoundKey SamSite { get; } = new SoundKey(SoundType.Completed, "sam-site");
            public static ISoundKey Shields { get; } = new SoundKey(SoundType.Completed, "shields");
            public static ISoundKey StealthGenerator { get; } = new SoundKey(SoundType.Completed, "stealth-field");
            public static ISoundKey TeslaCoil { get; } = new SoundKey(SoundType.Completed, "tesla-coil");

            // Units
            public static ISoundKey AttackBoat { get; } = new SoundKey(SoundType.Completed, "attack-boat");
            public static ISoundKey Frigate { get; } = new SoundKey(SoundType.Completed, "frigate");
            public static ISoundKey Destroyer { get; } = new SoundKey(SoundType.Completed, "destroyer");
            public static ISoundKey Bomber { get; } = new SoundKey(SoundType.Completed, "bomber");
            public static ISoundKey Gunship { get; } = new SoundKey(SoundType.Completed, "gunship");
            public static ISoundKey Fighter { get; } = new SoundKey(SoundType.Completed, "fighter");

            // Other
            public static ISoundKey SpySatellite { get; } = new SoundKey(SoundType.Completed, "satellite");
            public static ISoundKey Ultra { get; } = new SoundKey(SoundType.Completed, "ultra");
        }

        public static class Events
        {
            // Cruiser
            public static ISoundKey CruiserUnderAttack { get; } = new SoundKey(SoundType.Events, "cruiser-under-attack");
            public static ISoundKey CruiserSignificantlyDamaged { get; } = new SoundKey(SoundType.Events, "cruiser-significantly-damaged");
            
            // Drones
            public static ISoundKey DronesNewDronesReady { get; } = new SoundKey(SoundType.Events, "drones-new-drones-ready");
            public static ISoundKey DronesIdle { get; } = new SoundKey(SoundType.Events, "drones-idle");
            public static ISoundKey DronesNotEnoughDronesToBuild { get; } = new SoundKey(SoundType.Events, "drones-not-enough-drones-to-build");
            public static ISoundKey DronesNotEnoughDronesToFocus { get; } = new SoundKey(SoundType.Events, "drones-not-enough-drones-to-focus");
            public static ISoundKey DronesFocusing { get; } = new SoundKey(SoundType.Events, "drones-focusing");
            public static ISoundKey DronesAllFocused { get; } = new SoundKey(SoundType.Events, "drones-all-focused");
            public static ISoundKey DronesDispersing { get; } = new SoundKey(SoundType.Events, "drones-dispersing");

            // Other
            public static ISoundKey EnemyStartedUltra { get; } = new SoundKey(SoundType.Events, "enemy-started-ultra");
            public static ISoundKey FactoryIncomplete { get; } = new SoundKey(SoundType.Events, "wait-for-factory-to-complete");
            public static ISoundKey PopulationLimitReached { get; } = new SoundKey(SoundType.Events, "population-limit-reached");
            public static ISoundKey ShieldsDown { get; } = new SoundKey(SoundType.Events, "shields-down");
            public static ISoundKey TargettingNewTarget { get; } = new SoundKey(SoundType.Events, "targeting-new-target");
            public static ISoundKey TargettingDeselected { get; } = new SoundKey(SoundType.Events, "targeting-untargeted");
        }

        public static class Music
        {
            public static ISoundKey MainTheme { get; } = new SoundKey(SoundType.Music, "main-theme");
            public static ISoundKey Danger { get; } = new SoundKey(SoundType.Music, "danger");
            public static ISoundKey Victory { get; } = new SoundKey(SoundType.Music, "victory");
            public static ISoundKey Defeat { get; } = new SoundKey(SoundType.Music, "defeat");
            public static ISoundKey Loading { get; } = new SoundKey(SoundType.Music, "loading");
            
            public static class Background
            {
                public static ISoundKey Kentient { get; } = new SoundKey(SoundType.Music, "kentient");
                public static ISoundKey Experimental { get; } = new SoundKey(SoundType.Music, "experimental");
                public static ISoundKey Bobby { get; } = new SoundKey(SoundType.Music, "bobby");
            }
        }

        public static class UI
        {
            public static ISoundKey Click { get; } = new SoundKey(SoundType.UI, "default-click");
            public static ISoundKey ScreenChange { get; } = new SoundKey(SoundType.UI, "screen-change");
        }

        public static class Shields
        {
            public static ISoundKey FullyCharged { get; } = new SoundKey(SoundType.Shields, "shields-blinking-on");
            public static ISoundKey HitWhileActive { get; } = new SoundKey(SoundType.Shields, "shield-bracing");
            public static ISoundKey FullyDepleted { get; } = new SoundKey(SoundType.Shields, "shields-broken");
        }
    }
}
