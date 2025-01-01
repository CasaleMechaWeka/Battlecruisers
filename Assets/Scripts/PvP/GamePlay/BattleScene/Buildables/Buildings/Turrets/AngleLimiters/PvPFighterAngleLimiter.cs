using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPFighterAngleLimiter : AngleLimiter
    {
        public PvPFighterAngleLimiter()
            : base(minAngle: -30, maxAngle: 30)
        {
        }
    }
}
