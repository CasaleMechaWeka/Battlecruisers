using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.Buildables.Pools
{
    public class UnitToPoolMapTests
    {
        private IUnitToPoolMap _map;
        private IUnitPoolProvider _unitPoolProvider;
        private IUnit _invalid, _aircraft, _ship;

        [SetUp]
        public void TestSetup()
        {
            _unitPoolProvider = Substitute.For<IUnitPoolProvider>();
            _map = new UnitToPoolMap(_unitPoolProvider);

            _invalid = Substitute.For<IUnit>();
            _invalid.Category.Returns(UnitCategory.Untouchable);

            _aircraft = Substitute.For<IUnit>();
            _aircraft.Category.Returns(UnitCategory.Aircraft);

            _ship = Substitute.For<IUnit>();
            _ship.Category.Returns(UnitCategory.Naval);
        }

        [Test]
        public void GetPool_InvalidUnitCategory()
        {
            Assert.Throws<ArgumentException>(() => _map.GetPool(_invalid));
        }

        [Test]
        public void GetPool_InvalidAircraft()
        {
            _aircraft.PrefabName.Returns("Invalied aircraft name :P");
            Assert.Throws<ArgumentException>(() => _map.GetPool(_aircraft));
        }

        [Test]
        public void GetPool_Bomber()
        {
            _aircraft.PrefabName.Returns("Bomber");
            IPool<Unit, BuildableActivationArgs> pool = _map.GetPool(_aircraft);
            Assert.AreSame(_unitPoolProvider.BomberPool, pool);
        }

        [Test]
        public void GetPool_Fighter()
        {
            _aircraft.PrefabName.Returns("Fighter");
            IPool<Unit, BuildableActivationArgs> pool = _map.GetPool(_aircraft);
            Assert.AreSame(_unitPoolProvider.FighterPool, pool);
        }

        [Test]
        public void GetPool_Gunship()
        {
            _aircraft.PrefabName.Returns("Gunship");
            IPool<Unit, BuildableActivationArgs> pool = _map.GetPool(_aircraft);
            Assert.AreSame(_unitPoolProvider.GunshipPool, pool);
        }

        [Test]
        public void GetPool_TestAircraft()
        {
            _aircraft.PrefabName.Returns("TestAircraft");
            IPool<Unit, BuildableActivationArgs> pool = _map.GetPool(_aircraft);
            Assert.AreSame(_unitPoolProvider.TestAircraftPool, pool);
        }

        [Test]
        public void GetPool_InvalidShip()
        {
            _ship.PrefabName.Returns("Invalid ship name :/");
            Assert.Throws<ArgumentException>(() => _map.GetPool(_ship));
        }

        [Test]
        public void GetPool_AttackBoat()
        {
            _ship.PrefabName.Returns("AttackBoat");
            IPool<Unit, BuildableActivationArgs> pool = _map.GetPool(_ship);
            Assert.AreSame(_unitPoolProvider.AttackBoatPool, pool);
        }

        [Test]
        public void GetPool_Frigate()
        {
            _ship.PrefabName.Returns("Frigate");
            IPool<Unit, BuildableActivationArgs> pool = _map.GetPool(_ship);
            Assert.AreSame(_unitPoolProvider.FrigatePool, pool);
        }

        [Test]
        public void GetPool_Destroyer()
        {
            _ship.PrefabName.Returns("Destroyer");
            IPool<Unit, BuildableActivationArgs> pool = _map.GetPool(_ship);
            Assert.AreSame(_unitPoolProvider.DestroyerPool, pool);
        }

        [Test]
        public void GetPool_Archon()
        {
            _ship.PrefabName.Returns("ArchonBattleship");
            IPool<Unit, BuildableActivationArgs> pool = _map.GetPool(_ship);
            Assert.AreSame(_unitPoolProvider.ArchonPool, pool);
        }
    }
}