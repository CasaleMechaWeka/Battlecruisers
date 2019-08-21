using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public class AntiSeaBalancingTest : DefenceBuildingBalancingTest
    {
        protected override IFactory CreateFactory(ICruiser enemyCruiser)
        {
            NavalFactory factory = GetComponentInChildren<NavalFactory>();

            _helper
                .InitialiseBuilding(
                    factory, 
                    Faction.Blues, 
                    parentCruiserDirection: Direction.Right);

            return factory;
        }
    }
}
