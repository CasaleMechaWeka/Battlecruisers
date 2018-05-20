namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public interface IAudioSourceWrapper
    {
        IAudioClipWrapper AudioClip { set; }

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
