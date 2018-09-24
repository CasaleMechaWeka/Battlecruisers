using BattleCruisers.Scenes.Test.Utilities;
using System;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects
{
    public class SeaTestGod : MonoBehaviour
    {
        private void Start()
        {
            TestAircraftController aircraft = FindObjectOfType<TestAircraftController>();
            Helper helper = new Helper();
            helper.InitialiseUnit(aircraft);
            aircraft.StartConstruction();
            aircraft.CompletedBuildable += Aircraft_CompletedBuildable;
        }

        private void Aircraft_CompletedBuildable(object sender, EventArgs e)
        {
            TestAircraftController aircraft = sender as TestAircraftController;
            aircraft.TakeDamage(aircraft.MaxHealth, damageSource: null);
        }
    }
}