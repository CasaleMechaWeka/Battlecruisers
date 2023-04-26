using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions
{
    public class PvPSynchronizedParticleSystemsController : MonoBehaviour, IPvPSynchronizedParticleSystems
    {
        private static int _randomSeed;
        private ParticleSystem[] _particleSystems;

        public void Initialise()
        {
            Logging.Verbose(Tags.EXPLOSIONS, ToString());
            _particleSystems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);

            if (_randomSeed == 0)
            {
                _randomSeed = RandomGenerator.Instance.Range(0, int.MaxValue);
            }

            Logging.Verbose(Tags.EXPLOSIONS, $"{ToString()}  seed: {_randomSeed}");

            foreach (ParticleSystem particleSystem in _particleSystems)
            {
                if (particleSystem.isPlaying)
                {
                    particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
                particleSystem.randomSeed = (uint)_randomSeed;
                particleSystem.Play();
            }
        }

        public void ResetSeed()
        {
            _randomSeed = RandomGenerator.Instance.Range(0, int.MaxValue);
            Initialise();
        }
    }
}
