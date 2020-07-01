using BattleCruisers.Scenes.Test.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Smokes
{
    public class AircraftSmokeIntensityTestGod : TestGodBase
    {
        private int _numOfCompletedAircraft;
        private TestAircraftController[] _aircraft;
        public List<Vector2> patrolPoints;

        protected override List<GameObject> GetGameObjects()
        {
            Assert.IsTrue(patrolPoints.Count == 2);

            _aircraft = FindObjectsOfType<TestAircraftController>();
            Assert.IsTrue(_aircraft.Length != 0);

            _numOfCompletedAircraft = 0;

            return
                _aircraft
                    .Select(aircraft => aircraft.GameObject)
                    .ToList();
        }

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);

            foreach (TestAircraftController aircraft in _aircraft)
            {
                float fuzzedCruisingAltitute = FuzzCruisingAltitude(patrolPoints[0].y);
                aircraft.PatrolPoints = new List<Vector2>()
                {
                    new Vector2(patrolPoints[0].x, fuzzedCruisingAltitute),
                    new Vector2(patrolPoints[1].x, fuzzedCruisingAltitute)
                };
                helper.InitialiseUnit(aircraft);
                aircraft.StartConstruction();
                aircraft.CompletedBuildable += Aircraft_CompletedBuildable;
            }
        }

        private void Aircraft_CompletedBuildable(object sender, EventArgs e)
        {
            _numOfCompletedAircraft++;

            if (_numOfCompletedAircraft == _aircraft.Length)
            {
                DamagePlanes(_aircraft[0].MaxHealth - 1);
            }
        }

        private float FuzzCruisingAltitude(float cruisingAltitudeInM)
        {
            return BCUtils.RandomGenerator.Instance.RangeFromCenter(cruisingAltitudeInM, radius: 1);
        }

        public void KillPlanes()
        {
            DamagePlanes(_aircraft[0].MaxHealth);
        }

        private void DamagePlanes(float damage)
        {
            foreach (TestAircraftController aircraft in _aircraft)
            {
                aircraft.TakeDamage(damage, damageSource: null);
            }
        }
    }
}