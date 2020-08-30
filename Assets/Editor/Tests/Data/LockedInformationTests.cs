using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Data
{
    public class LockedInformationTests
    {
        private ILockedInformation _lockedInfo;

        private IGameModel _gameModel;
        private IStaticData _staticData;
        private ReadOnlyCollection<ILevel> _levels;

        [SetUp]
        public void SetuUp()
        {
            _gameModel = Substitute.For<IGameModel>();
            _staticData = Substitute.For<IStaticData>();

            IList<ILevel> levels = new List<ILevel>();
            for (int i = 0; i < LockedInformation.NUM_OF_LEVELS_IN_DEMO + 1; ++i)
            {
                levels.Add(Substitute.For<ILevel>());
            };
            _levels = new ReadOnlyCollection<ILevel>(levels);
            _staticData.Levels.Returns(_levels);
            _staticData.IsDemo.Returns(false);

            _lockedInfo = new LockedInformation(_gameModel, _staticData);
        }

        #region NumOfLevelsUnlocked
        [Test]
        public void NumOfLevelsUnlocked_FromNumOfLevelsCompleted()
        {
            _gameModel.NumOfLevelsCompleted.Returns(0);
            Assert.AreEqual(1, _lockedInfo.NumOfLevelsUnlocked);
        }

        [Test]
        public void NumOfLevelsUnlocked_WhenAllLevelsAreCompleted()
        {
            _gameModel.NumOfLevelsCompleted.Returns(_levels.Count);
            Assert.AreEqual(_levels.Count, _lockedInfo.NumOfLevelsUnlocked);
        }

        [Test]
        public void NumOfLevelsUnlocked_IsDemo()
        {
            _gameModel.NumOfLevelsCompleted.Returns(_levels.Count);
            _staticData.IsDemo.Returns(true);
            Assert.AreEqual(LockedInformation.NUM_OF_LEVELS_IN_DEMO, _lockedInfo.NumOfLevelsUnlocked);
        }
        #endregion NumOfLevelsUnlocked

        [Test]
        public void NumOfLockedHulls()
        {
            ReadOnlyCollection<HullKey> allHulls = new ReadOnlyCollection<HullKey>(new List<HullKey>()
            {
                new HullKey("Trident"),
                new HullKey("Raptor"),
                new HullKey("Longbow")
            });
            _staticData.HullKeys.Returns(allHulls);

            ReadOnlyCollection<HullKey> unlockedHulls = new ReadOnlyCollection<HullKey>(new List<HullKey>()
            {
                new HullKey("Trident")
            });
            _gameModel.UnlockedHulls.Returns(unlockedHulls);

            Assert.AreEqual(2, _lockedInfo.NumOfLockedHulls);
        }

        [Test]
        public void NumOfLockedBuildings()
        {
            ReadOnlyCollection<BuildingKey> allBuildings = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
            {
                new BuildingKey(BuildingCategory.Defence, "PeaShooter"),
                new BuildingKey(BuildingCategory.Defence, "Slingshot"),
                new BuildingKey(BuildingCategory.Ultra, "Kaboozka"),
                new BuildingKey(BuildingCategory.Tactical, "Gods Eye"),
                new BuildingKey(BuildingCategory.Offence, "Catapult")
            });
            _staticData.BuildingKeys.Returns(allBuildings);

            IList<BuildingKey> unlockedBuildings = Substitute.For<IList<BuildingKey>>();
            unlockedBuildings.Count.Returns(1);

            _gameModel.GetUnlockedBuildings(BuildingCategory.Defence).Returns(unlockedBuildings);

            Assert.AreEqual(1, _lockedInfo.NumOfLockedBuildings(BuildingCategory.Defence));
        }

        [Test]
        public void NumOfLockedUnits()
        {
            ReadOnlyCollection<UnitKey> allUnits = new ReadOnlyCollection<UnitKey>(new List<UnitKey>()
            {
                new UnitKey(UnitCategory.Aircraft, "Zeppelin"),
                new UnitKey(UnitCategory.Aircraft, "Messerschmitt"),
                new UnitKey(UnitCategory.Aircraft, "Schmetterling"),
                new UnitKey(UnitCategory.Naval, "Rubber Ducky"),
                new UnitKey(UnitCategory.Untouchable, "Sun")
            });
            _staticData.UnitKeys.Returns(allUnits);

            IList<UnitKey> unlockedUnits = Substitute.For<IList<UnitKey>>();
            unlockedUnits.Count.Returns(1);

            _gameModel.GetUnlockedUnits(UnitCategory.Aircraft).Returns(unlockedUnits);

            Assert.AreEqual(2, _lockedInfo.NumOfLockedUnits(UnitCategory.Aircraft));
        }
    }
}
