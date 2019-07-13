namespace UnityCommon.PlatformAbstractions
{
    public interface IMathf
    {
        float Abs(float value);
        float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime);
    }
}