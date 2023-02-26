using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.ParticleSystems
{
    public class BroadcastingParticleSystem : MonoBehaviour, IBroadcastingParticleSystem
    {
        private ParticleSystem _particleSystem;

        public event EventHandler Stopped;

        public void Initialise()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            Assert.IsNotNull(_particleSystem);
            ParticleSystem.MainModule mainModule = _particleSystem.main;
            mainModule.stopAction = ParticleSystemStopAction.Callback;
        }

        public void Play()
        {
            if (!_particleSystem.isPlaying)
            {
                _particleSystem.Play();
            }
        }

        public void Stop()
        {
            _particleSystem?.Stop();
        }

        private void OnParticleSystemStopped()
        {
            Logging.Verbose(Tags.EXPLOSIONS, $"{_particleSystem}");

            Stopped?.Invoke(this, EventArgs.Empty);
        }
    }
}