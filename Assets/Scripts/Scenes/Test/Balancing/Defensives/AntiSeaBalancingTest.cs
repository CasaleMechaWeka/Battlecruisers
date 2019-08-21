using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public class AntiSeaBalancingTest : DefenceBuildingBalancingTest
    {
        protected override IFactory CreateFactory(ICruiser parentCruiser, ICruiser enemyCruiser)
        {
            NavalFactory factory = GetComponentInChildren<NavalFactory>();

            _helper
                .InitialiseBuilding(
                    factory, 
                    Faction.Blues, 
                    parentCruiserDirection: Direction.Right,
                    parentCruiser: parentCruiser);

            Helper.SetupFactoryForUnitMonitor(factory, parentCruiser);

            return factory;
        }
    }
}
