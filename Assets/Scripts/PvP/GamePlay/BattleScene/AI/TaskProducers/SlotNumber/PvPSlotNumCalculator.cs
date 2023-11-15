using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber
{
    public abstract class PvPSlotNumCalculator : IPvPSlotNumCalculator
    {
        private readonly int _roofSlotNum;
        private readonly IDictionary<PvPThreatLevel, int> _threatLevelsToSlotNumbers;

        protected PvPSlotNumCalculator(int roofSlotNum, int slotsForNoThreat, int slotsForLowThreat, int slotsForHighThreat)
        {
            _roofSlotNum = roofSlotNum;
            _threatLevelsToSlotNumbers = new Dictionary<PvPThreatLevel, int>()
            {
                { PvPThreatLevel.None, slotsForNoThreat },
                { PvPThreatLevel.Low, slotsForLowThreat },
                { PvPThreatLevel.High, slotsForHighThreat}
            };
        }

        public int FindSlotNum(PvPThreatLevel threatLevel)
        {
            Assert.IsTrue(_threatLevelsToSlotNumbers.ContainsKey(threatLevel));

            int numOfSlots = _threatLevelsToSlotNumbers[threatLevel];

            if (numOfSlots > _roofSlotNum)
            {
                numOfSlots = _roofSlotNum;
            }

            return numOfSlots;
        }
    }
}