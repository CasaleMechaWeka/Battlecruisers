using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects
{
    public class BroadcastingParticleSystem : MonoBehaviour, IBroadcastingParticleSystem
    {
        // FELIX  Make private :)
        public ParticleSystem ParticleSystem { get; private set; }

        public event EventHandler Stopped;

        public void Initialise()
        {
            ParticleSystem = GetComponent<ParticleSystem>();
            Assert.IsNotNull(ParticleSystem);
            ParticleSystem.MainModule mainModule = ParticleSystem.main;
            mainModule.stopAction = ParticleSystemStopAction.Callback;
        }

        public void Play()
        {
            ParticleSystem.Play();
        }

        private void OnParticleSystemStopped()
        {
            Logging.Log(Tags.EXPLOSIONS, $"{ParticleSystem}");

            Stopped?.Invoke(this, EventArgs.Empty);
        }
    }
}