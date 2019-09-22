using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones.Feedback;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Drones.Feedback
{
    public class DroneAudioActivenessDeciderTests
    {
        private IDroneAudioActivenessDecider _droneAudioActivenessDecider;
        private IDictionary<Faction, int> _factionToActiveDroneNum;
        private int _maxActiveDroneAudioSources;

        [SetUp]
        public void TestSetup()
        {
            _factionToActiveDroneNum = new Dictionary<Faction, int>();
            IReadOnlyDictionary<Faction, int> readonlyDictionary = new ReadOnlyDictionary<Faction, int>(_factionToActiveDroneNum);

            _maxActiveDroneAudioSources = 3;

            _droneAudioActivenessDecider = new DroneAudioActivenessDecider(readonlyDictionary, _maxActiveDroneAudioSources);

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void ShouldHaveAudio_NoKey_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _droneAudioActivenessDecider.ShouldHaveAudio(Faction.Reds));
        }

        [Test]
        public void ShouldHaveAudio_True()
        {
            _factionToActiveDroneNum.Add(Faction.Reds, _maxActiveDroneAudioSources - 1);
            Assert.IsTrue(_droneAudioActivenessDecider.ShouldHaveAudio(Faction.Reds));
        }

        [Test]
        public void ShouldHaveAudio_False()
        {
            _factionToActiveDroneNum.Add(Faction.Reds, _maxActiveDroneAudioSources);
            Assert.IsFalse(_droneAudioActivenessDecider.ShouldHaveAudio(Faction.Reds));
        }
    }
}