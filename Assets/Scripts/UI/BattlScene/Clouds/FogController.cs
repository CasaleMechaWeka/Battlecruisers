using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class FogController : MonoBehaviour
    {
        public void Initialise(Color fogColor)
        {
            SpriteRenderer[] fogSprites = GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer fogSprite in fogSprites)
            {
                fogSprite.color = fogColor;
            }
        }
    }
}