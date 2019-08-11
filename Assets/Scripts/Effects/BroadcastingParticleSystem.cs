using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects
{
    public class BroadcastingParticleSystem : MonoBehaviour, IBroadcastingParticleSystem
    {
        public ParticleSystem ParticleSystem { get; private set; }

        public event EventHandler Stopped;

        public void Initialise()
        {
            ParticleSystem = GetComponent<ParticleSystem>();
            Assert.IsNotNull(ParticleSystem);
            ParticleSystem.MainModule mainModule = ParticleSystem.main;
            mainModule.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }
    }
}