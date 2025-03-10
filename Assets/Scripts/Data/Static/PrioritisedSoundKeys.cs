using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data.Static
{
    public static class PrioritisedSoundKeys
    {
        public static class Completed
        {
            public static class Buildings
            {
                public static PrioritisedSoundKey AirFactory = new PrioritisedSoundKey(SoundKeys.Completed.AirFactory, SoundPriority.VeryLow);
                public static PrioritisedSoundKey AntiAirTurret = new PrioritisedSoundKey(SoundKeys.Completed.AntiAirTurret, SoundPriority.VeryLow);
                public static PrioritisedSoundKey AntiShipTurret = new PrioritisedSoundKey(SoundKeys.Completed.AntiShipTurret, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Artillery = new PrioritisedSoundKey(SoundKeys.Completed.Artillery, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Booster = new PrioritisedSoundKey(SoundKeys.Completed.Booster, SoundPriority.VeryLow);
                public static PrioritisedSoundKey DroneStation = new PrioritisedSoundKey(SoundKeys.Completed.DroneStation, SoundPriority.VeryLow);
                public static PrioritisedSoundKey ControlTower = new PrioritisedSoundKey(SoundKeys.Completed.ControlTower, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Mortar = new PrioritisedSoundKey(SoundKeys.Completed.Mortar, SoundPriority.VeryLow);
                public static PrioritisedSoundKey NavalFactory = new PrioritisedSoundKey(SoundKeys.Completed.NavalFactory, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Railgun = new PrioritisedSoundKey(SoundKeys.Completed.Railgun, SoundPriority.VeryLow);
                public static PrioritisedSoundKey RocketLauncher = new PrioritisedSoundKey(SoundKeys.Completed.RocketLauncher, SoundPriority.VeryLow);
                public static PrioritisedSoundKey SamSite = new PrioritisedSoundKey(SoundKeys.Completed.SamSite, SoundPriority.VeryLow);
                public static PrioritisedSoundKey SpySatellite = new PrioritisedSoundKey(SoundKeys.Completed.SpySatellite, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Shields = new PrioritisedSoundKey(SoundKeys.Completed.Shields, SoundPriority.VeryLow);
                public static PrioritisedSoundKey StealthGenerator = new PrioritisedSoundKey(SoundKeys.Completed.StealthGenerator, SoundPriority.VeryLow);
                public static PrioritisedSoundKey TeslaCoil = new PrioritisedSoundKey(SoundKeys.Completed.TeslaCoil, SoundPriority.VeryLow);
            }

            public static PrioritisedSoundKey Ultra = new PrioritisedSoundKey(SoundKeys.Completed.Ultra, SoundPriority.VeryHigh);
        }

        public static class Events
        {
            public static class Cruiser
            {
                public static PrioritisedSoundKey UnderAttack = new PrioritisedSoundKey(SoundKeys.Events.CruiserUnderAttack, SoundPriority.Normal);
                public static PrioritisedSoundKey SignificantlyDamaged = new PrioritisedSoundKey(SoundKeys.Events.CruiserSignificantlyDamaged, SoundPriority.High);
                public static PrioritisedSoundKey NoBuildingSlotsLeft = new PrioritisedSoundKey(SoundKeys.Events.NoBuildingSlotsLeft , SoundPriority.VeryHigh);
            }

            public static class Drones
            {
                public static PrioritisedSoundKey NewDronesReady = new PrioritisedSoundKey(SoundKeys.Events.DronesNewDronesReady, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Idle = new PrioritisedSoundKey(SoundKeys.Events.DronesIdle, SoundPriority.Normal);
                public static PrioritisedSoundKey NotEnoughDronesToBuild = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToBuild, SoundPriority.VeryHigh);
                public static PrioritisedSoundKey NotEnoughDronesToFocus = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToFocus, SoundPriority.High);
                public static PrioritisedSoundKey Focusing = new PrioritisedSoundKey(SoundKeys.Events.DronesFocusing, SoundPriority.High);
                public static PrioritisedSoundKey AllFocused = new PrioritisedSoundKey(SoundKeys.Events.DronesAllFocused, SoundPriority.High);
                public static PrioritisedSoundKey Dispersing = new PrioritisedSoundKey(SoundKeys.Events.DronesDispersing, SoundPriority.High);
            }

            public static class Targetting
            {
                public static PrioritisedSoundKey NewTarget  = new PrioritisedSoundKey(SoundKeys.Events.TargettingNewTarget, SoundPriority.Normal);
                public static PrioritisedSoundKey TargetCleared = new PrioritisedSoundKey(SoundKeys.Events.TargettingDeselected, SoundPriority.Normal);
            }

            public static PrioritisedSoundKey EnemyStartedUltra = new PrioritisedSoundKey(SoundKeys.Events.EnemyStartedUltra, SoundPriority.VeryHigh);
            public static PrioritisedSoundKey IncompleteFactory = new PrioritisedSoundKey(SoundKeys.Events.FactoryIncomplete, SoundPriority.VeryHigh);
            public static PrioritisedSoundKey PopulationLimitReached  = new PrioritisedSoundKey(SoundKeys.Events.PopulationLimitReached, SoundPriority.VeryHigh);
        }

        public static void SetSoundKeys(bool usingDroneWhistles)
        {
            if (usingDroneWhistles)
            {
                Completed.Buildings.AirFactory = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.AntiAirTurret = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.AntiShipTurret = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.Artillery = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.Booster = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.ControlTower = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.DroneStation = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.Mortar = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.NavalFactory = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.Railgun = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.RocketLauncher = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.SamSite = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.Shields = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.SpySatellite = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.StealthGenerator = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                Completed.Buildings.TeslaCoil = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);

                Completed.Ultra = new PrioritisedSoundKey(SoundKeys.AltDrones.UltraReadyAlt, SoundPriority.VeryHigh);//missing file


                Events.Cruiser.UnderAttack = new PrioritisedSoundKey(SoundKeys.AltDrones.CruiserUnderAttackAlt, SoundPriority.Normal);

                Events.Cruiser.SignificantlyDamaged = new PrioritisedSoundKey(SoundKeys.AltDrones.CruiserSignificantlyDamagedAlt, SoundPriority.High);

                Events.Cruiser.NoBuildingSlotsLeft = new PrioritisedSoundKey(SoundKeys.AltDrones.NowhereToBuildAlt, SoundPriority.VeryHigh);


                Events.Drones.AllFocused = new PrioritisedSoundKey(SoundKeys.AltDrones.AllDronesFocused, SoundPriority.High);

                Events.Drones.Dispersing = new PrioritisedSoundKey(SoundKeys.AltDrones.DispersingAlt, SoundPriority.High);

                Events.Drones.Focusing = new PrioritisedSoundKey(SoundKeys.AltDrones.FocusingAlt, SoundPriority.High);

                Events.Drones.Idle = new PrioritisedSoundKey(SoundKeys.AltDrones.DronesIdleAlt, SoundPriority.Normal);

                Events.Drones.NewDronesReady = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildersReadyAlt, SoundPriority.VeryLow);

                Events.Drones.NotEnoughDronesToBuild = new PrioritisedSoundKey(SoundKeys.AltDrones.InsufficientBuildersAlt, SoundPriority.VeryHigh);

                Events.Drones.NotEnoughDronesToFocus = new PrioritisedSoundKey(SoundKeys.AltDrones.InsufficientBuildersAlt, SoundPriority.High);


                Events.Targetting.NewTarget = new PrioritisedSoundKey(SoundKeys.AltDrones.TargetingAlt, SoundPriority.Normal);

                Events.Targetting.TargetCleared = new PrioritisedSoundKey(SoundKeys.AltDrones.UntargetAlt, SoundPriority.Normal);


                Events.EnemyStartedUltra = new PrioritisedSoundKey(SoundKeys.AltDrones.EnemyBuildingUltraAlt, SoundPriority.VeryHigh);

                Events.IncompleteFactory = new PrioritisedSoundKey(SoundKeys.AltDrones.WaitForFactoryToCompleteAlt, SoundPriority.VeryHigh);

                Events.PopulationLimitReached = new PrioritisedSoundKey(SoundKeys.AltDrones.PopCapReachedAlt, SoundPriority.VeryHigh);
            }
            else
            {
                Completed.Buildings.AirFactory = new PrioritisedSoundKey(SoundKeys.Completed.AircraftReady, SoundPriority.VeryLow);
                Completed.Buildings.AntiAirTurret = new PrioritisedSoundKey(SoundKeys.Completed.AntiAirTurret, SoundPriority.VeryLow);
                Completed.Buildings.AntiShipTurret = new PrioritisedSoundKey(SoundKeys.Completed.AntiShipTurret, SoundPriority.VeryLow);
                Completed.Buildings.Artillery = new PrioritisedSoundKey(SoundKeys.Completed.Artillery, SoundPriority.VeryLow);
                Completed.Buildings.Booster = new PrioritisedSoundKey(SoundKeys.Completed.Booster, SoundPriority.VeryLow);
                Completed.Buildings.ControlTower = new PrioritisedSoundKey(SoundKeys.Completed.ControlTower, SoundPriority.VeryLow);
                Completed.Buildings.DroneStation = new PrioritisedSoundKey(SoundKeys.Completed.DroneStation, SoundPriority.VeryLow);
                Completed.Buildings.Mortar = new PrioritisedSoundKey(SoundKeys.Completed.Mortar, SoundPriority.VeryLow);
                Completed.Buildings.NavalFactory = new PrioritisedSoundKey(SoundKeys.Completed.NavalFactory, SoundPriority.VeryLow);
                Completed.Buildings.Railgun = new PrioritisedSoundKey(SoundKeys.Completed.Railgun, SoundPriority.VeryLow);
                Completed.Buildings.RocketLauncher = new PrioritisedSoundKey(SoundKeys.Completed.RocketLauncher, SoundPriority.VeryLow);
                Completed.Buildings.SamSite = new PrioritisedSoundKey(SoundKeys.Completed.SamSite, SoundPriority.VeryLow);
                Completed.Buildings.Shields = new PrioritisedSoundKey(SoundKeys.Completed.Shields, SoundPriority.VeryLow);
                Completed.Buildings.SpySatellite = new PrioritisedSoundKey(SoundKeys.Completed.SpySatellite, SoundPriority.VeryLow);
                Completed.Buildings.StealthGenerator = new PrioritisedSoundKey(SoundKeys.Completed.StealthGenerator, SoundPriority.VeryLow);
                Completed.Buildings.TeslaCoil = new PrioritisedSoundKey(SoundKeys.Completed.TeslaCoil, SoundPriority.VeryLow);

                Completed.Ultra = new PrioritisedSoundKey(SoundKeys.Completed.Ultra, SoundPriority.VeryHigh);


                Events.Cruiser.UnderAttack = new PrioritisedSoundKey(SoundKeys.Events.CruiserUnderAttack, SoundPriority.Normal);

                Events.Cruiser.SignificantlyDamaged = new PrioritisedSoundKey(SoundKeys.Events.CruiserSignificantlyDamaged, SoundPriority.High);

                Events.Cruiser.NoBuildingSlotsLeft = new PrioritisedSoundKey(SoundKeys.Events.NoBuildingSlotsLeft, SoundPriority.VeryHigh);


                Events.Drones.AllFocused = new PrioritisedSoundKey(SoundKeys.Events.DronesAllFocused, SoundPriority.High);

                Events.Drones.Dispersing = new PrioritisedSoundKey(SoundKeys.Events.DronesDispersing, SoundPriority.High);

                Events.Drones.Focusing = new PrioritisedSoundKey(SoundKeys.Events.DronesFocusing, SoundPriority.High);

                Events.Drones.Idle = new PrioritisedSoundKey(SoundKeys.Events.DronesIdle, SoundPriority.Normal);

                Events.Drones.NewDronesReady = new PrioritisedSoundKey(SoundKeys.Events.DronesNewDronesReady, SoundPriority.VeryLow);

                Events.Drones.NotEnoughDronesToBuild = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToBuild, SoundPriority.VeryHigh);

                Events.Drones.NotEnoughDronesToFocus = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToFocus, SoundPriority.High);


                Events.Targetting.NewTarget = new PrioritisedSoundKey(SoundKeys.Events.TargettingNewTarget, SoundPriority.Normal);

                Events.Targetting.TargetCleared = new PrioritisedSoundKey(SoundKeys.Events.TargettingDeselected, SoundPriority.Normal);


                Events.EnemyStartedUltra = new PrioritisedSoundKey(SoundKeys.Events.EnemyStartedUltra, SoundPriority.VeryHigh);

                Events.IncompleteFactory = new PrioritisedSoundKey(SoundKeys.Events.FactoryIncomplete, SoundPriority.VeryHigh);

                Events.PopulationLimitReached = new PrioritisedSoundKey(SoundKeys.Events.PopulationLimitReached, SoundPriority.VeryHigh);
            }
        }
    }
}