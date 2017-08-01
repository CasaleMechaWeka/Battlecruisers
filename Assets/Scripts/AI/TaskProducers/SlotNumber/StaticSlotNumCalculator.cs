using System.Collections.Generic;
using BattleCruisers.AI.ThreatMonitors;

namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
    public class StaticSlotNumCalculator : SlotNumCalculator
    {
        private readonly IDictionary<ThreatLevel, int> _threatLevelsToSlotNumbers;
        protected override IDictionary<ThreatLevel, int> ThreatLevelsToSlotNumbers { get { return _threatLevelsToSlotNumbers; } }

        public StaticSlotNumCalculator(int numOfSlots)
            : base(numOfSlots)
        {
            _threatLevelsToSlotNumbers = new Dictionary<ThreatLevel, int>()
            {
                { ThreatLevel.None, 0 },
                { ThreatLevel.Low, numOfSlots },
                { ThreatLevel.High, numOfSlots }
            };
        }
    }
}
