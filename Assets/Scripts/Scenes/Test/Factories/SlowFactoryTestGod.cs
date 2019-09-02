using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class SlowFactoryTestGod : MonoBehaviour
    {
        public UnitWrapper unitPrefab;

        void Start()
        {
            Helper helper = new Helper(buildSpeedMultiplier: 10);

            Factory factory = FindObjectOfType<Factory>();
            Assert.IsNotNull(factory);
            helper.InitialiseBuilding(factory);
            factory.StartConstruction();
            factory.CompletedBuildable += (sender, e) => factory.StartBuildingUnit(unitPrefab);
        }
    }
}