using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Performance.ObjectPooling
{
    public class UnitRecyclingTestGod : TestGodBase
    {
        public UnitWrapper unitPrefab;

        protected override void Start()
        {
            base.Start();

            Assert.IsNotNull(unitPrefab);
            unitPrefab.Initialise();

            Helper helper = new Helper(buildSpeedMultiplier: BCUtils.BuildSpeedMultipliers.FAST, updaterProvider: _updaterProvider);

            TimeScaleDeferrer deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(deferrer);

            Factory factory = FindObjectOfType<Factory>();
            Assert.IsNotNull(factory);
            helper.InitialiseBuilding(factory);
            factory.StartConstruction();
            factory.CompletedBuildable += (sender, e) => factory.StartBuildingUnit(unitPrefab);
            factory.UnitCompleted += (sender, e) => deferrer.Defer(e.CompletedUnit.Destroy, delayInS: 1);
        }
    }
}