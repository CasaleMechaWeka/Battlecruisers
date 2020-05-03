using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class MistController : MonoBehaviour
    {
        public void Initialse(Color mistColour)
        {
            SetColour(mistColour);
            RandomiseAnimationStartingPosition();
        }

        private void SetColour(Color mistColour)
        {
            SpriteRenderer[] mistSprites = GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer mistSprite in mistSprites)
            {
                mistSprite.color = mistColour;
            }
        }

        private void RandomiseAnimationStartingPosition()
        {
            Animator[] animations = GetComponentsInChildren<Animator>();

            foreach (Animator animation in animations)
            {
                animation.Play("MistRotor", layer: -1, normalizedTime: RandomGenerator.Instance.Value);
            }
        }
    }
}