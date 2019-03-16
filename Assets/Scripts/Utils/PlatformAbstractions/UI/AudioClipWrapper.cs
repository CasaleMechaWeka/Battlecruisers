using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public class AudioClipWrapper : IAudioClipWrapper
    {
        public AudioClip AudioClip { get; }

        public AudioClipWrapper(AudioClip audioClip)
        {
            Assert.IsNotNull(audioClip);
            AudioClip = audioClip;
        }
    }
}
