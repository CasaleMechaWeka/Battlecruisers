using UnityEngine.Assertions;

namespace BattleCruisers.Utils.DataStrctures
{
    public class Rectangle
    {
        public float MinX { get; private set; }
        public float MaxX { get; private set; }
        public float MinY { get; private set; }
        public float MaxY { get; private set; }

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
