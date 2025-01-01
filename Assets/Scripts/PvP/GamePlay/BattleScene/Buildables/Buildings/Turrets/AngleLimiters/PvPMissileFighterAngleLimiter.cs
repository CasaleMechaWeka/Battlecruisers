using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPMissileFighterAngleLimiter : AngleLimiter
    {
        public PvPMissileFighterAngleLimiter()
            : base(minAngle: -30, maxAngle: 30)
        {
        }
    }
}
