using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots.Feedback
{
    public class SlotBoostFeedbackMonitorInitialiser : MonoBehaviour
    {
        public SlotBoostFeedbackMonitor CreateFeedbackMonitor(ISlot slot)
        {
            Assert.IsNotNull(slot);

            ParticleSystem singleBoostEffect = transform.FindNamedComponent<ParticleSystem>("SingleBoostEffect");
            ParticleSystem doubleBoostEffect = transform.FindNamedComponent<ParticleSystem>("DoubleBoostEffect");

            return
                new SlotBoostFeedbackMonitor(
                    slot,
                    new BoostStateFinder(),
                    new BoostFeedback(
                        new GameObjectBC(singleBoostEffect.gameObject),
                        new GameObjectBC(doubleBoostEffect.gameObject)));
        }
    }
}