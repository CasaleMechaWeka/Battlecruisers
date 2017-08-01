using System.Collections.Generic;
using BattleCruisers.AI.ThreatMonitors;

namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
	public class AntiNavalSlotNumCalculator : SlotNumCalculator
	{
		private readonly IDictionary<ThreatLevel, int> _threatLevelsToSlotNumbers;
		protected override IDictionary<ThreatLevel, int> ThreatLevelsToSlotNumbers { get { return _threatLevelsToSlotNumbers; } }

		public AntiNavalSlotNumCalculator(int roofSlotNum)
			: base(roofSlotNum)
		{
			_threatLevelsToSlotNumbers = new Dictionary<ThreatLevel, int>()
			{
				{ ThreatLevel.None, 0 },
				{ ThreatLevel.Low, 2},
				{ ThreatLevel.High, 4 }
			};
		}
	}
}
