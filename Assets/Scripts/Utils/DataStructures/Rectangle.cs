using UnityEngine.Assertions;

namespace BattleCruisers.Utils.DataStrctures
{
    public class Rectangle
    {
        public float MinX { get; }
        public float MaxX { get; }
        public float MinY { get; }
        public float MaxY { get; }

		public Rectangle(float minX, float maxX, float minY, float maxY)
        {
            Assert.IsTrue(minX < maxX);
            Assert.IsTrue(minY < maxY);

            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }
    }
}
