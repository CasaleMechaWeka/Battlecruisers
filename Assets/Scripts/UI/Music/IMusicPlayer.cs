using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.Music
{
    // FELIX  Remove :P
    public enum Music
    {
        ScreensScene, BattleScene, Danger, Victory
    }

    public interface IMusicPlayer
    {
        ISoundKey LevelMusicKey { set; }

        void PlayScreensSceneMusic();
        void PlayBattleSceneMusic();
        void PlayDangerMusic();
        void PlayVictoryMusic();
        void Stop();
    }
}