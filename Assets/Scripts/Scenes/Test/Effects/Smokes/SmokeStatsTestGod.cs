using BattleCruisers.Effects.Smoke;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Smokes
{
    public class SmokeStatsTestGod : MonoBehaviour
    {
        public ParticleSystem smoke;

        void Start()
        {
            Assert.IsNotNull(smoke);

            //ApplySmokeStats(smoke, StaticSmokeStats.Ship.Weak);
            //ApplySmokeStats(smoke, StaticSmokeStats.Ship.Normal);
            //ApplySmokeStats(smoke, StaticSmokeStats.Ship.Strong);

            smoke.Play();
        }

        public void WeakSmoke()
        {
            ApplySmokeStats(smoke, StaticSmokeStats.Ship.Weak);
        }

        public void NormalSmoke()
        {
            ApplySmokeStats(smoke, StaticSmokeStats.Ship.Normal);
        }

        public void StrongSmoke()
        {
            ApplySmokeStats(smoke, StaticSmokeStats.Ship.Strong);
        }

        private void ApplySmokeStats(ParticleSystem smoke, SmokeStatistics smokeStats)
        {
            ParticleSystem.MainModule mainModule = smoke.main;
            mainModule.startLifetime = new ParticleSystem.MinMaxCurve(smokeStats.StartLifetime.Min, smokeStats.StartLifetime.Max);
            mainModule.startSpeed = new ParticleSystem.MinMaxCurve(smokeStats.StartSpeed);
            ParticleSystem.MinMaxCurve startSizeCurve = mainModule.startSize;
            startSizeCurve.curveMultiplier = smokeStats.StartSize;
            mainModule.startSize = startSizeCurve;

            ParticleSystem.EmissionModule emissionModule = smoke.emission;
            emissionModule.rateOverTime = smokeStats.RateOverTime;
            emissionModule.rateOverDistance = smokeStats.RateOverDistance;
            emissionModule.burstCount = smokeStats.BurstsCount;

            ParticleSystem.ShapeModule shapeModule = smoke.shape;
            shapeModule.radius = smokeStats.EdgeRadius;

            ParticleSystem.NoiseModule noiseModule = smoke.noise;
            noiseModule.strength = smokeStats.NoiseStrength;

            ParticleSystem.SizeOverLifetimeModule sizeOverLifetimeModule = smoke.sizeOverLifetime;
            ParticleSystem.MinMaxCurve sizeCurve = sizeOverLifetimeModule.size;
            sizeCurve.curveMultiplier = smokeStats.StartSize;
        }
    }
}