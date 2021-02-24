using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Performance.ObjectPooling
{
    public class GunshipRecyclingTestGod : TestGodBase
    {
        private Factory _factory;
        private ShipController _target;

        public UnitWrapper unitPrefab;
        public float timeToDieInS = 1;

        protected override List<GameObject> GetGameObjects()
        {
            _factory = FindObjectOfType<Factory>();
            Assert.IsNotNull(_factory);

            _target = FindObjectOfType<ShipController>();
            Assert.IsNotNull(_target);

            return new List<GameObject>()
            {
                _factory.GameObject,
                _target.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            Assert.IsNotNull(unitPrefab);
            unitPrefab.StaticInitialise(helper.CommonStrings);

            TimeScaleDeferrer deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(deferrer);

            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Setup target
            helper.InitialiseUnit(_target, Faction.Reds, parentCruiserDirection: Direction.Left);
            _target.StartConstruction();
            Helper.SetupUnitForUnitMonitor(_target, redCruiser);

            // Setup gunship factory
            helper.InitialiseBuilding(_factory, Faction.Blues, parentCruiserDirection: Direction.Right, enemyCruiser: redCruiser);
            _factory.StartConstruction();
            _factory.CompletedBuildable += (sender, e) => _factory.StartBuildingUnit(unitPrefab);
            _factory.UnitCompleted += (sender, e) => deferrer.Defer(e.CompletedUnit.Destroy, delayInS: timeToDieInS);
        }
    }
}