using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class BulletImpactExplosion : MonoBehaviour, IExplosion
    {
        private BroadcastingParticleSystem _sparks;
        private ParticleSystem _blast;

        public event EventHandler Deactivated;

        public void Initialise()
        {
            _sparks = transform.FindNamedComponent<BroadcastingParticleSystem>("sparks");
            _sparks.Initialise();

            _blast = transform.FindNamedComponent<ParticleSystem>("blast");

            // Sparks is longer particle system, so wait for it to complete
            _sparks.Stopped += _sparks_Stopped;

            gameObject.SetActive(false);
        }

        private void _sparks_Stopped(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public void Activate(Vector3 position)
        {
            gameObject.SetActive(true);
            gameObject.transform.position = position;

            _blast.Play();
            _sparks.ParticleSystem.Play();
        }
    }
}