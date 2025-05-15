using BattleCruisers.UI.Sound;
using System.Collections.Generic;

namespace BattleCruisers.Data.Static
{
    public static class SoundKeys
    {
        public static class Firing
        {
            public static SoundKey AntiAir { get; } = new SoundKey(SoundType.Firing, "anti-air");
            public static SoundKey Artillery { get; } = new SoundKey(SoundType.Firing, "artillery");
            public static SoundKey AttackBoat { get; } = new SoundKey(SoundType.Firing, "small-cannon");
            public static SoundKey Broadsides { get; } = new SoundKey(SoundType.Firing, "broadsides");
            public static SoundKey BigCannon { get; } = new SoundKey(SoundType.Firing, "big-cannon");
            public static SoundKey Laser { get; } = new SoundKey(SoundType.Firing, "laser");
            public static SoundKey RocketLauncher { get; } = new SoundKey(SoundType.Firing, "rocket-launcher");
            public static SoundKey Missile { get; } = new SoundKey(SoundType.Firing, "SAMFiring");
            public static SoundKey Lightning { get; } = new SoundKey(SoundType.Firing, "TeslaCoil");
            public static SoundKey RailCannon { get; } = new SoundKey(SoundType.Firing, "RailCannon");

            public static SoundKey PneumaticSlug { get; } = new SoundKey(SoundType.Firing, "PneumaticSlug");
        }

        public static class Explosions
        {
            public static SoundKey Bomb { get; } = new SoundKey(SoundType.Explosions, "bomb");
            public static SoundKey Default { get; } = new SoundKey(SoundType.Explosions, "default");
        }

        public static class Completed
        {
            // Buildings
            public static SoundKey AirFactory { get; } = new SoundKey(SoundType.Completed, "air-factory");
            public static SoundKey AntiAirTurret { get; } = new SoundKey(SoundType.Completed, "anti-air");
            public static SoundKey AntiShipTurret { get; } = new SoundKey(SoundType.Completed, "anti-surface");
            public static SoundKey Artillery { get; } = new SoundKey(SoundType.Completed, "artillery");
            public static SoundKey Booster { get; } = new SoundKey(SoundType.Completed, "booster");
            public static SoundKey DroneStation { get; } = new SoundKey(SoundType.Completed, "builders");
            public static SoundKey ControlTower { get; } = new SoundKey(SoundType.Completed, "control-tower");
            public static SoundKey Mortar { get; } = new SoundKey(SoundType.Completed, "mortar");
            public static SoundKey NavalFactory { get; } = new SoundKey(SoundType.Completed, "naval-factory");
            public static SoundKey Railgun { get; } = new SoundKey(SoundType.Completed, "railgun");
            public static SoundKey RocketLauncher { get; } = new SoundKey(SoundType.Completed, "rocket-launcher");
            public static SoundKey SamSite { get; } = new SoundKey(SoundType.Completed, "sam-site");
            public static SoundKey Shields { get; } = new SoundKey(SoundType.Completed, "shields");
            public static SoundKey StealthGenerator { get; } = new SoundKey(SoundType.Completed, "stealth-field");
            public static SoundKey TeslaCoil { get; } = new SoundKey(SoundType.Completed, "tesla-coil");

            // Units
            public static SoundKey AircraftReady { get; } = new SoundKey(SoundType.Completed, "AircraftReadyWhoosh");
            public static SoundKey ShipReady { get; } = new SoundKey(SoundType.Completed, "ShipReadyWhoosh");

            // Other
            public static SoundKey SpySatellite { get; } = new SoundKey(SoundType.Completed, "satellite");
            public static SoundKey Ultra { get; } = new SoundKey(SoundType.Completed, "ultra");
        }

        public static class Events
        {
            // Cruiser
            public static SoundKey CruiserUnderAttack { get; } = new SoundKey(SoundType.Events, "cruiser-under-attack");
            public static SoundKey CruiserSignificantlyDamaged { get; } = new SoundKey(SoundType.Events, "cruiser-significantly-damaged");
            public static SoundKey NoBuildingSlotsLeft { get; } = new SoundKey(SoundType.Events, "no-building-slots-left");

            // Drones
            public static SoundKey DronesNewDronesReady { get; } = new SoundKey(SoundType.Events, "drones-new-drones-ready");
            public static SoundKey DronesIdle { get; } = new SoundKey(SoundType.Events, "drones-idle");
            public static SoundKey DronesNotEnoughDronesToBuild { get; } = new SoundKey(SoundType.Events, "drones-not-enough-drones-to-build");
            public static SoundKey DronesNotEnoughDronesToFocus { get; } = new SoundKey(SoundType.Events, "drones-not-enough-drones-to-focus");
            public static SoundKey DronesFocusing { get; } = new SoundKey(SoundType.Events, "drones-focusing");
            public static SoundKey DronesAllFocused { get; } = new SoundKey(SoundType.Events, "drones-all-focused");
            public static SoundKey DronesDispersing { get; } = new SoundKey(SoundType.Events, "drones-dispersing");

            // Other
            public static SoundKey EnemyStartedUltra { get; } = new SoundKey(SoundType.Events, "enemy-started-ultra");
            public static SoundKey FactoryIncomplete { get; } = new SoundKey(SoundType.Events, "wait-for-factory-to-complete");
            public static SoundKey PopulationLimitReached { get; } = new SoundKey(SoundType.Events, "population-limit-reached");
            public static SoundKey TargettingNewTarget { get; } = new SoundKey(SoundType.Events, "targeting-new-target");
            public static SoundKey TargettingDeselected { get; } = new SoundKey(SoundType.Events, "targeting-untargeted");
        }

        public static class Music
        {
            public static SoundKey MainTheme { get; } = new SoundKey(SoundType.Music, "main-theme");
            public static SoundKey Victory { get; } = new SoundKey(SoundType.Music, "victory");
            public static SoundKey Defeat { get; } = new SoundKey(SoundType.Music, "defeat");
            public static SoundKey TrashTalk { get; } = new SoundKey(SoundType.Music, "TrashTalk");
            public static SoundKey Cutscene { get; } = new SoundKey(SoundType.Music, "OrionOmega");
            public static SoundKey Credits { get; } = new SoundKey(SoundType.Music, "SerialKillerRemaster");
            public static SoundKey Advertisements { get; } = new SoundKey(SoundType.Music, "FullscreenAdsMusic");

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

                public static SoundKeyPair Againagain { get; }
                    = new SoundKeyPair(
                        new SoundKey(SoundType.Music, "againagain-base"),
                        new SoundKey(SoundType.Music, "againagain-danger"));

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

                public static SoundKeyPair Fortress { get; }
                    = new SoundKeyPair(
                        new SoundKey(SoundType.Music, "fortress-base"),
                        new SoundKey(SoundType.Music, "fortress-danger"));


                public static SoundKeyPair Boss { get; }
                = new SoundKeyPair(
                    new SoundKey(SoundType.Music, "fortress-base"),
                    new SoundKey(SoundType.Music, "fortress-danger"));

                public static IList<SoundKeyPair> All = new List<SoundKeyPair>()
                {
                    Bobby,
                    Confusion,
                    Experimental,
                    Juggernaut,
                    Nothing,
                    Sleeper,
                    Fortress,
                    Boss
                };
            }
        }

        public static class UI
        {
            public static SoundKey Click { get; } = new SoundKey(SoundType.UI, "default-click");
            public static SoundKey ChunkyClick { get; } = new SoundKey(SoundType.UI, "chunky-click");
            public static SoundKey Delete { get; } = new SoundKey(SoundType.UI, "delete");
            public static SoundKey ScreenChange { get; } = new SoundKey(SoundType.UI, "screen-change");

            public static class Selected
            {
                public static SoundKey EnemyCruiser { get; } = new SoundKey(SoundType.UI, "EnemyCruiserSelected");
                public static SoundKey FriendlyCruiser { get; } = new SoundKey(SoundType.UI, "FriendlyCruiserSelected");
            }
        }

        public static class Shields
        {
            public static SoundKey FullyCharged { get; } = new SoundKey(SoundType.Shields, "shields-blinking-on");
            public static SoundKey HitWhileActive { get; } = new SoundKey(SoundType.Shields, "shield-bracing");
            public static SoundKey FullyDepleted { get; } = new SoundKey(SoundType.Shields, "shields-broken");
        }

        public static class AltDrones
        {
            public static SoundKey AllDronesFocused { get; } = new SoundKey(SoundType.Events, "AllDronesFocusedAlt");
            public static SoundKey BuildersReadyAlt { get; } = new SoundKey(SoundType.Events, "BuildersReadyAlt");
            public static SoundKey BuildingReadyAlt { get; } = new SoundKey(SoundType.Events, "BuildingReadyAlt");
            public static SoundKey CruiserSignificantlyDamagedAlt { get; } = new SoundKey(SoundType.Events, "CruiserSignificantlyDamagedAlt");
            public static SoundKey CruiserUnderAttackAlt { get; } = new SoundKey(SoundType.Events, "CruiserUnderAttackAlt");
            public static SoundKey DispersingAlt { get; } = new SoundKey(SoundType.Events, "DispersingAlt");
            public static SoundKey DronesIdleAlt { get; } = new SoundKey(SoundType.Events, "DronesIdleAlt");
            public static SoundKey EnemyBuildingUltraAlt { get; } = new SoundKey(SoundType.Events, "EnemyBuildingUltraAlt");
            public static SoundKey FocusingAlt { get; } = new SoundKey(SoundType.Events, "FocusingAlt");
            public static SoundKey InsufficientBuildersAlt { get; } = new SoundKey(SoundType.Events, "InsufficientBuildersAlt");
            public static SoundKey NowhereToBuildAlt { get; } = new SoundKey(SoundType.Events, "NowhereToBuildAlt");
            public static SoundKey PopCapReachedAlt { get; } = new SoundKey(SoundType.Events, "PopCapReachedAlt");
            public static SoundKey TargetingAlt { get; } = new SoundKey(SoundType.Events, "TargetingAlt");
            public static SoundKey UntargetAlt { get; } = new SoundKey(SoundType.Events, "UntargetAlt");
            public static SoundKey WaitForFactoryToCompleteAlt { get; } = new SoundKey(SoundType.Events, "WaitForFactoryToCompleteAlt");
            public static SoundKey UltraReadyAlt { get; } = new SoundKey(SoundType.Events, "UltraReadyAlt");
        }
    }
}
