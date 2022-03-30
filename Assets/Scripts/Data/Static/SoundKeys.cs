using BattleCruisers.UI.Sound;
using System.Collections.Generic;

namespace BattleCruisers.Data.Static
{
    public static class SoundKeys
    {
        public static class Firing
        {
            public static ISoundKey AntiAir { get; } = new SoundKey(SoundType.Firing, "anti-air");
            public static ISoundKey Artillery { get; } = new SoundKey(SoundType.Firing, "artillery");
            public static ISoundKey AttackBoat { get; } = new SoundKey(SoundType.Firing, "small-cannon");
            public static ISoundKey Broadsides { get; } = new SoundKey(SoundType.Firing, "broadsides");
			public static ISoundKey BigCannon { get; } = new SoundKey(SoundType.Firing, "big-cannon");
            public static ISoundKey Laser { get; } = new SoundKey(SoundType.Firing, "laser");
            public static ISoundKey RocketLauncher { get; } = new SoundKey(SoundType.Firing, "rocket-launcher");
            public static ISoundKey Missile { get; } = new SoundKey(SoundType.Firing, "SAMFiring");
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
            public static ISoundKey AircraftReady { get; } = new SoundKey(SoundType.Completed, "AircraftReadyWhoosh");
            public static ISoundKey ShipReady { get; } = new SoundKey(SoundType.Completed, "ShipReadyWhoosh");

            // Other
            public static ISoundKey SpySatellite { get; } = new SoundKey(SoundType.Completed, "satellite");
            public static ISoundKey Ultra { get; } = new SoundKey(SoundType.Completed, "ultra");
        }

        public static class Events
        {
            // Cruiser
            public static ISoundKey CruiserUnderAttack { get; } = new SoundKey(SoundType.Events, "cruiser-under-attack");
            public static ISoundKey CruiserSignificantlyDamaged { get; } = new SoundKey(SoundType.Events, "cruiser-significantly-damaged");
            public static ISoundKey NoBuildingSlotsLeft { get; } = new SoundKey(SoundType.Events, "no-building-slots-left");
            
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
            public static ISoundKey TargettingNewTarget { get; } = new SoundKey(SoundType.Events, "targeting-new-target");
            public static ISoundKey TargettingDeselected { get; } = new SoundKey(SoundType.Events, "targeting-untargeted");
        }

        public static class Music
        {
            public static ISoundKey MainTheme { get; } = new SoundKey(SoundType.Music, "main-theme");
            public static ISoundKey Victory { get; } = new SoundKey(SoundType.Music, "victory");
            public static ISoundKey Defeat { get; } = new SoundKey(SoundType.Music, "defeat");
            public static ISoundKey TrashTalk { get; } = new SoundKey(SoundType.Music, "TrashTalk");
            public static ISoundKey Cutscene { get; } = new SoundKey(SoundType.Music, "OrionOmega");
            public static ISoundKey Credits { get; } = new SoundKey(SoundType.Music, "SerialKillerRemaster");
            
            public static class Background
            {
                public static SoundKeyPair Bobby { get; }
                    = new SoundKeyPair(
                        new SoundKey(SoundType.Music, "bobby-base"),
                        new SoundKey(SoundType.Music, "bobby-danger"));

                public static SoundKeyPair Confusion { get; }
                    = new SoundKeyPair(
                        new SoundKey(SoundType.Music, "confusion-base"),
                        new SoundKey(SoundType.Music, "confusion-danger"));

                public static SoundKeyPair Experimental { get; }
                    = new SoundKeyPair(
                        new SoundKey(SoundType.Music, "experimental-base"),
                        new SoundKey(SoundType.Music, "experimental-danger"));

                public static SoundKeyPair Juggernaut { get; }
                    = new SoundKeyPair(
                        new SoundKey(SoundType.Music, "juggernaut-base"),
                        new SoundKey(SoundType.Music, "juggernaut-danger"));
                
                public static SoundKeyPair Nothing { get; }
                    = new SoundKeyPair(
                        new SoundKey(SoundType.Music, "nothing-base"),
                        new SoundKey(SoundType.Music, "nothing-danger"));
                
                public static SoundKeyPair Sleeper { get; }
                    = new SoundKeyPair(
                        new SoundKey(SoundType.Music, "sleeper-base"),
                        new SoundKey(SoundType.Music, "sleeper-danger"));

                public static SoundKeyPair Boss { get; }
                = new SoundKeyPair(
                    new SoundKey(SoundType.Music, "juggernaut-danger"),
                    new SoundKey(SoundType.Music, "juggernaut-danger"));

                public static IList<SoundKeyPair> All = new List<SoundKeyPair>()
                {
                    Bobby,
                    Confusion,
                    Experimental,
                    Juggernaut,
                    Nothing,
                    Sleeper,
                    Boss
                };
            }
        }

        public static class UI
        {
            public static ISoundKey Click { get; } = new SoundKey(SoundType.UI, "default-click");
            public static ISoundKey ChunkyClick { get; } = new SoundKey(SoundType.UI, "chunky-click");
            public static ISoundKey Delete { get; } = new SoundKey(SoundType.UI, "delete");
            public static ISoundKey ScreenChange { get; } = new SoundKey(SoundType.UI, "screen-change");

            public static class Selected
            {
                public static ISoundKey EnemyCruiser { get; } = new SoundKey(SoundType.UI, "EnemyCruiserSelected");
                public static ISoundKey FriendlyCruiser { get; } = new SoundKey(SoundType.UI, "FriendlyCruiserSelected");
            }
        }

        public static class Shields
        {
            public static ISoundKey FullyCharged { get; } = new SoundKey(SoundType.Shields, "shields-blinking-on");
            public static ISoundKey HitWhileActive { get; } = new SoundKey(SoundType.Shields, "shield-bracing");
            public static ISoundKey FullyDepleted { get; } = new SoundKey(SoundType.Shields, "shields-broken");
        }

        public static class AltDrones
        {
            public static ISoundKey AllDronesFocused { get; } = new SoundKey(SoundType.Events, "AllDronesFocusedAlt");
            public static ISoundKey BuildersReadyAlt { get; } = new SoundKey(SoundType.Events, "BuildersReadyAlt");
            public static ISoundKey BuildingReadyAlt { get; } = new SoundKey(SoundType.Events, "BuildingReadyAlt");
            public static ISoundKey CruiserSignificantlyDamagedAlt { get; } = new SoundKey(SoundType.Events, "CruiserSignificantlyDamagedAlt");
            public static ISoundKey CruiserUnderAttackAlt { get; } = new SoundKey(SoundType.Events, "CruiserUnderAttackAlt");
            public static ISoundKey DispersingAlt { get; } = new SoundKey(SoundType.Events, "DispersingAlt");
            public static ISoundKey DronesIdleAlt { get; } = new SoundKey(SoundType.Events, "DronesIdleAlt");
            public static ISoundKey EnemyBuildingUltraAlt { get; } = new SoundKey(SoundType.Events, "EnemyBuildingUltraAlt");
            public static ISoundKey FocusingAlt { get; } = new SoundKey(SoundType.Events, "FocusingAlt");
            public static ISoundKey InsufficientBuildersAlt { get; } = new SoundKey(SoundType.Events, "InsufficientBuildersAlt");
            public static ISoundKey NowhereToBuildAlt { get; } = new SoundKey(SoundType.Events, "NowhereToBuildAlt");
            public static ISoundKey PopCapReachedAlt { get; } = new SoundKey(SoundType.Events, "PopCapReachedAlt");
            public static ISoundKey TargetingAlt { get; } = new SoundKey(SoundType.Events, "TargetingAlt");
            public static ISoundKey UntargetAlt { get; } = new SoundKey(SoundType.Events, "UntargetAlt");
            public static ISoundKey WaitForFactoryToCompleteAlt { get; } = new SoundKey(SoundType.Events, "WaitForFactoryToCompleteAlt");
            public static ISoundKey UltraReadyAlt { get; } = new SoundKey(SoundType.Events, "UltraReadyAlt");
        }
    }
}
