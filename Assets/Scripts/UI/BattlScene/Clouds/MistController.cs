using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class MistController : MonoBehaviour
    {
        public void Initialse(Color mistColour)
        {
            SpriteRenderer[] mistSprites = GetComponentsInChildren<SpriteRenderer>();
            
            foreach (SpriteRenderer mistSprite in mistSprites)
            {
                mistSprite.color = mistColour;
            }
        }
    }
}