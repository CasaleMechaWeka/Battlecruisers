using System;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
{
    public class SoundFetcher : ISoundFetcher
    {
        public IAudioClipWrapper GetSound(string soundName)
        {
            AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + soundName);

            if (audioClip == null)
            {
                throw new ArgumentException("Invalid sound name: " + soundName);
            }

            return new AudioClipWrapper(audioClip);
        }
    }
}
