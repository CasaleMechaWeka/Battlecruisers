using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools
{
    public class PvPAudioSourceInitialiser : PvPPrefab
    {
        [SerializeField]
        private AudioSource _audioSource;
        public int type = -1;

        public PvPAudioSourcePoolable Initialise(IDeferrer realTimeDeferrer, ISettingsManager settingsManager)
        {
            Assert.IsNotNull(_audioSource);
            PvPHelper.AssertIsNotNull(realTimeDeferrer, settingsManager);

            return
                new PvPAudioSourcePoolable(
                    new PvPEffectVolumeAudioSource(
                        new AudioSourceBC(_audioSource),
                        settingsManager, type),
                    realTimeDeferrer);
        }
    }
}