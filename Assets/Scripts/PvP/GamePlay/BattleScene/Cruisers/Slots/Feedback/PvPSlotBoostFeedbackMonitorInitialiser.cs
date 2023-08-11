using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.Feedback
{
    public class PvPSlotBoostFeedbackMonitorInitialiser : MonoBehaviour
    {
        public PvPSlotBoostFeedbackMonitor CreateFeedbackMonitor(PvPSlot slot, bool isClient)
        {
            Assert.IsNotNull(slot);

            Transform singleBoostEffect = transform.FindNamedComponent<Transform>("SingleBoostEffect");
            Transform doubleBoostEffect = transform.FindNamedComponent<Transform>("DoubleBoostEffect");

            return
                new PvPSlotBoostFeedbackMonitor(
                    slot,
                    new PvPBoostStateFinder(),
                    new PvPBoostFeedback(
                        new PvPGameObjectBC(singleBoostEffect.gameObject),
                        new PvPGameObjectBC(doubleBoostEffect.gameObject)),
                    isClient);
        }
    }
}