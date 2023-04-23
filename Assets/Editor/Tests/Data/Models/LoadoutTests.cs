using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Data.Models
{
    public class LoadoutTests
    {
        private ILoadout _loadout;

        private HullKey _hullKey1, _hullKey2;
        private BuildingKey _offensiveBuildingKey, _defensiveBuildingKey, _ultraBuildingKey;
        private UnitKey _shipUnitKey1, _shipUnitKey2, _aircraftUnitKey;

        [SetUp]
        public void SetuUp()
        {
            _hullKey1 = new HullKey("Raptor");
            _hullKey2 = new HullKey("Longbow");

            _offensiveBuildingKey = new BuildingKey(BuildingCategory.Offence, "Railgun");
            _defensiveBuildingKey = new BuildingKey(BuildingCategory.Defence, "Sam Site");
            _ultraBuildingKey = new BuildingKey(BuildingCategory.Ultra, "Ultralisk");

            _shipUnitKey1 = new UnitKey(UnitCategory.Naval, "Archon");
            _shipUnitKey2 = new UnitKey(UnitCategory.Naval, "Frigate");
            _aircraftUnitKey = new UnitKey(UnitCategory.Aircraft, "Turtle Gunship");

            /*_loadout
                = new Loadout(
                    _hullKey1,
                    new List<BuildingKey>() { _offensiveBuildingKey, _defensiveBuildingKey },
                    new List<UnitKey>() { _shipUnitKey1, _aircraftUnitKey });*/
        }

        #region Hull
        [Test]
        public void Hull_InitialGet()
        {
            Assert.AreSame(_hullKey1, _loadout.Hull);
        }

        [Test]
        public void Hull_SetUpdatesGet()
        {
            _loadout.Hull = _hullKey2;
            Assert.AreSame(_hullKey2, _loadout.Hull);
        }
        #endregion Hull

        #region Buildings
        [Test]
        public void GetBuildings()
        {
            ExpectGetBuildings(BuildingCategory.Offence, _offensiveBuildingKey);
        }

        [Test]
        public void AddBuilding_Duplicate_DoesNothing()
        {
            Assert.AreEqual(1, _loadout.GetBuildings(BuildingCategory.Offence).Count);
            _loadout.AddBuilding(_offensiveBuildingKey);
            Assert.AreEqual(1, _loadout.GetBuildings(BuildingCategory.Offence).Count);
        }

        [Test]
        public void AddBuilding_Works()
        {
            _loadout.AddBuilding(_ultraBuildingKey);
            ExpectGetBuildings(BuildingCategory.Ultra, _ultraBuildingKey);
        }

        [Test]
        public void RemoveBuilding_NonExistant_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _loadout.RemoveBuilding(_ultraBuildingKey));
        }

        [Test]
        public void RemoveBuilding_Works()
        {
            _loadout.RemoveBuilding(_offensiveBuildingKey);
            IList<BuildingKey> offensives = _loadout.GetBuildings(BuildingCategory.Offence);
            Assert.AreEqual(0, offensives.Count);
        }
        #endregion Buildings

        #region Units
        [Test]
        public void GetUnits()
        {
            ExpectGetUnits(UnitCategory.Aircraft, _aircraftUnitKey);
        }

        [Test]
        public void AddUnit_Duplicate_DoesNothing()
        {
            Assert.AreEqual(1, _loadout.GetUnits(UnitCategory.Aircraft).Count);
            _loadout.AddUnit(_aircraftUnitKey);
            Assert.AreEqual(1, _loadout.GetUnits(UnitCategory.Aircraft).Count);
        }

        [Test]
        public void AddUnit_Works()
        {
            _loadout.AddUnit(_shipUnitKey2);
            ExpectGetUnits(UnitCategory.Naval, _shipUnitKey1, _shipUnitKey2);
        }

        [Test]
        public void RemoveUnit_NonExistant_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _loadout.RemoveUnit(_shipUnitKey2));
        }

        [Test]
        public void RemoveUnit_Works()
        {
            _loadout.RemoveUnit(_shipUnitKey1);
            IList<UnitKey> ships = _loadout.GetUnits(UnitCategory.Naval);
            Assert.AreEqual(0, ships.Count);
        }
        #endregion Units

        private void ExpectGetBuildings(BuildingCategory categoryToGet, BuildingKey expectedKey)
        {
            IList<BuildingKey> expectedBuildings = new List<BuildingKey>()
            {
                expectedKey
            };
            IList<BuildingKey> actualBuildings = _loadout.GetBuildings(categoryToGet);
            Assert.IsTrue(Enumerable.SequenceEqual(expectedBuildings, actualBuildings));
        }

        private void ExpectGetUnits(UnitCategory categoryToGet, params UnitKey[] expectedUnits)
        {
            IList<UnitKey> actualUnits = _loadout.GetUnits(categoryToGet);
            Assert.IsTrue(Enumerable.SequenceEqual(expectedUnits, actualUnits));
        }
    }
}
