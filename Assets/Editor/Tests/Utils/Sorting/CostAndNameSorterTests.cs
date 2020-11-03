using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Utils.Sorting;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.Utils.Sorting
{
    public class CostAndNameSorterTests
    {
        private IBuildableSorter<IBuilding> _sorter;

        private IBuildableWrapper<IBuilding> _building1, _building2, _building3, _building4;

        [SetUp]
        public void SetuUp()
        {
            _sorter = new CostAndNameSorter<IBuilding>();

            _building1 = CreateBuilding(numOfDronesRequired: 2, buildTimeInS: 20, name: "Archon");
            _building2 = CreateBuilding(numOfDronesRequired: 2, buildTimeInS: 30, name: "Archon");
            _building3 = CreateBuilding(numOfDronesRequired: 2, buildTimeInS: 20, name: "Turtle");
            _building4 = CreateBuilding(numOfDronesRequired: 4, buildTimeInS: 10, name: "Zilch");
        }

        [Test]
        public void Sort()
        {
            IList<IBuildableWrapper<IBuilding>> expected = new List<IBuildableWrapper<IBuilding>>()
            {
                _building1, _building3, _building2, _building4
            };

            IList<IBuildableWrapper<IBuilding>> input = new List<IBuildableWrapper<IBuilding>>()
            {
                _building3, _building4, _building1, _building2
            };

            IList<IBuildableWrapper<IBuilding>> actual = _sorter.Sort(input);

            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; ++i)
            {
                IBuilding expectedBuilding = expected[i].Buildable;
                IBuilding actualBuilding = actual[i].Buildable;
                Assert.AreSame(expectedBuilding, actualBuilding);
            }
        }

        private IBuildableWrapper<IBuilding> CreateBuilding(int numOfDronesRequired, float buildTimeInS, string name)
        {
            IBuilding building = Substitute.For<IBuilding>();
            building.NumOfDronesRequired.Returns(numOfDronesRequired);
            building.BuildTimeInS.Returns(buildTimeInS);
            building.Name.Returns(name);

            IBuildableWrapper<IBuilding> wrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            wrapper.Buildable.Returns(building);

            return wrapper;
        }
    }
}
