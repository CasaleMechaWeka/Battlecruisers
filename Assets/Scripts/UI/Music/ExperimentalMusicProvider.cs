using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.Music
{
    public class ExperimentalMusicProvider : IMusicProvider
    {
        public ISoundKey ScreensSceneKey { get { return SoundKeys.Music.MainTheme; } }
        public ISoundKey BattleSceneKey { get { return SoundKeys.Music.Experimental; } }
        public ISoundKey DangerKey { get { return SoundKeys.Music.ExperimentalDanger; } }
        public ISoundKey VictoryKey { get { return SoundKeys.Music.ExperimentalVictory; } }
    }
}