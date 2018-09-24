using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.Music
{
    public class KentientMusicProvider : IMusicProvider
    {
        public ISoundKey ScreensSceneKey { get { return SoundKeys.Music.MainTheme; } }
        public ISoundKey BattleSceneKey { get { return SoundKeys.Music.Kentient; } }
        public ISoundKey DangerKey { get { return SoundKeys.Music.KentientDanger; } }
        public ISoundKey VictoryKey { get { return SoundKeys.Music.KentientVictory; } }
    }
}