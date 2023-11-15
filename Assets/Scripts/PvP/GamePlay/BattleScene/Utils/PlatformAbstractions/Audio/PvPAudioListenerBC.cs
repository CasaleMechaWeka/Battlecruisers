using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio
{
    public class PvPAudioListenerBC : IPvPAudioListener
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