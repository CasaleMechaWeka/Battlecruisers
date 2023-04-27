using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio
{
    public class PvPAudioClipWrapper : IPvPAudioClipWrapper
    {
        public AudioClip AudioClip { get; }
        public float Length => AudioClip.length;
        public AsyncOperationHandle<AudioClip> Handle { get; }

        public PvPAudioClipWrapper(AudioClip audioClip, AsyncOperationHandle<AudioClip> handle = default)
        {
            Assert.IsNotNull(audioClip);

            AudioClip = audioClip;
            Handle = handle;
        }
    }
}
