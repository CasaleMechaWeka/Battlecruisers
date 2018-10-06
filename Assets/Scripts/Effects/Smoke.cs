using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects
{
    public abstract class Smoke : MonoBehaviour, ISmoke
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
                    _smokeStrength = value;

                    SmokeStats smokeStats = GetStatsForStrength(_smokeStrength);

                    if (smokeStats != null)
                    {
                        ApplySmokeStats(smokeStats);
                        _particleSystem.Play();
                    }
                    else
                    {
                        _particleSystem.Stop();
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
            mainModule.maxParticles = smokeStats.MaxNumberOfParticles;

            ParticleSystem.EmissionModule emissionModule = _particleSystem.emission;
            emissionModule.rateOverTime = smokeStats.EmissionRatePerS;
        }

        protected abstract SmokeStats GetStatsForStrength(SmokeStrength strength);
    }
}