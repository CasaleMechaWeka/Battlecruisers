using BattleCruisers.Movement.Rotation;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Used by:
    /// + Tesla coil
    /// </summary>
    public class PvPInvisibleDirectFireBarrelWrapper : PvPDirectFireBarrelWrapper
    {
        protected override IRotationMovementController CreateRotationMovementController(IBarrelController barrel, IDeltaTimeProvider deltaTimeProvider)
        {
            return new DummyRotationMovementController();
        }

        protected override AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(-180, 180);
        }
    }
}
