namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public interface IPvPManualDetector
    {
        /// <summary>
        /// Should be called periodically, perhaps every 3rd frame or so.  The
        /// target detector will check if any targets have entered or exited.
        /// </summary>
        void Detect();
    }
}
