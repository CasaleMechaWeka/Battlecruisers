using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;


namespace BattleCruisers.Effects.Explosions
{
    public class CruiserDeathExplosion : ExplosionController
    {
        private AudioSourceGroup _audioSources;

        public IExplosion Initialise(ISettingsManager settingsManager)
        {
            IExplosion explosion = base.Initialise();

            Assert.IsNotNull(settingsManager);

            AudioSource[] platformAudioSources = GetComponentsInChildren<AudioSource>(includeInactive: true);
            Assert.IsTrue(platformAudioSources.Length != 0);
            IList<IAudioSource> audioSources
                = platformAudioSources
                    .Select(audioSource => (IAudioSource)new AudioSourceBC(audioSource))
                    .ToList();

            _audioSources = new AudioSourceGroup(settingsManager, audioSources);

            return explosion;
        }
    }
}