using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Data.Static
{
    public class LootTests
    {
        private IPrefabKey _key1, _key2, _key3;

        [SetUp]
        public void SetuUp()
        {
            _key1 = Substitute.For<IPrefabKey>();
            _key2 = Substitute.For<IPrefabKey>();
            _key3 = Substitute.For<IPrefabKey>();
        }

        [Test]
        public void Equality_True()
        {
            ILoot loot1 = new Loot(buildingKey: _key1, unitKey: _key2, hullKey: _key3);
            ILoot loot2 = new Loot(buildingKey: _key1, unitKey: _key2, hullKey: _key3);

            Assert.AreEqual(loot1, loot2);
        }

        [Test]
        public void Equality_True_AllKeysNull()
        {
            ILoot loot1 = new Loot();
            ILoot loot2 = new Loot();

            Assert.AreEqual(loot1, loot2);
        }

        [Test]
        public void Equality_False_DifferentBuildingKey()
        {
            ILoot loot1 = new Loot(buildingKey: _key2, unitKey: _key2, hullKey: _key3);
            ILoot loot2 = new Loot(buildingKey: _key1, unitKey: _key2, hullKey: _key3);

            Assert.AreNotEqual(loot1, loot2);
        }

        [Test]
        public void Equality_False_DifferentUnitKey()
        {
            ILoot loot1 = new Loot(buildingKey: _key1, unitKey: _key2, hullKey: _key3);
            ILoot loot2 = new Loot(buildingKey: _key1, unitKey: _key3, hullKey: _key3);

            Assert.AreNotEqual(loot1, loot2);
        }

        [Test]
        public void Equality_False_DifferentHullKey()
        {
            ILoot loot1 = new Loot(buildingKey: _key2, unitKey: _key2, hullKey: _key3);
            ILoot loot2 = new Loot(buildingKey: _key1, unitKey: _key2, hullKey: _key1);

            Assert.AreNotEqual(loot1, loot2);
        }
    }
}
