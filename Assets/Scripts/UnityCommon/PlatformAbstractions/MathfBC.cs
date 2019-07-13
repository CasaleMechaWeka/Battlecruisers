using UnityEngine;

namespace UnityCommon.PlatformAbstractions
{
    public class MathfBC : IMathf
    {
        public float Abs(float value)
        {
            return Mathf.Abs(value);
        }

        public float SmoothDamp(
            float current, 
            float target, 
            ref float currentVelocity, 
            float smoothTime, 
            float maxSpeed, 
            float deltaTime)
        {
            return
                Mathf.SmoothDamp(
                    current,
                    target,
                    ref currentVelocity,
                    smoothTime,
                    maxSpeed,
                    deltaTime);
        }
    }
}