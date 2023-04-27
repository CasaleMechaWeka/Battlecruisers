using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public class PvPSmokeChanger : IPvPSmokeChanger
    {
        public void Change(ParticleSystem smoke, PvPSmokeStatistics smokeStats)
        {
            Helper.AssertIsNotNull(smoke, smokeStats);

            ChangeMainModule(smoke, smokeStats);
            ChangeEmissionModule(smoke, smokeStats);
            ChangeShapeModule(smoke, smokeStats);
            ChangeNoiseModule(smoke, smokeStats);
            ChangeVelocityOverLifetimeModule(smoke, smokeStats);
            ChangeSizeOverLifetimeModule(smoke, smokeStats);
        }

        private void ChangeMainModule(ParticleSystem smoke, PvPSmokeStatistics smokeStats)
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

        private void ChangeEmissionModule(ParticleSystem smoke, PvPSmokeStatistics smokeStats)
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

        private void ChangeShapeModule(ParticleSystem smoke, PvPSmokeStatistics smokeStats)
        {
            if (smokeStats.EdgeRadius != null)
            {
                ParticleSystem.ShapeModule shapeModule = smoke.shape;
                shapeModule.radius = (float)smokeStats.EdgeRadius;
            }
        }

        private void ChangeNoiseModule(ParticleSystem smoke, PvPSmokeStatistics smokeStats)
        {
            if (smokeStats.NoiseStrength != null)
            {
                ParticleSystem.NoiseModule noiseModule = smoke.noise;
                noiseModule.strength = (float)smokeStats.NoiseStrength;
            }
        }

        private void ChangeVelocityOverLifetimeModule(ParticleSystem smoke, PvPSmokeStatistics smokeStats)
        {
            if (smokeStats.VelocityOverLifetimeY != null)
            {
                ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = smoke.velocityOverLifetime;
                velocityOverLifetimeModule.yMultiplier = (float)smokeStats.VelocityOverLifetimeY;
            }
        }

        private void ChangeSizeOverLifetimeModule(ParticleSystem smoke, PvPSmokeStatistics smokeStats)
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