using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects
{
    public class Smoke : MonoBehaviour, ISmoke
    {
        private ParticleSystem _particleSystem;

        private SmokeStrength _smokeStrength;
        public SmokeStrength SmokeStrength
        {
            get { return _smokeStrength; }
            set
            {
                if (value != _smokeStrength)
                {
                    SmokeStats smokeStats = GetStatsForStrength(_smokeStrength);

                    if (smokeStats != null)
                    {
                        ApplySmokeStats(smokeStats);
                        _particleSystem.Play();
                    }
                    else
                    {
                        _particleSystem.Pause();
                    }
                }
            }
        }

        public void Initialise()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            Assert.IsNotNull(_particleSystem);
            _particleSystem.Pause();
        }

        private void ApplySmokeStats(SmokeStats smokeStats)
        {
            ParticleSystem.MainModule mainModule = _particleSystem.main;
            mainModule.startLifetime = new ParticleSystem.MinMaxCurve(smokeStats.StartLifetimeMin, smokeStats.StartLifetimeMax);

            ParticleSystem.EmissionModule emissionModule = _particleSystem.emission;
            emissionModule.rateOverTime = smokeStats.EmissionRatePerS;
        }

        private SmokeStats GetStatsForStrength(SmokeStrength strength)
        {
            switch (strength)
            {
                case SmokeStrength.Weak:
                    return StaticSmokeStats.Small.WeakSmoke;

                case SmokeStrength.Normal:
                    return StaticSmokeStats.Small.NormalSmoke;

                case SmokeStrength.Strong:
                    return StaticSmokeStats.Small.StrongSmoke;

                default:
                    return null;
            }
        }
    }
}