using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.Music
{
    public interface IMusicPlayer
    {
        ISoundKey LevelMusicKey { set; }

        void PlayScreensSceneMusic();
        void PlayBattleSceneMusic();
        void PlayDangerMusic();
        void PlayVictoryMusic();
        void PlayDefeatMusic();
        void Stop();
    }
}