using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.PlatformAbstractions.Audio
{
    public class AudioClipWrapper : IAudioClipWrapper
    {
        public AudioClip AudioClip { get; }
        public float Length => AudioClip.length;
        public AsyncOperationHandle<AudioClip> Handle { get; }

        public AudioClipWrapper(AudioClip audioClip, AsyncOperationHandle<AudioClip> handle = default)
        {
            Assert.IsNotNull(audioClip);

            AudioClip = audioClip;
            Handle = handle;
        }
    }
}
