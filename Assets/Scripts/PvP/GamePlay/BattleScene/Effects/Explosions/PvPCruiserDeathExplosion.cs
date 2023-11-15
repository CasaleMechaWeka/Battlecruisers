using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions
{
    public class PvPCruiserDeathExplosion : PvPExplosionController
    {
        private PvPAudioSourceGroup _audioSources;

        public IPvPExplosion Initialise(ISettingsManager settingsManager)
        {
            IPvPExplosion explosion = base.Initialise();

            Assert.IsNotNull(settingsManager);

            AudioSource[] platformAudioSources = GetComponentsInChildren<AudioSource>(includeInactive: true);
            Assert.IsTrue(platformAudioSources.Length != 0);
            IList<IPvPAudioSource> audioSources
                = platformAudioSources
                    .Select(audioSource => (IPvPAudioSource)new PvPAudioSourceBC(audioSource))
                    .ToList();

            _audioSources = new PvPAudioSourceGroup(settingsManager, audioSources);

            return explosion;
        }
    }
}