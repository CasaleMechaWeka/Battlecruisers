using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser
{
    public class PvPLaserImpact : MonoBehaviour, IPvPLaserImpact
    {
        private IPvPParticleSystemGroup _effects;
        private IPvPDebouncer _debouncer;

        private const float HIDE_IMPACT_DEBOUNCE_TIME_IN_S = 0.25f;

        public void Initialise(IPvPDeferrer timeScaleDeferrer)
        {
            // Logging.LogMethod(Tags.LASER);
            Assert.IsNotNull(timeScaleDeferrer);

            PvPParticleSystemGroupInitialiser effectsInitialiser = GetComponent<PvPParticleSystemGroupInitialiser>();
            Assert.IsNotNull(effectsInitialiser);
            _effects = effectsInitialiser.CreateParticleSystemGroup();
            _effects.Stop();

            _debouncer = new PvPDeferredDebouncer(PvPTimeBC.Instance.TimeSinceGameStartProvider, timeScaleDeferrer, HIDE_IMPACT_DEBOUNCE_TIME_IN_S);
        }

        public void Show(Vector3 position)
        {
            // Logging.Log(Tags.LASER, $"position: {position}");

            transform.position = position;
            _effects.Play();
            _debouncer.Debounce(Hide);
        }

        private void Hide()
        {
            // Logging.LogMethod(Tags.LASER);
            _effects.Stop();
        }
    }
}