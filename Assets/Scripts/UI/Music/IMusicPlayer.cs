namespace BattleCruisers.UI.Music
{
    public enum Music
    {
        ScreensScene, BattleScene, Danger, Victory
    }

    public interface IMusicPlayer
    {
        void PlayScreensSceneMusic();
        void PlayBattleSceneMusic();
        void PlayDangerMusic();
        void PlayVictoryMusic();
        void Stop();
    }
}