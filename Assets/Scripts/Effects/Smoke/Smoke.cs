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
        private ISmokeChanger _smokeChanger;
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

                    SmokeStatistics smokeStats = GetStatsForStrength(_smokeStrength);

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

        public void Initialise(ISmokeChanger smokeChanger)
        {
            Assert.IsNotNull(smokeChanger);
            _smokeChanger = smokeChanger;

            _particleSystem = GetComponent<ParticleSystem>();
            Assert.IsNotNull(_particleSystem);
            _particleSystem.Pause();
        }

        private void ApplySmokeStats(SmokeStatistics smokeStats)
        {
            _smokeChanger.Change(_particleSystem, smokeStats);
        }

        // Would normally make abstract, but see class summary comment.
        protected virtual SmokeStatistics GetStatsForStrength(SmokeStrength strength)
        {
            throw new NotImplementedException();
        }
    }
}