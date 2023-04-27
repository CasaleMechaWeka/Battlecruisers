using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    /// <summary>
    /// Usually I would make this an abstract class, but then Unity complains
    /// when I do:
    /// 
    /// Smoke smoke = GetComponent<Smoke>();
    /// 
    /// in SmokeInitialiser.  Sigh, so not making it abstract :)
    /// </summary>
    public class PvPSmoke : MonoBehaviour, IPvPSmoke
    {
        private IPvPSmokeChanger _smokeChanger;
        private ParticleSystem _particleSystem;

        private PvPSmokeStrength _smokeStrength;
        public PvPSmokeStrength SmokeStrength
        {
            get { return _smokeStrength; }
            set
            {
                if (value != _smokeStrength)
                {
                    _smokeStrength = value;

                    PvPSmokeStatistics smokeStats = GetStatsForStrength(_smokeStrength);

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

        public void Initialise(IPvPSmokeChanger smokeChanger)
        {
            Assert.IsNotNull(smokeChanger);
            _smokeChanger = smokeChanger;

            _particleSystem = GetComponent<ParticleSystem>();
            Assert.IsNotNull(_particleSystem);
            _particleSystem.Pause();

            transform.rotation = Quaternion.Euler(-90, 0, 0);
        }

        private void ApplySmokeStats(PvPSmokeStatistics smokeStats)
        {
            _smokeChanger.Change(_particleSystem, smokeStats);
        }

        // Would normally make abstract, but see class summary comment.
        protected virtual PvPSmokeStatistics GetStatsForStrength(PvPSmokeStrength strength)
        {
            throw new NotImplementedException();
        }
    }
}