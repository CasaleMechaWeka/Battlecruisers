namespace BattleCruisers.UI.Music
{
    public interface IMusicPlayer
    {
        void PlayScreensSceneMusic();
        void PlayVictoryMusic();
        void PlayDefeatMusic();
        void PlayTrashMusic();
        void Stop();
    }
}