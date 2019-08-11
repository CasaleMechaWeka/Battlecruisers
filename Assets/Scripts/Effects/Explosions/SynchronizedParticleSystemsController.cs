using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    public class SynchronizedParticleSystemsController : MonoBehaviour, IBroadcastingParticleSystem
    {
        private int _numOfParticleSystems;
        private int _numOfStoppedSystems = 0;

        public event EventHandler Stopped;

        public void Initialise(IRandomGenerator randomGenerator)
        {
            Assert.IsNotNull(randomGenerator);

            int seed = randomGenerator.Range(0, int.MaxValue);

            BroadcastingParticleSystem[] particleSystems = GetComponentsInChildren<BroadcastingParticleSystem>(includeInactive: true);
            _numOfParticleSystems = particleSystems.Length;

            foreach (BroadcastingParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Initialise();
                particleSystem.ParticleSystem.randomSeed = (uint)seed;
                particleSystem.Stopped += ParticleSystem_Stopped;
            }
        }

        private void ParticleSystem_Stopped(object sender, EventArgs e)
        {
            _numOfStoppedSystems++;

            if (_numOfStoppedSystems == _numOfParticleSystems)
            {
                _numOfStoppedSystems = 0;
                Stopped?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}