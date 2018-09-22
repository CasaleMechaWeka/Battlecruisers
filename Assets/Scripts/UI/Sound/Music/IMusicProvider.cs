using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.Music
{
    public interface IMusicProvider
    {
        ISoundKey ScreensSceneKey { get; }
        ISoundKey BattleSceneKey { get; }
        ISoundKey DangerKey { get; }
        ISoundKey VictoryKey { get; }
    }
}