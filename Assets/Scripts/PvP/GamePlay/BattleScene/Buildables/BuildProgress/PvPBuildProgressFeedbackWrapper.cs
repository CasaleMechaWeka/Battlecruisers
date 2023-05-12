using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public class PvPBuildProgressFeedbackWrapper : MonoBehaviour
    {
        public Image unitImage;
        public IPvPBuildProgressFeedback CreateFeedback()
        {
            Image fillableImage = GetComponent<Image>();
            Assert.IsNotNull(fillableImage);
            Assert.AreEqual(Image.Type.Filled, fillableImage.type);

            Image pausedFeedback = transform.FindNamedComponent<Image>("PausedFeedback");

            return
                new PvPBuildProgressFeedback(
                    new PvPFillableImage(fillableImage),
                    new PvPGameObjectBC(pausedFeedback.gameObject),
                    unitImage);
        }
    }
}