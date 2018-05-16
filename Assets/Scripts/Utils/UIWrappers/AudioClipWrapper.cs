using UnityEngine;
using UnityEngine.Assertions;

// FELIX  Move into PLatformWrappers namespace :P
namespace BattleCruisers.Utils.UIWrappers
{
    public class AudioClipWrapper : IAudioClipWrapper
    {
        public AudioClip AudioClip { get; private set; }

        public AudioClipWrapper(AudioClip audioClip)
        {
            Assert.IsNotNull(audioClip);
            AudioClip = audioClip;
        }
    }
}
