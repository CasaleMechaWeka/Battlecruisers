using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots.Feedback
{
            // FELIX  Remove
    public class SlotBoostFeedbackInitialiser : MonoBehaviour
    {
        public SlotBoostTextFeedback CreateSlotBoostFeedback(ISlot parentSlot)
        {
            Assert.IsNotNull(parentSlot);

            TextMesh textMesh = GetComponent<TextMesh>();
            Assert.IsNotNull(textMesh);
            ITextMesh textMeshWrapper = new TextMeshWrapper(textMesh);

            return new SlotBoostTextFeedback(textMeshWrapper, parentSlot.BoostProviders);
        }
    }
}