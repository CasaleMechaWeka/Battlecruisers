using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Turrets.AntiAir
{
    public class AntiAirTurretIgnoresSatellitesTestGod : TestGodBase
    {
        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            // Aircraft
            AircraftController[] aircraftList = FindObjectsOfType<AircraftController>();
            foreach (AircraftController aircraft in aircraftList)
            {
                helper.InitialiseUnit(aircraft, Faction.Blues);
                aircraft.StartConstruction();
            }

            // Turret
            TurretController turret = FindObjectOfType<TurretController>();
            helper.InitialiseBuilding(turret, Faction.Reds);
            turret.StartConstruction();
        }
    }
}