using System;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
{
    public class SoundFetcher : ISoundFetcher
    {
        private const string SOUND_ROOT_DIR = "Sounds";
        private const char PATH_SEPARATOR = '/';

        public IAudioClipWrapper GetSound(ISoundKey soundKey)
        {
            string soundPath = CreateSoundPath(soundKey);
            AudioClip audioClip = Resources.Load<AudioClip>(soundPath);

            if (audioClip == null)
            {
                throw new ArgumentException("Invalid sound path: " + soundPath);
            }

            return new AudioClipWrapper(audioClip);
        }

        private string CreateSoundPath(ISoundKey soundKey)
        {
            return SOUND_ROOT_DIR + PATH_SEPARATOR + soundKey.Type.ToString() + PATH_SEPARATOR + soundKey.Name;
        }
    }
}
