using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio
{
    public class PvPAudioListenerBC : IAudioListener
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