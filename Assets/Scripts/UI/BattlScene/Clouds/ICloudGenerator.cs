using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public enum CloudDensity
    {
        Low, Medium, High
    }

    public enum CloudMovementSpeed
    {
        Slow, Fast
    }

    public interface ICloudGenerator
    {
        void GenerateClouds(
            Rect cloudArea, 
            CloudDensity density = CloudDensity.Medium, 
            CloudMovementSpeed movementSpeed = CloudMovementSpeed.Slow);
    }
}
