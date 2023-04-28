using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static
{
    public static class PvPPrioritisedSoundKeys
    {
        public static class Completed
        {
            public static class Buildings
            {
                public static PvPPrioritisedSoundKey AirFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AirFactory, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey AntiAirTurret = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AntiAirTurret, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey AntiShipTurret = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AntiShipTurret, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey Artillery = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Artillery, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey Booster = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Booster, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey DroneStation = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.DroneStation, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey ControlTower = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.ControlTower, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey Mortar = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Mortar, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey NavalFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.NavalFactory, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey Railgun = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Railgun, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey RocketLauncher = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.RocketLauncher, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey SamSite = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.SamSite, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey SpySatellite = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.SpySatellite, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey Shields = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Shields, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey StealthGenerator = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.StealthGenerator, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey TeslaCoil = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.TeslaCoil, PvPSoundPriority.VeryLow);
            }

            public static PvPPrioritisedSoundKey Ultra = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Ultra, PvPSoundPriority.VeryHigh);
        }

        public static class Events
        {
            public static class Cruiser
            {
                public static PvPPrioritisedSoundKey UnderAttack = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserUnderAttack, PvPSoundPriority.Normal);
                public static PvPPrioritisedSoundKey SignificantlyDamaged = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserSignificantlyDamaged, PvPSoundPriority.High);
                public static PvPPrioritisedSoundKey NoBuildingSlotsLeft = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.NoBuildingSlotsLeft, PvPSoundPriority.VeryHigh);
            }

            public static class Drones
            {
                public static PvPPrioritisedSoundKey NewDronesReady = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNewDronesReady, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey Idle = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesIdle, PvPSoundPriority.Normal);
                public static PvPPrioritisedSoundKey NotEnoughDronesToBuild = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToBuild, PvPSoundPriority.VeryHigh);
                public static PvPPrioritisedSoundKey NotEnoughDronesToFocus = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToFocus, PvPSoundPriority.High);
                public static PvPPrioritisedSoundKey Focusing = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesFocusing, PvPSoundPriority.High);
                public static PvPPrioritisedSoundKey AllFocused = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesAllFocused, PvPSoundPriority.High);
                public static PvPPrioritisedSoundKey Dispersing = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesDispersing, PvPSoundPriority.High);
            }

            public static class Targetting
            {
                public static PvPPrioritisedSoundKey NewTarget = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.TargettingNewTarget, PvPSoundPriority.Normal);
                public static PvPPrioritisedSoundKey TargetCleared = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.TargettingDeselected, PvPSoundPriority.Normal);
            }

            public static PvPPrioritisedSoundKey EnemyStartedUltra = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.EnemyStartedUltra, PvPSoundPriority.VeryHigh);
            public static PvPPrioritisedSoundKey IncompleteFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.FactoryIncomplete, PvPSoundPriority.VeryHigh);
            public static PvPPrioritisedSoundKey PopulationLimitReached = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.PopulationLimitReached, PvPSoundPriority.VeryHigh);
        }

        public static void SetSoundKeys(bool usingDroneWhistles)
        {
            if (usingDroneWhistles)
            {
                Completed.Buildings.AirFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.AntiAirTurret = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.AntiShipTurret = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.Artillery = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.Booster = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.ControlTower = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.DroneStation = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.Mortar = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.NavalFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.Railgun = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.RocketLauncher = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.SamSite = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.Shields = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.SpySatellite = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.StealthGenerator = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                Completed.Buildings.TeslaCoil = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);

                Completed.Ultra = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.UltraReadyAlt, PvPSoundPriority.VeryHigh);//missing file


                Events.Cruiser.UnderAttack = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.CruiserUnderAttackAlt, PvPSoundPriority.Normal);

                Events.Cruiser.SignificantlyDamaged = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.CruiserSignificantlyDamagedAlt, PvPSoundPriority.High);

                Events.Cruiser.NoBuildingSlotsLeft = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.NowhereToBuildAlt, PvPSoundPriority.VeryHigh);


                Events.Drones.AllFocused = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.AllDronesFocused, PvPSoundPriority.High);

                Events.Drones.Dispersing = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.DispersingAlt, PvPSoundPriority.High);

                Events.Drones.Focusing = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.FocusingAlt, PvPSoundPriority.High);

                Events.Drones.Idle = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.DronesIdleAlt, PvPSoundPriority.Normal);

                Events.Drones.NewDronesReady = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildersReadyAlt, PvPSoundPriority.VeryLow);

                Events.Drones.NotEnoughDronesToBuild = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.InsufficientBuildersAlt, PvPSoundPriority.VeryHigh);

                Events.Drones.NotEnoughDronesToFocus = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.InsufficientBuildersAlt, PvPSoundPriority.High);


                Events.Targetting.NewTarget = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.TargetingAlt, PvPSoundPriority.Normal);

                Events.Targetting.TargetCleared = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.UntargetAlt, PvPSoundPriority.Normal);


                Events.EnemyStartedUltra = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.EnemyBuildingUltraAlt, PvPSoundPriority.VeryHigh);

                Events.IncompleteFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.WaitForFactoryToCompleteAlt, PvPSoundPriority.VeryHigh);

                Events.PopulationLimitReached = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.PopCapReachedAlt, PvPSoundPriority.VeryHigh);
            }
            else
            {
                Completed.Buildings.AirFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AircraftReady, PvPSoundPriority.VeryLow);
                Completed.Buildings.AntiAirTurret = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AntiAirTurret, PvPSoundPriority.VeryLow);
                Completed.Buildings.AntiShipTurret = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AntiShipTurret, PvPSoundPriority.VeryLow);
                Completed.Buildings.Artillery = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Artillery, PvPSoundPriority.VeryLow);
                Completed.Buildings.Booster = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Booster, PvPSoundPriority.VeryLow);
                Completed.Buildings.ControlTower = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.ControlTower, PvPSoundPriority.VeryLow);
                Completed.Buildings.DroneStation = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.DroneStation, PvPSoundPriority.VeryLow);
                Completed.Buildings.Mortar = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Mortar, PvPSoundPriority.VeryLow);
                Completed.Buildings.NavalFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.NavalFactory, PvPSoundPriority.VeryLow);
                Completed.Buildings.Railgun = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Railgun, PvPSoundPriority.VeryLow);
                Completed.Buildings.RocketLauncher = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.RocketLauncher, PvPSoundPriority.VeryLow);
                Completed.Buildings.SamSite = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.SamSite, PvPSoundPriority.VeryLow);
                Completed.Buildings.Shields = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Shields, PvPSoundPriority.VeryLow);
                Completed.Buildings.SpySatellite = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.SpySatellite, PvPSoundPriority.VeryLow);
                Completed.Buildings.StealthGenerator = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.StealthGenerator, PvPSoundPriority.VeryLow);
                Completed.Buildings.TeslaCoil = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.TeslaCoil, PvPSoundPriority.VeryLow);

                Completed.Ultra = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Ultra, PvPSoundPriority.VeryHigh);


                Events.Cruiser.UnderAttack = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserUnderAttack, PvPSoundPriority.Normal);

                Events.Cruiser.SignificantlyDamaged = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserSignificantlyDamaged, PvPSoundPriority.High);

                Events.Cruiser.NoBuildingSlotsLeft = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.NoBuildingSlotsLeft, PvPSoundPriority.VeryHigh);


                Events.Drones.AllFocused = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesAllFocused, PvPSoundPriority.High);

                Events.Drones.Dispersing = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesDispersing, PvPSoundPriority.High);

                Events.Drones.Focusing = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesFocusing, PvPSoundPriority.High);

                Events.Drones.Idle = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesIdle, PvPSoundPriority.Normal);

                Events.Drones.NewDronesReady = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNewDronesReady, PvPSoundPriority.VeryLow);

                Events.Drones.NotEnoughDronesToBuild = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToBuild, PvPSoundPriority.VeryHigh);

                Events.Drones.NotEnoughDronesToFocus = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToFocus, PvPSoundPriority.High);


                Events.Targetting.NewTarget = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.TargettingNewTarget, PvPSoundPriority.Normal);

                Events.Targetting.TargetCleared = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.TargettingDeselected, PvPSoundPriority.Normal);


                Events.EnemyStartedUltra = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.EnemyStartedUltra, PvPSoundPriority.VeryHigh);

                Events.IncompleteFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.FactoryIncomplete, PvPSoundPriority.VeryHigh);

                Events.PopulationLimitReached = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.PopulationLimitReached, PvPSoundPriority.VeryHigh);
            }
        }
    }
}