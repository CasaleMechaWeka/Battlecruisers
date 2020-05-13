using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class MistController : MonoBehaviour
    {
        public void Initialse(ICloudStats cloudStats)
        {
            Assert.IsNotNull(cloudStats);

            SetColour(cloudStats.MistColour);
            RandomiseAnimationStartingPosition();
            transform.position = new Vector3(transform.position.x, cloudStats.MistYPosition, cloudStats.MistZPosition);
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