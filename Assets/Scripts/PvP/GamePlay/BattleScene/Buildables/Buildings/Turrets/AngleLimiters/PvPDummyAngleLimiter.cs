using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    /// <summary>
    /// Null object
    /// </summary>
    public class PvPDummyAngleLimiter : IAngleLimiter
    {
        public float LimitAngle(float desiredAngleInDegrees)
        {
            return desiredAngleInDegrees;
        }
    }
}
