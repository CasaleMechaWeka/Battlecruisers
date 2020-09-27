using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
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

        public void Show(Vector3 position)
        {
            Logging.Log(Tags.LASER, $"position: {position}");

            transform.position = position;
            _effects.Play();
        }

        public void Hide()
        {
            Logging.LogMethod(Tags.LASER);
            _effects.Stop();
        }
    }
}