using System.Collections.Generic;
using BattleCruisers.AI.ThreatMonitors;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
	public abstract class SlotNumCalculator : ISlotNumCalculator
	{
        private readonly int _roofSlotNum;
		private readonly IDictionary<ThreatLevel, int> _threatLevelsToSlotNumbers;

        protected SlotNumCalculator(int roofSlotNum, int slotsForNoThreat, int slotsForLowThreat, int slotsForHighThreat)
        {
            _roofSlotNum = roofSlotNum;
            _threatLevelsToSlotNumbers = new Dictionary<ThreatLevel, int>()
            {
				{ ThreatLevel.None, slotsForNoThreat },
                { ThreatLevel.Low, slotsForLowThreat },
                { ThreatLevel.High, slotsForHighThreat}
            };
        }

		public int FindSlotNum(ThreatLevel threatLevel)
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