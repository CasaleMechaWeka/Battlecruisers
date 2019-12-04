using BattleCruisers.Effects.Smoke;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Smokes
{
    public class SmokeStatsTestGod : MonoBehaviour
    {
        private SmokeChanger _smokeChanger;

        public ParticleSystem smoke;

        void Start()
        {
            Assert.IsNotNull(smoke);
            smoke.Play();
            _smokeChanger = new SmokeChanger();
        }

        public void WeakSmoke()
        {
            _smokeChanger.Change(smoke, StaticSmokeStats.Ship.Weak);
        }

        public void NormalSmoke()
        {
            _smokeChanger.Change(smoke, StaticSmokeStats.Ship.Normal);
        }

        public void StrongSmoke()
        {
            _smokeChanger.Change(smoke, StaticSmokeStats.Ship.Strong);
        }
    }
}