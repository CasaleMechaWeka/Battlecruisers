using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.Music
{
    // FELIX  Deprecated :/
    public class ExperimentalMusicProvider : IMusicProvider
    {
        public ISoundKey ScreensSceneKey { get { return SoundKeys.Music.MainTheme; } }
        public ISoundKey BattleSceneKey { get { return SoundKeys.Music.Background.Experimental; } }
        public ISoundKey DangerKey { get { return SoundKeys.Music.Danger; } }
        public ISoundKey VictoryKey { get { return SoundKeys.Music.Victory; } }
    }
}