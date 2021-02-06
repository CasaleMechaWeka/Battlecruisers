namespace BattleCruisers.UI.Music
{
    public interface IMusicPlayer
    {
        float Volume { set; }

        void PlayScreensSceneMusic();
        void PlayVictoryMusic();
        void PlayDefeatMusic();
        void PlayTrashMusic();
        void Stop();
    }
}