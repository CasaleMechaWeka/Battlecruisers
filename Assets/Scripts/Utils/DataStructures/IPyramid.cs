using UnityEngine;

namespace BattleCruisers.Utils.DataStrctures
{
    /// <summary>
    /// A tringale with a flat base, with the apex in the middle
    /// of the two base vertices.
    /// </summary>
    public interface IPyramid
    {
        Vector2 BottomLeftVertex { get; }
        Vector2 BottomRightVertex { get; }
        Vector2 TopCenterVertex { get; }
        float Width { get; }
        float Height { get; }

        float FindMaxY(float xPosition);
        IRange<float> FindGlobalXRange(float yPosition);
    }
}