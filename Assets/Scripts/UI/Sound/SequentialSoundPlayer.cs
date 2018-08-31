using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound
{
    // FELIX  Test
    public class SequentialSoundPlayer : ISoundPlayer
    {
        private readonly IAwaitableAudioSource _audioSource;
        // FELIX  Initialise :P
        private readonly ISoundFetcher _soundFetcher;
        private readonly Queue<ISoundKey> _soundsToPlay;

        public SequentialSoundPlayer(IAwaitableAudioSource audioSource)
        {
            Assert.IsNotNull(audioSource);

            _audioSource = audioSource;
            _soundsToPlay = new Queue<ISoundKey>();
        }

        public void PlaySound(ISoundKey soundKey)
        {
            _soundsToPlay.Enqueue(soundKey);

            if (_soundsToPlay.Count == 1)
            {
                // Not currently playing any sound, so can play new sound immediately
                PlayOldestSound();
            }
        }

        private void PlayOldestSound()
        {
            IAudioClipWrapper audioClip = _soundFetcher.GetSound(_soundsToPlay.Peek());
            _audioSource.Play(audioClip, OnSoundCompleted);

        }

        private void OnSoundCompleted()
        {
            Assert.AreNotEqual(_soundsToPlay.Count, 0);
            _soundsToPlay.Dequeue();
            PlayOldestSound();
        }

        public void PlaySound(ISoundKey soundKey, Vector2 position)
        {
            // FELIX
        }
    }
}
