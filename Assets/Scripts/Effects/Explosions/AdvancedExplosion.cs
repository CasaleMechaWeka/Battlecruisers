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
            }

            _particleSystems = GetComponentsInChildren<ParticleSystem>();
        }

        public void Activate(Vector3 activationArgs)
        {
            // FELIX
            throw new NotImplementedException();
        }

        public void Show(Vector3 position)
        {
            gameObject.transform.position = position;
           
            foreach (ParticleSystem particleSystem in _particleSystems)
            {
                particleSystem.Play();
            }
        }
    }
}