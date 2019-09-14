using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Sounds
{
    public class UnitReadySoundTestGod : MonoBehaviour
    {
        public UnitWrapper aircraftPrefab;

        void Start()
        {
            Helper helper = new Helper(buildSpeedMultiplier: BCUtils.BuildSpeedMultipliers.DEFAULT_TUTORIAL);

            AirFactory airFactory = FindObjectOfType<AirFactory>();
            Assert.IsNotNull(airFactory);
            helper.InitialiseBuilding(airFactory);
            airFactory.StartConstruction();
            airFactory.CompletedBuildable += (sender, e) => airFactory.StartBuildingUnit(aircraftPrefab);
        }
    }
}