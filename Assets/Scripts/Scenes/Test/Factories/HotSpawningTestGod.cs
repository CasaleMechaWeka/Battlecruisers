using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class HotSpawningTestGod : MonoBehaviour
    {
        public UnitWrapper unitPrefab;

        void Start()
        {
            unitPrefab.Initialise();
            unitPrefab.Buildable.StaticInitialise();

            Helper helper = new Helper(buildSpeedMultiplier: 5);

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
