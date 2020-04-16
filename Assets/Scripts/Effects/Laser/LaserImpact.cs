using BattleCruisers.Effects.ParticleSystems;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Laser
{
    public class LaserImpact : MonoBehaviour, ILaserImpact
    {
        private IParticleSystemGroup _effects;

        public void Initialise()
        {
            ParticleSystemGroupInitialiser effectsInitialiser = GetComponent<ParticleSystemGroupInitialiser>();
            Assert.IsNotNull(effectsInitialiser);
            _effects = effectsInitialiser.CreateParticleSystemGroup();
            _effects.Stop();
        }

        public void Show(Vector3 postion)
        {
            transform.position = postion;
            _effects.Play();
        }

        public void Hide()
        {
            _effects.Stop();
        }
    }
}