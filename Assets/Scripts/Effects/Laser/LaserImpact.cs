using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.Timers;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Laser
{
    public class LaserImpact : MonoBehaviour, ILaserImpact
    {
        private IParticleSystemGroup _effects;
        private IDebouncer _debouncer;

        private const float HIDE_IMPACT_DEBOUNCE_TIME_IN_S = 0.25f;

        public void Initialise(IDeferrer timeScaleDeferrer)
        {
            Logging.LogMethod(Tags.LASER);
            Assert.IsNotNull(timeScaleDeferrer);

            ParticleSystemGroupInitialiser effectsInitialiser = GetComponent<ParticleSystemGroupInitialiser>();
            Assert.IsNotNull(effectsInitialiser);
            _effects = effectsInitialiser.CreateParticleSystemGroup();
            _effects.Stop();

            _debouncer = new DeferredDebouncer(TimeBC.Instance.TimeSinceGameStartProvider, timeScaleDeferrer, HIDE_IMPACT_DEBOUNCE_TIME_IN_S);
        }

        public void Show(Vector3 position)
        {
            Logging.Log(Tags.LASER, $"position: {position}");

            transform.position = position;
            _effects.Play();

            _debouncer.Debounce(Hide);
        }

        private void Hide()
        {
            Logging.LogMethod(Tags.LASER);
            _effects.Stop();
        }
    }
}