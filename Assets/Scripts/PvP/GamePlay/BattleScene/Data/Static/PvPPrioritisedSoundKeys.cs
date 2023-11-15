using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static
{
    public static class PvPPrioritisedSoundKeys
    {
        public static class PvPCompleted
        {
            public static class PvPBuildings
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

        public static class PvPEvents
        {
            public static class PvPCruiser
            {
                public static PvPPrioritisedSoundKey UnderAttack = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserUnderAttack, PvPSoundPriority.Normal);
                public static PvPPrioritisedSoundKey SignificantlyDamaged = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserSignificantlyDamaged, PvPSoundPriority.High);
                public static PvPPrioritisedSoundKey NoBuildingSlotsLeft = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.NoBuildingSlotsLeft, PvPSoundPriority.VeryHigh);
            }

            public static class PvPDrones
            {
                public static PvPPrioritisedSoundKey NewDronesReady = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNewDronesReady, PvPSoundPriority.VeryLow);
                public static PvPPrioritisedSoundKey Idle = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesIdle, PvPSoundPriority.Normal);
                public static PvPPrioritisedSoundKey NotEnoughDronesToBuild = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToBuild, PvPSoundPriority.VeryHigh);
                public static PvPPrioritisedSoundKey NotEnoughDronesToFocus = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToFocus, PvPSoundPriority.High);
                public static PvPPrioritisedSoundKey Focusing = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesFocusing, PvPSoundPriority.High);
                public static PvPPrioritisedSoundKey AllFocused = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesAllFocused, PvPSoundPriority.High);
                public static PvPPrioritisedSoundKey Dispersing = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesDispersing, PvPSoundPriority.High);
            }

            public static class PvPTargetting
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
                PvPCompleted.PvPBuildings.AirFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiAirTurret = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiShipTurret = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Artillery = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Booster = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.ControlTower = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.DroneStation = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Mortar = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.NavalFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Railgun = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.RocketLauncher = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SamSite = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Shields = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SpySatellite = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.StealthGenerator = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.TeslaCoil = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildingReadyAlt, PvPSoundPriority.VeryLow);

                PvPCompleted.Ultra = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.UltraReadyAlt, PvPSoundPriority.VeryHigh);//missing file


                PvPEvents.PvPCruiser.UnderAttack = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.CruiserUnderAttackAlt, PvPSoundPriority.Normal);

                PvPEvents.PvPCruiser.SignificantlyDamaged = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.CruiserSignificantlyDamagedAlt, PvPSoundPriority.High);

                PvPEvents.PvPCruiser.NoBuildingSlotsLeft = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.NowhereToBuildAlt, PvPSoundPriority.VeryHigh);


                PvPEvents.PvPDrones.AllFocused = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.AllDronesFocused, PvPSoundPriority.High);

                PvPEvents.PvPDrones.Dispersing = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.DispersingAlt, PvPSoundPriority.High);

                PvPEvents.PvPDrones.Focusing = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.FocusingAlt, PvPSoundPriority.High);

                PvPEvents.PvPDrones.Idle = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.DronesIdleAlt, PvPSoundPriority.Normal);

                PvPEvents.PvPDrones.NewDronesReady = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.BuildersReadyAlt, PvPSoundPriority.VeryLow);

                PvPEvents.PvPDrones.NotEnoughDronesToBuild = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.InsufficientBuildersAlt, PvPSoundPriority.VeryHigh);

                PvPEvents.PvPDrones.NotEnoughDronesToFocus = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.InsufficientBuildersAlt, PvPSoundPriority.High);


                PvPEvents.PvPTargetting.NewTarget = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.TargetingAlt, PvPSoundPriority.Normal);

                PvPEvents.PvPTargetting.TargetCleared = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.UntargetAlt, PvPSoundPriority.Normal);


                PvPEvents.EnemyStartedUltra = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.EnemyBuildingUltraAlt, PvPSoundPriority.VeryHigh);

                PvPEvents.IncompleteFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.WaitForFactoryToCompleteAlt, PvPSoundPriority.VeryHigh);

                PvPEvents.PopulationLimitReached = new PvPPrioritisedSoundKey(PvPSoundKeys.AltDrones.PopCapReachedAlt, PvPSoundPriority.VeryHigh);
            }
            else
            {
                PvPCompleted.PvPBuildings.AirFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AircraftReady, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiAirTurret = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AntiAirTurret, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.AntiShipTurret = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.AntiShipTurret, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Artillery = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Artillery, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Booster = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Booster, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.ControlTower = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.ControlTower, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.DroneStation = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.DroneStation, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Mortar = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Mortar, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.NavalFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.NavalFactory, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Railgun = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Railgun, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.RocketLauncher = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.RocketLauncher, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SamSite = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.SamSite, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.Shields = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Shields, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.SpySatellite = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.SpySatellite, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.StealthGenerator = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.StealthGenerator, PvPSoundPriority.VeryLow);
                PvPCompleted.PvPBuildings.TeslaCoil = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.TeslaCoil, PvPSoundPriority.VeryLow);

                PvPCompleted.Ultra = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPCompleted.Ultra, PvPSoundPriority.VeryHigh);


                PvPEvents.PvPCruiser.UnderAttack = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserUnderAttack, PvPSoundPriority.Normal);

                PvPEvents.PvPCruiser.SignificantlyDamaged = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.CruiserSignificantlyDamaged, PvPSoundPriority.High);

                PvPEvents.PvPCruiser.NoBuildingSlotsLeft = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.NoBuildingSlotsLeft, PvPSoundPriority.VeryHigh);


                PvPEvents.PvPDrones.AllFocused = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesAllFocused, PvPSoundPriority.High);

                PvPEvents.PvPDrones.Dispersing = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesDispersing, PvPSoundPriority.High);

                PvPEvents.PvPDrones.Focusing = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesFocusing, PvPSoundPriority.High);

                PvPEvents.PvPDrones.Idle = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesIdle, PvPSoundPriority.Normal);

                PvPEvents.PvPDrones.NewDronesReady = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNewDronesReady, PvPSoundPriority.VeryLow);

                PvPEvents.PvPDrones.NotEnoughDronesToBuild = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToBuild, PvPSoundPriority.VeryHigh);

                PvPEvents.PvPDrones.NotEnoughDronesToFocus = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.DronesNotEnoughDronesToFocus, PvPSoundPriority.High);


                PvPEvents.PvPTargetting.NewTarget = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.TargettingNewTarget, PvPSoundPriority.Normal);

                PvPEvents.PvPTargetting.TargetCleared = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.TargettingDeselected, PvPSoundPriority.Normal);


                PvPEvents.EnemyStartedUltra = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.EnemyStartedUltra, PvPSoundPriority.VeryHigh);

                PvPEvents.IncompleteFactory = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.FactoryIncomplete, PvPSoundPriority.VeryHigh);

                PvPEvents.PopulationLimitReached = new PvPPrioritisedSoundKey(PvPSoundKeys.PvPEvents.PopulationLimitReached, PvPSoundPriority.VeryHigh);
            }
        }
    }
}