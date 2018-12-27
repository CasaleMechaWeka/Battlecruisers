using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.Music
{
    // FELIX  Deprecated :/
    public class KentientMusicProvider : IMusicProvider
    {
        public ISoundKey ScreensSceneKey { get { return SoundKeys.Music.MainTheme; } }
        public ISoundKey BattleSceneKey { get { return SoundKeys.Music.Background.Kentient; } }
        public ISoundKey DangerKey { get { return SoundKeys.Music.Danger; } }
        public ISoundKey VictoryKey { get { return SoundKeys.Music.Victory; } }
    }
}