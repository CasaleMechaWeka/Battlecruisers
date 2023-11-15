using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Used by:
    /// + Tesla coil
    /// </summary>
    public class PvPInvisibleDirectFireBarrelWrapper : PvPDirectFireBarrelWrapper
    {
        protected override IPvPRotationMovementController CreateRotationMovementController(IPvPBarrelController barrel, IPvPDeltaTimeProvider deltaTimeProvider)
        {
            return _factoryProvider.MovementControllerFactory.CreateDummyRotationMovementController();
        }

        protected override IPvPAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateDummyLimiter();
        }
    }
}
