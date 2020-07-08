using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions.Audio
{
    public class AudioClipWrapper : IAudioClipWrapper
    {
        public AudioClip AudioClip { get; }
        public float Length => AudioClip.length;

        public AudioClipWrapper(AudioClip audioClip)
        {
            Assert.IsNotNull(audioClip);
            AudioClip = audioClip;
        }
    }
}
