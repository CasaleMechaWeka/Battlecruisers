using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Pools;
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

        public AudioSourcePoolable Initialise(IDeferrer realTimeDeferrer)
        {
            Assert.IsNotNull(_audioSource);
            PvPHelper.AssertIsNotNull(realTimeDeferrer);

            return
                new AudioSourcePoolable(
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(_audioSource), type),
                    realTimeDeferrer);
        }
    }
}