using System;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    public class AdvancedExplosion : MonoBehaviour, IExplosion
    {
        private ParticleSystem[] _particleSystems;

        public event EventHandler Deactivated;

        public void Initialise(IRandomGenerator randomGenerator)
        {
            Assert.IsNotNull(randomGenerator);

            SynchronizedParticleSystemsController[] fireSmokePairs = GetComponentsInChildren<SynchronizedParticleSystemsController>(includeInactive: true);

            foreach (SynchronizedParticleSystemsController fireSmokePair in fireSmokePairs)
            {
                fireSmokePair.Initialise(randomGenerator);
                fireSmokePair.Stopped += FireSmokePair_Stopped;
            }

            _particleSystems = GetComponentsInChildren<ParticleSystem>();

            gameObject.SetActive(false);
        }

        // All fire and smoke pairs should complete at the same time, so deactivate
        // when first pair completes.
        private void FireSmokePair_Stopped(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public void Activate(Vector3 position)
        {
            gameObject.SetActive(true);
            gameObject.transform.position = position;
           
            foreach (ParticleSystem particleSystem in _particleSystems)
            {
                particleSystem.Play();
            }
        }
    }
}