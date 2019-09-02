using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    public class SandboxTestGod : MonoBehaviour
    {
        void Start()
        {
            Artillery artillery = FindObjectOfType<Artillery>();
            Assert.IsNotNull(artillery);

            //BuildingWrapper wrapper = artillery.GetComponentInParent<BuildingWrapper>();
            //Assert.IsNotNull(wrapper);
            //wrapper.Initialise();

            Helper helper = new Helper(buildSpeedMultiplier: 5);
            helper.InitialiseBuilding(artillery);
            artillery.StartConstruction();
        }
    }
}