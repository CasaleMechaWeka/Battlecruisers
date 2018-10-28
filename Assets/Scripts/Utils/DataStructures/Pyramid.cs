using UnityEngine;

namespace BattleCruisers.Utils.DataStrctures
{
    public class Pyramid : IPyramid
    {
        public Vector2 BottomLeftVertex { get; private set; }
        public Vector2 BottomRightVertex { get; private set; }
        public Vector2 TopCenterVertex { get; private set; }

        public Pyramid(Vector2 bottomLeftVertex, Vector2 bottomRightVertex, Vector2 topCenterVertex)
        {
            BottomLeftVertex = bottomLeftVertex;
            BottomRightVertex = bottomRightVertex;
            TopCenterVertex = topCenterVertex;
        }
    }
}