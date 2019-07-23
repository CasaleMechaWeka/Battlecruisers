namespace BattleCruisers.Targets.TargetDetectors
{
    public interface IManualDetector
    {
        /// <summary>
        /// Should be called periodically, perhaps every 3rd frame or so.  The
        /// target detector will check if any targets have entered or exited.
        /// </summary>
        void Detect();
    }
}