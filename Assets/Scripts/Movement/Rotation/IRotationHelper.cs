namespace BattleCruisers.Movement.Rotation
{
    public interface IRotationHelper
    {
        float FindDirectionMultiplier(float currentAngleInRadians, float desiredAngleInDegrees);
    }
}
