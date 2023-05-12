using BattleCruisers.Effects.Explosions;
using UnityEngine.Assertions;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data;
using UnityEngine;

namespace BattleCruisers.Effects.ParticleSystems
{
    public class ParticleSystemGroupInitialiser : MonoBehaviourWrapper, IParticleSystemGroupInitialiser
    {
        AudioSource audioSource;
        private ISettingsManager _settingsManager;
        public IParticleSystemGroup CreateParticleSystemGroup()
        {
            return
                new ParticleSystemGroup(
                    GetParticleSystems(),
                    GetSynchronizedSystems());
        }

        protected virtual IBroadcastingParticleSystem[] GetParticleSystems()
        {
            BroadcastingParticleSystem[] particleSystems = GetComponentsInChildren<BroadcastingParticleSystem>();
            Assert.IsTrue(particleSystems.Length != 0);

            foreach (BroadcastingParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Initialise();
            }

            return particleSystems;
        }

        protected ISynchronizedParticleSystems[] GetSynchronizedSystems()
        {
            SynchronizedParticleSystemsController[] synchronizedSystems = GetComponentsInChildren<SynchronizedParticleSystemsController>();

            foreach (SynchronizedParticleSystemsController system in synchronizedSystems)
            {
                system.Initialise();
            }

            return synchronizedSystems;
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