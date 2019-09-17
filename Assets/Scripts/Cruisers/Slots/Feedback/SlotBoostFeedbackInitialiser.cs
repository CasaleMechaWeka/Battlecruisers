using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots.Feedback
{
    public class SlotBoostFeedbackInitialiser : MonoBehaviour
    {
        public SlotBoostFeedback CreateSlotBoostFeedback(ISlot parentSlot)
        {
            Assert.IsNotNull(parentSlot);

            TextMesh textMesh = GetComponent<TextMesh>();
            Assert.IsNotNull(textMesh);
            ITextMesh textMeshWrapper = new TextMeshWrapper(textMesh);

            return new SlotBoostFeedback(textMeshWrapper, parentSlot.BoostProviders);
        }
    }
}