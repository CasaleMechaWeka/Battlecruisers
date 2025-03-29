using BattleCruisers.Buildables.Buildings.Turrets;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPAntiAirTurretBarrelWrapper : PvPLeadingDirectFireBarrelWrapper
    {
        protected override AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(30, 150);
        }
    }
}
