using BattleCruisers.Movement.Rotation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation
{
    public class PvPDummyRotationMovementController : IRotationMovementController
    {
        private readonly bool _isOnTarget;

        public PvPDummyRotationMovementController(bool isOnTarget = true)
        {
            _isOnTarget = isOnTarget;
        }

        public bool IsOnTarget(float desiredAngleInDegrees)
        {
            return _isOnTarget;
        }

        public void AdjustRotation(float desiredAngleInDegrees) { }
    }
}