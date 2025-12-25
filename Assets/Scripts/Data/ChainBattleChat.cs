using UnityEngine;

namespace BattleCruisers.Data
{
    // Simplified chat system using localization keys
    [System.Serializable]
    public class ChainBattleChat
    {
        public string chatKey; // Chat key in format "level{number}/{chatName}" (e.g., "level32/PlayerChat1")
        public SpeakerType speaker = SpeakerType.EnemyCaptain;
        public float displayDuration = 4f;
        [TextArea(2, 4)]
        public string englishText; // Optional English text for translation context

        public enum SpeakerType
        {
            EnemyCaptain,
            PlayerCaptain,
            Narrative
        }
    }
}
