using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures
{
    // PERF  Struct candidate :)
    public class PvPRectangle
    {
        public float MinX { get; }
        public float MaxX { get; }
        public float MinY { get; }
        public float MaxY { get; }

        public PvPRectangle(float minX, float maxX, float minY, float maxY)
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
