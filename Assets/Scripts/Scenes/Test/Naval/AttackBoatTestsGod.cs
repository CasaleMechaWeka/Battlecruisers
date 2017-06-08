using BattleCruisers.Buildables;
using BattleCruisers.Scenes.Test.Factories;

namespace BattleCruisers.Scenes.Test.Naval
{
	public class AttackBoatTestsGod : FactoryTestGod
	{
		protected override Faction FactoryFacingLeftFaction { get { return Faction.Reds; } }
		protected override Faction FactoryFacingRightFaction { get { return Faction.Blues; } }
	}
}
