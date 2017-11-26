using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class AntiSeaBalancingTest : DefenceBuildingBalancingTest
    {
        protected override IFactory CreateFactory(IList<ITarget> defenceBuildings)
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
