namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    /// <summary>
    /// Null object
    /// </summary>
    public class PvPDummyAngleLimiter : IPvPAngleLimiter
    {
        public float LimitAngle(float desiredAngleInDegrees)
        {
            return desiredAngleInDegrees;
        }
    }
}
