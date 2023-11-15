namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation
{
    public interface IPvPRotationMovementController
    {
        bool IsOnTarget(float desiredAngleInDegrees);
        void AdjustRotation(float desiredAngleInDegrees);
    }
}