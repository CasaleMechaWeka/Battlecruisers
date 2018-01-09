namespace BattleCruisers.Utils.UIWrappers
{
    public interface IAudioSourceWrapper
    {
        IAudioClipWrapper AudioClip { set; }

        void Play(bool loop = false);
        void Stop();
    }
}
