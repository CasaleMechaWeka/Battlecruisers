using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Buildables.BuildProgress
{
    public class BuildProgressFeedbackWrapper : MonoBehaviour
    {
        public IBuildProgressFeedback CreateFeedback()
        {
            Image fillableImage = GetComponent<Image>();
            Assert.IsNotNull(fillableImage);
            Assert.AreEqual(Image.Type.Filled, fillableImage.type);

            GameObject pausedFeedback = transform.FindNamedComponent<GameObject>("PausedFeedback");
            Assert.IsNotNull(pausedFeedback);

            return 
                new BuildProgressFeedback(
                    new FillableImage(fillableImage),
                    new GameObjectBC(pausedFeedback));
        }
    }
}