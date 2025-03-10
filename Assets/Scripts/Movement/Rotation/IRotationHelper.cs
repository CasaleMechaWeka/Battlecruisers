namespace BattleCruisers.Movement.Rotation
{
    public interface IRotationHelper
    {
        float FindDirectionMultiplier(float currentAngleInDegrees, float desiredAngleInDegrees);
    }
}
