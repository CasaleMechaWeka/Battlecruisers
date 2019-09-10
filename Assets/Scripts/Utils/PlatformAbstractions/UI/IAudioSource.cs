namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public interface IAudioSource
    {
        bool IsPlaying { get; }
        IAudioClipWrapper AudioClip { set; }
        float Volume { get; set; }

        /// <param name="isSpatial">
        /// True if the sound should get quieter the further away from the camera it is.
        /// </param>
        /// <param name="loop">
        /// True if the sound should loop (eg: engine), false otherwise.
        /// </param>
        void Play(bool isSpatial = true, bool loop = false);

        void Stop();
    }
}
