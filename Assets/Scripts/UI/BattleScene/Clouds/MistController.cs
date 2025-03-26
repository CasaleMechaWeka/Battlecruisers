using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class MistController : MonoBehaviour
    {
        private const float NarrowAspectRatioThreshold = 1.0f; // Threshold for aspect ratio narrower than 1:1
        private const float NarrowMistYPosition = -120.0f; // Set this to whatever fixed Y position you want for narrow aspect ratios

        public void Initialise(ICloudStats cloudStats)
        {
            Assert.IsNotNull(cloudStats);

            SetColour(cloudStats.MistColour);
            RandomiseAnimationStartingPosition();

            // Determine the appropriate Y position based on the aspect ratio
            float currentAspectRatio = Camera.main.aspect;
            float mistYPosition = currentAspectRatio < NarrowAspectRatioThreshold ? NarrowMistYPosition : cloudStats.MistYPosition;

            transform.position = new Vector3(transform.position.x, mistYPosition, cloudStats.MistZPosition);
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
                animation.Play("MistRotor", layer: -1, normalizedTime: RandomGenerator.Value);
            }
        }
    }
}
