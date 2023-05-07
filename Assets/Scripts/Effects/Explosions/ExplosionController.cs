using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionController : ParticleSystemGroupInitialiser
    {
        AudioSource audioSource;
        private ISettingsManager _settingsManager;
        public virtual IExplosion Initialise()
        {

            return
                new Explosion(
                    this,
                    GetParticleSystems(),
                    GetSynchronizedSystems());
        }

        private void Awake()
        {
            _settingsManager = ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager;
            audioSource = GetComponentInChildren<AudioSource>();
            if (audioSource != null)
                audioSource.volume = _settingsManager.EffectVolume * _settingsManager.MasterVolume;
        }


    }

}