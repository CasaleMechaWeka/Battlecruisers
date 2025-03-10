using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions.Audio
{
    public class AudioListenerBC : IAudioListener
    {
        public void Pause()
        {
            AudioListener.pause = true;
        }

        public void Resume()
        {
            AudioListener.pause = false;
        }
    }
}