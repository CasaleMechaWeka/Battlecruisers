namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation
{
    public interface IPvPRotationHelper
    {
        float FindDirectionMultiplier(float currentAngleInDegrees, float desiredAngleInDegrees);
    }
}
