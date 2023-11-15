using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds
{
    public class PvPMistController : MonoBehaviour
    {
        public void Initialise(IPvPCloudStats cloudStats)
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
                animation.Play("MistRotor", layer: -1, normalizedTime: PvPRandomGenerator.Instance.Value);
            }
        }
    }
}