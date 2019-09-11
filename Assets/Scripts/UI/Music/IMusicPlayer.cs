using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.Music
{
    public interface IMusicPlayer
    {
        // FELIX  Remove :)
        ISoundKey LevelMusicKey { set; }
        
        // FELIX  Remove :)
        void PlayBattleSceneMusic();
        void PlayDangerMusic();

        void PlayScreensSceneMusic();
        void PlayVictoryMusic();
        void PlayDefeatMusic();
        void PlayLoadingMusic();
        void Stop();
    }
}