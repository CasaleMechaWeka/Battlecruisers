using System.Collections.Generic;
using BattleCruisers.AI.ThreatMonitors;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
	public abstract class SlotNumCalculator : ISlotNumCalculator
	{
        private readonly int _roofSlotNum;

        protected abstract IDictionary<ThreatLevel, int> ThreatLevelsToSlotNumbers { get; }

        protected SlotNumCalculator(int roofSlotNum)
        {
            _roofSlotNum = roofSlotNum;
        }

		public int FindSlotNum(ThreatLevel threatLevel)
        {
            Assert.IsTrue(ThreatLevelsToSlotNumbers.ContainsKey(threatLevel));

            int numOfSlots = ThreatLevelsToSlotNumbers[threatLevel];

            if (numOfSlots > _roofSlotNum)
			{
                numOfSlots = _roofSlotNum;
			}

			return numOfSlots;
        }
	}
}