using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools
{
    public class PvPAudioSourceInitialiser : PvPPrefab
    {
        [SerializeField]
        private AudioSource _audioSource;
        public int type = -1;

        public IAudioSource Initialise()
        {
            Assert.IsNotNull(_audioSource);

            return
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(_audioSource), type);
        }
    }
}