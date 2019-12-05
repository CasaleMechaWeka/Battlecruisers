using BattleCruisers.Effects.Smoke;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Smokes
{
    public class SmokeStatsTestGod : MonoBehaviour
    {
        private SmokeChanger _smokeChanger;

        public ParticleSystem shipSmoke, aircraftSmoke, buildingSmoke;

        void Start()
        {
            Assert.IsNotNull(shipSmoke);
            Assert.IsNotNull(aircraftSmoke);
            // FELIX
            //Assert.IsNotNull(buildingSmoke);

            shipSmoke.Play();
            aircraftSmoke.Play();
            shipSmoke.Play();

            _smokeChanger = new SmokeChanger();
        }

        public void WeakSmoke()
        {
            _smokeChanger.Change(shipSmoke, StaticSmokeStats.Ship.Weak);
            _smokeChanger.Change(aircraftSmoke, StaticSmokeStats.Aircraft.Weak);
        }

        public void NormalSmoke()
        {
            _smokeChanger.Change(shipSmoke, StaticSmokeStats.Ship.Normal);
            _smokeChanger.Change(aircraftSmoke, StaticSmokeStats.Aircraft.Normal);
        }

        public void StrongSmoke()
        {
            _smokeChanger.Change(shipSmoke, StaticSmokeStats.Ship.Strong);
            _smokeChanger.Change(aircraftSmoke, StaticSmokeStats.Aircraft.Strong);
        }
    }
}