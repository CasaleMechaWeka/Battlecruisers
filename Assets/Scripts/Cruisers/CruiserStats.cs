namespace BattleCruisers.Cruisers
{
	public class CruiserStats : ICruiserStats
	{
		public int Health { get; private set; }
		public int NumOfDrones { get; private set; }
		public int NumOfPlatformSlots { get; private set; }
		public int NumOfDeckSlots { get; private set; }
		public int NumOfUtilitySlots { get; private set; }
		public int NumOfMastSlots { get; private set; }

		public CruiserStats(
			int health,
			int numOfDrones,
			int numOfPlatformSlots,
			int numOfDeckSlots,
			int numOfUtilitySlots,
			int numOfMastSlots)
		{
            Health = health;
            NumOfDrones = numOfDrones;
            NumOfPlatformSlots = numOfPlatformSlots;
            NumOfDeckSlots = numOfDeckSlots;
            NumOfUtilitySlots = numOfUtilitySlots;
            NumOfMastSlots = numOfMastSlots;
		}
	}
}
