using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundImageStats
    {
        public float Scale;
        public Vector2 PositionAt4to3;
        public float YPositionAt16to9;
        public float YPositionAt24to10;
        public string SpriteName;
        public Color Colour;
        public bool FlipX = false;
        public int OrderInLayer;

        public BackgroundImageStats(float scale, 
                                    Vector2 positionAt4to3,
                                    float yPositionAt16to9,
                                    float yPositionAt24to10,
                                    string spriteName,
                                    Color colour,
                                    bool flipX,
                                    int orderInLayer)
        {
            Scale = scale;
            PositionAt4to3 = positionAt4to3;
            YPositionAt16to9 = yPositionAt16to9;
            YPositionAt24to10 = yPositionAt24to10;
            SpriteName = spriteName;
            Colour = colour;
            FlipX = flipX;
            OrderInLayer = orderInLayer;
        }
    }
}