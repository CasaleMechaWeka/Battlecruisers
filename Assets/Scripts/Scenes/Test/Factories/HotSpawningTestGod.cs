using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class HotSpawningTestGod : TestGodBase
    {
        public UnitWrapper unitPrefab;

        protected override void Start()
        {
            base.Start();

            unitPrefab.Initialise();

            Helper helper = new Helper(buildSpeedMultiplier: 5, updaterProvider: _updaterProvider);

            // Factory
            Factory factory = FindObjectOfType<Factory>();
            helper.InitialiseBuilding(factory, Faction.Blues, parentCruiserDirection: Direction.Right);
            factory.CompletedBuildable += (sender, e) => factory.StartBuildingUnit(unitPrefab);
            factory.StartConstruction();

            // Turret
            TurretController turret = FindObjectOfType<TurretController>();
            helper.InitialiseBuilding(turret, Faction.Reds);
            turret.StartConstruction();
        }
    }
}
