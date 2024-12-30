using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static
{
    public static class PvPPrioritisedSoundKeys
    {
        public static class PvPCompleted
        {
            public static class PvPBuildings
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

        public static class PvPEvents
        {
            public static class PvPCruiser
            {
                public static PrioritisedSoundKey UnderAttack = new PrioritisedSoundKey(SoundKeys.Events.CruiserUnderAttack, SoundPriority.Normal);
                public static PrioritisedSoundKey SignificantlyDamaged = new PrioritisedSoundKey(SoundKeys.Events.CruiserSignificantlyDamaged, SoundPriority.High);
                public static PrioritisedSoundKey NoBuildingSlotsLeft = new PrioritisedSoundKey(SoundKeys.Events.NoBuildingSlotsLeft, SoundPriority.VeryHigh);
            }

            public static class PvPDrones
            {
                public static PrioritisedSoundKey NewDronesReady = new PrioritisedSoundKey(SoundKeys.Events.DronesNewDronesReady, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Idle = new PrioritisedSoundKey(SoundKeys.Events.DronesIdle, SoundPriority.Normal);
                public static PrioritisedSoundKey NotEnoughDronesToBuild = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToBuild, SoundPriority.VeryHigh);
                public static PrioritisedSoundKey NotEnoughDronesToFocus = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToFocus, SoundPriority.High);
                public static PrioritisedSoundKey Focusing = new PrioritisedSoundKey(SoundKeys.Events.DronesFocusing, SoundPriority.High);
                public static PrioritisedSoundKey AllFocused = new PrioritisedSoundKey(SoundKeys.Events.DronesAllFocused, SoundPriority.High);
                public static PrioritisedSoundKey Dispersing = new PrioritisedSoundKey(SoundKeys.Events.DronesDispersing, SoundPriority.High);
            }

            public static class PvPTargetting
            {
                public static PrioritisedSoundKey NewTarget = new PrioritisedSoundKey(SoundKeys.Events.TargettingNewTarget, SoundPriority.Normal);
                public static PrioritisedSoundKey TargetCleared = new PrioritisedSoundKey(SoundKeys.Events.TargettingDeselected, SoundPriority.Normal);
            }

            public static PrioritisedSoundKey EnemyStartedUltra = new PrioritisedSoundKey(SoundKeys.Events.EnemyStartedUltra, SoundPriority.VeryHigh);
            public static PrioritisedSoundKey IncompleteFactory = new PrioritisedSoundKey(SoundKeys.Events.FactoryIncomplete, SoundPriority.VeryHigh);
            public static PrioritisedSoundKey PopulationLimitReached = new PrioritisedSoundKey(SoundKeys.Events.PopulationLimitReached, SoundPriority.VeryHigh);
        }

        public static void SetSoundKeys(bool usingDroneWhistles)
        {
            if (usingDroneWhistles)
            {
                PvPCompleted.PvPBuildings.AirFactory = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiAirTurret = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiShipTurret = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Artillery = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Booster = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.ControlTower = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.DroneStation = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Mortar = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.NavalFactory = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Railgun = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.RocketLauncher = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SamSite = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Shields = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SpySatellite = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.StealthGenerator = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.TeslaCoil = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);

                PvPCompleted.Ultra = new PrioritisedSoundKey(SoundKeys.AltDrones.UltraReadyAlt, SoundPriority.VeryHigh);//missing file


                PvPEvents.PvPCruiser.UnderAttack = new PrioritisedSoundKey(SoundKeys.AltDrones.CruiserUnderAttackAlt, SoundPriority.Normal);

                PvPEvents.PvPCruiser.SignificantlyDamaged = new PrioritisedSoundKey(SoundKeys.AltDrones.CruiserSignificantlyDamagedAlt, SoundPriority.High);

                PvPEvents.PvPCruiser.NoBuildingSlotsLeft = new PrioritisedSoundKey(SoundKeys.AltDrones.NowhereToBuildAlt, SoundPriority.VeryHigh);


                PvPEvents.PvPDrones.AllFocused = new PrioritisedSoundKey(SoundKeys.AltDrones.AllDronesFocused, SoundPriority.High);

                PvPEvents.PvPDrones.Dispersing = new PrioritisedSoundKey(SoundKeys.AltDrones.DispersingAlt, SoundPriority.High);

                PvPEvents.PvPDrones.Focusing = new PrioritisedSoundKey(SoundKeys.AltDrones.FocusingAlt, SoundPriority.High);

                PvPEvents.PvPDrones.Idle = new PrioritisedSoundKey(SoundKeys.AltDrones.DronesIdleAlt, SoundPriority.Normal);

                PvPEvents.PvPDrones.NewDronesReady = new PrioritisedSoundKey(SoundKeys.AltDrones.BuildersReadyAlt, SoundPriority.VeryLow);

                PvPEvents.PvPDrones.NotEnoughDronesToBuild = new PrioritisedSoundKey(SoundKeys.AltDrones.InsufficientBuildersAlt, SoundPriority.VeryHigh);

                PvPEvents.PvPDrones.NotEnoughDronesToFocus = new PrioritisedSoundKey(SoundKeys.AltDrones.InsufficientBuildersAlt, SoundPriority.High);


                PvPEvents.PvPTargetting.NewTarget = new PrioritisedSoundKey(SoundKeys.AltDrones.TargetingAlt, SoundPriority.Normal);

                PvPEvents.PvPTargetting.TargetCleared = new PrioritisedSoundKey(SoundKeys.AltDrones.UntargetAlt, SoundPriority.Normal);


                PvPEvents.EnemyStartedUltra = new PrioritisedSoundKey(SoundKeys.AltDrones.EnemyBuildingUltraAlt, SoundPriority.VeryHigh);

                PvPEvents.IncompleteFactory = new PrioritisedSoundKey(SoundKeys.AltDrones.WaitForFactoryToCompleteAlt, SoundPriority.VeryHigh);

                PvPEvents.PopulationLimitReached = new PrioritisedSoundKey(SoundKeys.AltDrones.PopCapReachedAlt, SoundPriority.VeryHigh);
            }
            else
            {
                PvPCompleted.PvPBuildings.AirFactory = new PrioritisedSoundKey(SoundKeys.Completed.AircraftReady, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiAirTurret = new PrioritisedSoundKey(SoundKeys.Completed.AntiAirTurret, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiShipTurret = new PrioritisedSoundKey(SoundKeys.Completed.AntiShipTurret, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Artillery = new PrioritisedSoundKey(SoundKeys.Completed.Artillery, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Booster = new PrioritisedSoundKey(SoundKeys.Completed.Booster, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.ControlTower = new PrioritisedSoundKey(SoundKeys.Completed.ControlTower, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.DroneStation = new PrioritisedSoundKey(SoundKeys.Completed.DroneStation, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Mortar = new PrioritisedSoundKey(SoundKeys.Completed.Mortar, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.NavalFactory = new PrioritisedSoundKey(SoundKeys.Completed.NavalFactory, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Railgun = new PrioritisedSoundKey(SoundKeys.Completed.Railgun, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.RocketLauncher = new PrioritisedSoundKey(SoundKeys.Completed.RocketLauncher, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SamSite = new PrioritisedSoundKey(SoundKeys.Completed.SamSite, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Shields = new PrioritisedSoundKey(SoundKeys.Completed.Shields, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SpySatellite = new PrioritisedSoundKey(SoundKeys.Completed.SpySatellite, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.StealthGenerator = new PrioritisedSoundKey(SoundKeys.Completed.StealthGenerator, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.TeslaCoil = new PrioritisedSoundKey(SoundKeys.Completed.TeslaCoil, SoundPriority.VeryLow);

                PvPCompleted.Ultra = new PrioritisedSoundKey(SoundKeys.Completed.Ultra, SoundPriority.VeryHigh);


                PvPEvents.PvPCruiser.UnderAttack = new PrioritisedSoundKey(SoundKeys.Events.CruiserUnderAttack, SoundPriority.Normal);

                PvPEvents.PvPCruiser.SignificantlyDamaged = new PrioritisedSoundKey(SoundKeys.Events.CruiserSignificantlyDamaged, SoundPriority.High);

                PvPEvents.PvPCruiser.NoBuildingSlotsLeft = new PrioritisedSoundKey(SoundKeys.Events.NoBuildingSlotsLeft, SoundPriority.VeryHigh);


                PvPEvents.PvPDrones.AllFocused = new PrioritisedSoundKey(SoundKeys.Events.DronesAllFocused, SoundPriority.High);

                PvPEvents.PvPDrones.Dispersing = new PrioritisedSoundKey(SoundKeys.Events.DronesDispersing, SoundPriority.High);

                PvPEvents.PvPDrones.Focusing = new PrioritisedSoundKey(SoundKeys.Events.DronesFocusing, SoundPriority.High);

                PvPEvents.PvPDrones.Idle = new PrioritisedSoundKey(SoundKeys.Events.DronesIdle, SoundPriority.Normal);

                PvPEvents.PvPDrones.NewDronesReady = new PrioritisedSoundKey(SoundKeys.Events.DronesNewDronesReady, SoundPriority.VeryLow);

                PvPEvents.PvPDrones.NotEnoughDronesToBuild = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToBuild, SoundPriority.VeryHigh);

                PvPEvents.PvPDrones.NotEnoughDronesToFocus = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToFocus, SoundPriority.High);


                PvPEvents.PvPTargetting.NewTarget = new PrioritisedSoundKey(SoundKeys.Events.TargettingNewTarget, SoundPriority.Normal);

                PvPEvents.PvPTargetting.TargetCleared = new PrioritisedSoundKey(SoundKeys.Events.TargettingDeselected, SoundPriority.Normal);


                PvPEvents.EnemyStartedUltra = new PrioritisedSoundKey(SoundKeys.Events.EnemyStartedUltra, SoundPriority.VeryHigh);

                PvPEvents.IncompleteFactory = new PrioritisedSoundKey(SoundKeys.Events.FactoryIncomplete, SoundPriority.VeryHigh);

                PvPEvents.PopulationLimitReached = new PrioritisedSoundKey(SoundKeys.Events.PopulationLimitReached, SoundPriority.VeryHigh);
            }
        }
    }
}