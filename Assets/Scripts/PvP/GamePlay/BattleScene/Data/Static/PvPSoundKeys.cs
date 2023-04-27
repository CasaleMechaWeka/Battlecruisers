using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static
{
    public static class PvPSoundKeys
    {
        public static class PvPFiring
        {
            public static IPvPSoundKey AntiAir { get; } = new PvPSoundKey(PvPSoundType.Firing, "anti-air");
            public static IPvPSoundKey Artillery { get; } = new PvPSoundKey(PvPSoundType.Firing, "artillery");
            public static IPvPSoundKey AttackBoat { get; } = new PvPSoundKey(PvPSoundType.Firing, "small-cannon");
            public static IPvPSoundKey Broadsides { get; } = new PvPSoundKey(PvPSoundType.Firing, "broadsides");
            public static IPvPSoundKey BigCannon { get; } = new PvPSoundKey(PvPSoundType.Firing, "big-cannon");
            public static IPvPSoundKey Laser { get; } = new PvPSoundKey(PvPSoundType.Firing, "laser");
            public static IPvPSoundKey RocketLauncher { get; } = new PvPSoundKey(PvPSoundType.Firing, "rocket-launcher");
            public static IPvPSoundKey Missile { get; } = new PvPSoundKey(PvPSoundType.Firing, "SAMFiring");
        }

        public static class PvPExplosions
        {
            public static IPvPSoundKey Bomb { get; } = new PvPSoundKey(PvPSoundType.Explosions, "bomb");
            public static IPvPSoundKey Default { get; } = new PvPSoundKey(PvPSoundType.Explosions, "default");
        }

        public static class PvPCompleted
        {
            // Buildings
            public static IPvPSoundKey AirFactory { get; } = new PvPSoundKey(PvPSoundType.Completed, "air-factory");
            public static IPvPSoundKey AntiAirTurret { get; } = new PvPSoundKey(PvPSoundType.Completed, "anti-air");
            public static IPvPSoundKey AntiShipTurret { get; } = new PvPSoundKey(PvPSoundType.Completed, "anti-surface");
            public static IPvPSoundKey Artillery { get; } = new PvPSoundKey(PvPSoundType.Completed, "artillery");
            public static IPvPSoundKey Booster { get; } = new PvPSoundKey(PvPSoundType.Completed, "booster");
            public static IPvPSoundKey DroneStation { get; } = new PvPSoundKey(PvPSoundType.Completed, "builders");
            public static IPvPSoundKey ControlTower { get; } = new PvPSoundKey(PvPSoundType.Completed, "control-tower");
            public static IPvPSoundKey Mortar { get; } = new PvPSoundKey(PvPSoundType.Completed, "mortar");
            public static IPvPSoundKey NavalFactory { get; } = new PvPSoundKey(PvPSoundType.Completed, "naval-factory");
            public static IPvPSoundKey Railgun { get; } = new PvPSoundKey(PvPSoundType.Completed, "railgun");
            public static IPvPSoundKey RocketLauncher { get; } = new PvPSoundKey(PvPSoundType.Completed, "rocket-launcher");
            public static IPvPSoundKey SamSite { get; } = new PvPSoundKey(PvPSoundType.Completed, "sam-site");
            public static IPvPSoundKey Shields { get; } = new PvPSoundKey(PvPSoundType.Completed, "shields");
            public static IPvPSoundKey StealthGenerator { get; } = new PvPSoundKey(PvPSoundType.Completed, "stealth-field");
            public static IPvPSoundKey TeslaCoil { get; } = new PvPSoundKey(PvPSoundType.Completed, "tesla-coil");

            // Units
            public static IPvPSoundKey AircraftReady { get; } = new PvPSoundKey(PvPSoundType.Completed, "AircraftReadyWhoosh");
            public static IPvPSoundKey ShipReady { get; } = new PvPSoundKey(PvPSoundType.Completed, "ShipReadyWhoosh");

            // Other
            public static IPvPSoundKey SpySatellite { get; } = new PvPSoundKey(PvPSoundType.Completed, "satellite");
            public static IPvPSoundKey Ultra { get; } = new PvPSoundKey(PvPSoundType.Completed, "ultra");
        }

        public static class PvPEvents
        {
            // Cruiser
            public static IPvPSoundKey CruiserUnderAttack { get; } = new PvPSoundKey(PvPSoundType.Events, "cruiser-under-attack");
            public static IPvPSoundKey CruiserSignificantlyDamaged { get; } = new PvPSoundKey(PvPSoundType.Events, "cruiser-significantly-damaged");
            public static IPvPSoundKey NoBuildingSlotsLeft { get; } = new PvPSoundKey(PvPSoundType.Events, "no-building-slots-left");

            // Drones
            public static IPvPSoundKey DronesNewDronesReady { get; } = new PvPSoundKey(PvPSoundType.Events, "drones-new-drones-ready");
            public static IPvPSoundKey DronesIdle { get; } = new PvPSoundKey(PvPSoundType.Events, "drones-idle");
            public static IPvPSoundKey DronesNotEnoughDronesToBuild { get; } = new PvPSoundKey(PvPSoundType.Events, "drones-not-enough-drones-to-build");
            public static IPvPSoundKey DronesNotEnoughDronesToFocus { get; } = new PvPSoundKey(PvPSoundType.Events, "drones-not-enough-drones-to-focus");
            public static IPvPSoundKey DronesFocusing { get; } = new PvPSoundKey(PvPSoundType.Events, "drones-focusing");
            public static IPvPSoundKey DronesAllFocused { get; } = new PvPSoundKey(PvPSoundType.Events, "drones-all-focused");
            public static IPvPSoundKey DronesDispersing { get; } = new PvPSoundKey(PvPSoundType.Events, "drones-dispersing");

            // Other
            public static IPvPSoundKey EnemyStartedUltra { get; } = new PvPSoundKey(PvPSoundType.Events, "enemy-started-ultra");
            public static IPvPSoundKey FactoryIncomplete { get; } = new PvPSoundKey(PvPSoundType.Events, "wait-for-factory-to-complete");
            public static IPvPSoundKey PopulationLimitReached { get; } = new PvPSoundKey(PvPSoundType.Events, "population-limit-reached");
            public static IPvPSoundKey TargettingNewTarget { get; } = new PvPSoundKey(PvPSoundType.Events, "targeting-new-target");
            public static IPvPSoundKey TargettingDeselected { get; } = new PvPSoundKey(PvPSoundType.Events, "targeting-untargeted");
        }

        public static class Music
        {
            public static IPvPSoundKey MainTheme { get; } = new PvPSoundKey(PvPSoundType.Music, "main-theme");
            public static IPvPSoundKey Victory { get; } = new PvPSoundKey(PvPSoundType.Music, "victory");
            public static IPvPSoundKey Defeat { get; } = new PvPSoundKey(PvPSoundType.Music, "defeat");
            public static IPvPSoundKey TrashTalk { get; } = new PvPSoundKey(PvPSoundType.Music, "TrashTalk");
            public static IPvPSoundKey Cutscene { get; } = new PvPSoundKey(PvPSoundType.Music, "OrionOmega");
            public static IPvPSoundKey Credits { get; } = new PvPSoundKey(PvPSoundType.Music, "SerialKillerRemaster");

            public static class Background
            {
                public static PvPSoundKeyPair Bobby { get; }
                    = new PvPSoundKeyPair(
                        new PvPSoundKey(PvPSoundType.Music, "bobby-base"),
                        new PvPSoundKey(PvPSoundType.Music, "bobby-danger"));

                public static PvPSoundKeyPair Confusion { get; }
                    = new PvPSoundKeyPair(
                        new PvPSoundKey(PvPSoundType.Music, "confusion-base"),
                        new PvPSoundKey(PvPSoundType.Music, "confusion-danger"));

                public static PvPSoundKeyPair Experimental { get; }
                    = new PvPSoundKeyPair(
                        new PvPSoundKey(PvPSoundType.Music, "experimental-base"),
                        new PvPSoundKey(PvPSoundType.Music, "experimental-danger"));

                public static PvPSoundKeyPair Againagain { get; }
                    = new PvPSoundKeyPair(
                        new PvPSoundKey(PvPSoundType.Music, "againagain-base"),
                        new PvPSoundKey(PvPSoundType.Music, "againagain-danger"));

                public static PvPSoundKeyPair Juggernaut { get; }
                    = new PvPSoundKeyPair(
                        new PvPSoundKey(PvPSoundType.Music, "juggernaut-base"),
                        new PvPSoundKey(PvPSoundType.Music, "juggernaut-danger"));

                public static PvPSoundKeyPair Nothing { get; }
                    = new PvPSoundKeyPair(
                        new PvPSoundKey(PvPSoundType.Music, "nothing-base"),
                        new PvPSoundKey(PvPSoundType.Music, "nothing-danger"));

                public static PvPSoundKeyPair Sleeper { get; }
                    = new PvPSoundKeyPair(
                        new PvPSoundKey(PvPSoundType.Music, "sleeper-base"),
                        new PvPSoundKey(PvPSoundType.Music, "sleeper-danger"));

                public static PvPSoundKeyPair Boss { get; }
                = new PvPSoundKeyPair(
                    new PvPSoundKey(PvPSoundType.Music, "juggernaut-danger"),
                    new PvPSoundKey(PvPSoundType.Music, "juggernaut-danger"));

                public static IList<PvPSoundKeyPair> All = new List<PvPSoundKeyPair>()
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
            public static IPvPSoundKey Click { get; } = new PvPSoundKey(PvPSoundType.UI, "default-click");
            public static IPvPSoundKey ChunkyClick { get; } = new PvPSoundKey(PvPSoundType.UI, "chunky-click");
            public static IPvPSoundKey Delete { get; } = new PvPSoundKey(PvPSoundType.UI, "delete");
            public static IPvPSoundKey ScreenChange { get; } = new PvPSoundKey(PvPSoundType.UI, "screen-change");

            public static class Selected
            {
                public static IPvPSoundKey EnemyCruiser { get; } = new PvPSoundKey(PvPSoundType.UI, "EnemyCruiserSelected");
                public static IPvPSoundKey FriendlyCruiser { get; } = new PvPSoundKey(PvPSoundType.UI, "FriendlyCruiserSelected");
            }
        }

        public static class Shields
        {
            public static IPvPSoundKey FullyCharged { get; } = new PvPSoundKey(PvPSoundType.Shields, "shields-blinking-on");
            public static IPvPSoundKey HitWhileActive { get; } = new PvPSoundKey(PvPSoundType.Shields, "shield-bracing");
            public static IPvPSoundKey FullyDepleted { get; } = new PvPSoundKey(PvPSoundType.Shields, "shields-broken");
        }

        public static class AltDrones
        {
            public static IPvPSoundKey AllDronesFocused { get; } = new PvPSoundKey(PvPSoundType.Events, "AllDronesFocusedAlt");
            public static IPvPSoundKey BuildersReadyAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "BuildersReadyAlt");
            public static IPvPSoundKey BuildingReadyAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "BuildingReadyAlt");
            public static IPvPSoundKey CruiserSignificantlyDamagedAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "CruiserSignificantlyDamagedAlt");
            public static IPvPSoundKey CruiserUnderAttackAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "CruiserUnderAttackAlt");
            public static IPvPSoundKey DispersingAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "DispersingAlt");
            public static IPvPSoundKey DronesIdleAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "DronesIdleAlt");
            public static IPvPSoundKey EnemyBuildingUltraAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "EnemyBuildingUltraAlt");
            public static IPvPSoundKey FocusingAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "FocusingAlt");
            public static IPvPSoundKey InsufficientBuildersAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "InsufficientBuildersAlt");
            public static IPvPSoundKey NowhereToBuildAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "NowhereToBuildAlt");
            public static IPvPSoundKey PopCapReachedAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "PopCapReachedAlt");
            public static IPvPSoundKey TargetingAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "TargetingAlt");
            public static IPvPSoundKey UntargetAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "UntargetAlt");
            public static IPvPSoundKey WaitForFactoryToCompleteAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "WaitForFactoryToCompleteAlt");
            public static IPvPSoundKey UltraReadyAlt { get; } = new PvPSoundKey(PvPSoundType.Events, "UltraReadyAlt");
        }
    }
}
