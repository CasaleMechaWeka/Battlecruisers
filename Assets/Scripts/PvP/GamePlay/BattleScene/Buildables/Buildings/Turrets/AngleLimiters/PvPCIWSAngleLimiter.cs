using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPCIWSAngleLimiter : AngleLimiter
    {
        public PvPCIWSAngleLimiter()
            : base(minAngle: -30, maxAngle: 180)
        {
        }
    }
}
