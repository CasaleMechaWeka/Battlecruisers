using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Smoke
{
    // FELIX  Interface
    public class SmokeChanger
    {
        public void Change(ParticleSystem smoke, SmokeStatistics smokeStats)
        {
            Helper.AssertIsNotNull(smoke, smokeStats);

            ChangeMainModule(smoke, smokeStats);
            ChangeEmissionModule(smoke, smokeStats);
            ChangeShapeModule(smoke, smokeStats);
            ChangeNoiseModule(smoke, smokeStats);
            ChangeVelocityOverLifetimeModule(smoke, smokeStats);
            ChangeSizeOverLifetimeModule(smoke, smokeStats);
        }

        private void ChangeMainModule(ParticleSystem smoke, SmokeStatistics smokeStats)
        {
            ParticleSystem.MainModule mainModule = smoke.main;

            if (smokeStats.StartLifetime != null)
            {
                mainModule.startLifetime = new ParticleSystem.MinMaxCurve(smokeStats.StartLifetime.Min, smokeStats.StartLifetime.Max);
            }
            if (smokeStats.StartSpeed != null)
            {
                mainModule.startSpeed = new ParticleSystem.MinMaxCurve((float)smokeStats.StartSpeed);
            }
            if (smokeStats.StartSize != null)
            {
                ParticleSystem.MinMaxCurve startSizeCurve = mainModule.startSize;
                startSizeCurve.curveMultiplier = (float)smokeStats.StartSize;
                mainModule.startSize = startSizeCurve;
            }
        }

        private void ChangeEmissionModule(ParticleSystem smoke, SmokeStatistics smokeStats)
        {
            ParticleSystem.EmissionModule emissionModule = smoke.emission;
            if (smokeStats.RateOverTime != null)
            {
                emissionModule.rateOverTime = (float)smokeStats.RateOverTime;
            }
            if (smokeStats.RateOverDistance != null)
            {
                emissionModule.rateOverDistance = (float)smokeStats.RateOverDistance;
            }
            if (smokeStats.BurstsCount != null)
            {
                emissionModule.burstCount = (int)smokeStats.BurstsCount;
            }
        }

        private void ChangeShapeModule(ParticleSystem smoke, SmokeStatistics smokeStats)
        {
            if (smokeStats.EdgeRadius != null)
            {
                ParticleSystem.ShapeModule shapeModule = smoke.shape;
                shapeModule.radius = (float)smokeStats.EdgeRadius;
            }
        }

        private void ChangeNoiseModule(ParticleSystem smoke, SmokeStatistics smokeStats)
        {
            if (smokeStats.NoiseStrength != null)
            {
                ParticleSystem.NoiseModule noiseModule = smoke.noise;
                noiseModule.strength = (float)smokeStats.NoiseStrength;
            }
        }

        private void ChangeVelocityOverLifetimeModule(ParticleSystem smoke, SmokeStatistics smokeStats)
        {
            if (smokeStats.VelocityOverLifetimeY != null)
            {
                ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = smoke.velocityOverLifetime;
                velocityOverLifetimeModule.orbitalOffsetYMultiplier = (float)smokeStats.VelocityOverLifetimeY;
            }
        }

        private void ChangeSizeOverLifetimeModule(ParticleSystem smoke, SmokeStatistics smokeStats)
        {
            if (smokeStats.StartSize != null)
            {
                ParticleSystem.SizeOverLifetimeModule sizeOverLifetimeModule = smoke.sizeOverLifetime;
                ParticleSystem.MinMaxCurve sizeCurve = sizeOverLifetimeModule.size;
                sizeCurve.curveMultiplier = (float)smokeStats.StartSize;
            }
        }
    }
}