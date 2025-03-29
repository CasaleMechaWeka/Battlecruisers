using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public class PvPBuildProgressFeedbackWrapper : MonoBehaviour
    {
        public Image unitImage;
        public PvPBuildProgressFeedback CreateFeedback()
        {
            Image fillableImage = GetComponent<Image>();
            Assert.IsNotNull(fillableImage);
            Assert.AreEqual(Image.Type.Filled, fillableImage.type);

            Image pausedFeedback = transform.FindNamedComponent<Image>("PausedFeedback");

            return
                new PvPBuildProgressFeedback(
                    fillableImage,
                    new GameObjectBC(pausedFeedback.gameObject),
                    unitImage);
        }
    }
}