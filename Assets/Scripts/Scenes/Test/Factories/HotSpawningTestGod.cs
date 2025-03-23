using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class HotSpawningTestGod : TestGodBase
    {
        private Factory _factory;
        private TurretController _turret;

        public UnitWrapper unitPrefab;

        protected override async Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            return await HelperFactory.CreateHelperAsync(buildSpeedMultiplier: 5, updaterProvider: updaterProvider);
        }

        protected override List<GameObject> GetGameObjects()
        {
            _factory = FindObjectOfType<Factory>();
            _turret = FindObjectOfType<TurretController>();

            return new List<GameObject>()
            {
                _factory.GameObject,
                _turret.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            unitPrefab.StaticInitialise();

            // Factory
            helper.InitialiseBuilding(_factory, Faction.Blues, parentCruiserDirection: Direction.Right);
            _factory.CompletedBuildable += (sender, e) => _factory.StartBuildingUnit(unitPrefab);
            _factory.StartConstruction();

            // Turret
            helper.InitialiseBuilding(_turret, Faction.Reds);
            _turret.StartConstruction();
        }
    }
}
