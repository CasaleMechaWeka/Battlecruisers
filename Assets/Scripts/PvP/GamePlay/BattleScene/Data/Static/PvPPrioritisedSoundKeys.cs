using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static
{
    public static class PvPPrioritisedSoundKeys
    {
        public static class PvPCompleted
        {
            public static class PvPBuildings
            {
                public static PrioritisedSoundKey AirFactory = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AirFactory, SoundPriority.VeryLow);
                public static PrioritisedSoundKey AntiAirTurret = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AntiAirTurret, SoundPriority.VeryLow);
                public static PrioritisedSoundKey AntiShipTurret = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AntiShipTurret, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Artillery = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Artillery, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Booster = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Booster, SoundPriority.VeryLow);
                public static PrioritisedSoundKey DroneStation = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.DroneStation, SoundPriority.VeryLow);
                public static PrioritisedSoundKey ControlTower = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.ControlTower, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Mortar = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Mortar, SoundPriority.VeryLow);
                public static PrioritisedSoundKey NavalFactory = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.NavalFactory, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Railgun = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Railgun, SoundPriority.VeryLow);
                public static PrioritisedSoundKey RocketLauncher = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.RocketLauncher, SoundPriority.VeryLow);
                public static PrioritisedSoundKey SamSite = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.SamSite, SoundPriority.VeryLow);
                public static PrioritisedSoundKey SpySatellite = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.SpySatellite, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Shields = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Shields, SoundPriority.VeryLow);
                public static PrioritisedSoundKey StealthGenerator = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.StealthGenerator, SoundPriority.VeryLow);
                public static PrioritisedSoundKey TeslaCoil = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.TeslaCoil, SoundPriority.VeryLow);
            }

            public static PrioritisedSoundKey Ultra = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Ultra, SoundPriority.VeryHigh);
        }

        public static class PvPEvents
        {
            public static class PvPCruiser
            {
                public static PrioritisedSoundKey UnderAttack = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserUnderAttack, SoundPriority.Normal);
                public static PrioritisedSoundKey SignificantlyDamaged = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserSignificantlyDamaged, SoundPriority.High);
                public static PrioritisedSoundKey NoBuildingSlotsLeft = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.NoBuildingSlotsLeft, SoundPriority.VeryHigh);
            }

            public static class PvPDrones
            {
                public static PrioritisedSoundKey NewDronesReady = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNewDronesReady, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Idle = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesIdle, SoundPriority.Normal);
                public static PrioritisedSoundKey NotEnoughDronesToBuild = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToBuild, SoundPriority.VeryHigh);
                public static PrioritisedSoundKey NotEnoughDronesToFocus = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToFocus, SoundPriority.High);
                public static PrioritisedSoundKey Focusing = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesFocusing, SoundPriority.High);
                public static PrioritisedSoundKey AllFocused = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesAllFocused, SoundPriority.High);
                public static PrioritisedSoundKey Dispersing = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesDispersing, SoundPriority.High);
            }

            public static class PvPTargetting
            {
                public static PrioritisedSoundKey NewTarget = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.TargettingNewTarget, SoundPriority.Normal);
                public static PrioritisedSoundKey TargetCleared = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.TargettingDeselected, SoundPriority.Normal);
            }

            public static PrioritisedSoundKey EnemyStartedUltra = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.EnemyStartedUltra, SoundPriority.VeryHigh);
            public static PrioritisedSoundKey IncompleteFactory = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.FactoryIncomplete, SoundPriority.VeryHigh);
            public static PrioritisedSoundKey PopulationLimitReached = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.PopulationLimitReached, SoundPriority.VeryHigh);
        }

        public static void SetSoundKeys(bool usingDroneWhistles)
        {
            if (usingDroneWhistles)
            {
                PvPCompleted.PvPBuildings.AirFactory = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiAirTurret = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiShipTurret = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Artillery = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Booster = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.ControlTower = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.DroneStation = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Mortar = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.NavalFactory = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Railgun = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.RocketLauncher = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SamSite = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Shields = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SpySatellite = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.StealthGenerator = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.TeslaCoil = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, SoundPriority.VeryLow);

                PvPCompleted.Ultra = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.UltraReadyAlt, SoundPriority.VeryHigh);//missing file


                PvPEvents.PvPCruiser.UnderAttack = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.CruiserUnderAttackAlt, SoundPriority.Normal);

                PvPEvents.PvPCruiser.SignificantlyDamaged = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.CruiserSignificantlyDamagedAlt, SoundPriority.High);

                PvPEvents.PvPCruiser.NoBuildingSlotsLeft = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.NowhereToBuildAlt, SoundPriority.VeryHigh);


                PvPEvents.PvPDrones.AllFocused = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.AllDronesFocused, SoundPriority.High);

                PvPEvents.PvPDrones.Dispersing = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.DispersingAlt, SoundPriority.High);

                PvPEvents.PvPDrones.Focusing = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.FocusingAlt, SoundPriority.High);

                PvPEvents.PvPDrones.Idle = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.DronesIdleAlt, SoundPriority.Normal);

                PvPEvents.PvPDrones.NewDronesReady = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildersReadyAlt, SoundPriority.VeryLow);

                PvPEvents.PvPDrones.NotEnoughDronesToBuild = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.InsufficientBuildersAlt, SoundPriority.VeryHigh);

                PvPEvents.PvPDrones.NotEnoughDronesToFocus = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.InsufficientBuildersAlt, SoundPriority.High);


                PvPEvents.PvPTargetting.NewTarget = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.TargetingAlt, SoundPriority.Normal);

                PvPEvents.PvPTargetting.TargetCleared = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.UntargetAlt, SoundPriority.Normal);


                PvPEvents.EnemyStartedUltra = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.EnemyBuildingUltraAlt, SoundPriority.VeryHigh);

                PvPEvents.IncompleteFactory = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.WaitForFactoryToCompleteAlt, SoundPriority.VeryHigh);

                PvPEvents.PopulationLimitReached = new PrioritisedSoundKey(PvPSoundKeys.AltDrones.PopCapReachedAlt, SoundPriority.VeryHigh);
            }
            else
            {
                PvPCompleted.PvPBuildings.AirFactory = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AircraftReady, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiAirTurret = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AntiAirTurret, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiShipTurret = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AntiShipTurret, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Artillery = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Artillery, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Booster = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Booster, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.ControlTower = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.ControlTower, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.DroneStation = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.DroneStation, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Mortar = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Mortar, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.NavalFactory = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.NavalFactory, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Railgun = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Railgun, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.RocketLauncher = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.RocketLauncher, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SamSite = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.SamSite, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Shields = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Shields, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SpySatellite = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.SpySatellite, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.StealthGenerator = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.StealthGenerator, SoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.TeslaCoil = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.TeslaCoil, SoundPriority.VeryLow);

                PvPCompleted.Ultra = new PrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Ultra, SoundPriority.VeryHigh);


                PvPEvents.PvPCruiser.UnderAttack = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserUnderAttack, SoundPriority.Normal);

                PvPEvents.PvPCruiser.SignificantlyDamaged = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserSignificantlyDamaged, SoundPriority.High);

                PvPEvents.PvPCruiser.NoBuildingSlotsLeft = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.NoBuildingSlotsLeft, SoundPriority.VeryHigh);


                PvPEvents.PvPDrones.AllFocused = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesAllFocused, SoundPriority.High);

                PvPEvents.PvPDrones.Dispersing = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesDispersing, SoundPriority.High);

                PvPEvents.PvPDrones.Focusing = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesFocusing, SoundPriority.High);

                PvPEvents.PvPDrones.Idle = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesIdle, SoundPriority.Normal);

                PvPEvents.PvPDrones.NewDronesReady = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNewDronesReady, SoundPriority.VeryLow);

                PvPEvents.PvPDrones.NotEnoughDronesToBuild = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToBuild, SoundPriority.VeryHigh);

                PvPEvents.PvPDrones.NotEnoughDronesToFocus = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToFocus, SoundPriority.High);


                PvPEvents.PvPTargetting.NewTarget = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.TargettingNewTarget, SoundPriority.Normal);

                PvPEvents.PvPTargetting.TargetCleared = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.TargettingDeselected, SoundPriority.Normal);


                PvPEvents.EnemyStartedUltra = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.EnemyStartedUltra, SoundPriority.VeryHigh);

                PvPEvents.IncompleteFactory = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.FactoryIncomplete, SoundPriority.VeryHigh);

                PvPEvents.PopulationLimitReached = new PrioritisedSoundKey(PvPSoundKeys.PvPEvents.PopulationLimitReached, SoundPriority.VeryHigh);
            }
        }
    }
}