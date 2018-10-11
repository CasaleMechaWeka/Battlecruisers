using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Smoke
{
    /// <summary>
    /// Usually I would make this an abstract class, but then Unity complains
    /// when I do:
    /// 
    /// Smoke smoke = GetComponent<Smoke>();
    /// 
    /// in SmokeInitialiser.  Sigh, so not making it abstract :)
    /// </summary>
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

        // Would normally make abstract, but see class summary comment.
        protected virtual SmokeStats GetStatsForStrength(SmokeStrength strength)
        {
            throw new NotImplementedException();
        }
    }
}