using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects
{
    public class BroadcastingParticleSystem : MonoBehaviour, IBroadcastingParticleSystem
    {
        public event EventHandler Stopped;

        public void Initialise()
        {
            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            Assert.IsNotNull(particleSystem);
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }
    }
}