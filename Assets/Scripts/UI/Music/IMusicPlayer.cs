namespace BattleCruisers.UI.Music
{
    public interface IMusicPlayer
    {
        float Volume { get; set; }

        void PlayScreensSceneMusic();
        void PlayVictoryMusic();
        void PlayDefeatMusic();
        void PlayTrashMusic();
        void Stop();
    }
}