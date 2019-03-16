using BattleCruisers.Buildables;
using BattleCruisers.Scenes.Test.Factories;

namespace BattleCruisers.Scenes.Test.Naval
{
	public class ShipTestsGod : FactoryTestGod
	{
		protected override Faction FactoryFacingLeftFaction => Faction.Reds;
		protected override Faction FactoryFacingRightFaction => Faction.Blues;
	}
}
