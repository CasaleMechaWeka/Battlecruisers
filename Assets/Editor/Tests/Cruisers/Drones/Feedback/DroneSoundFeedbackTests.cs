using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;
using UnityCommon.Properties;

namespace BattleCruisers.Tests.Cruisers.Drones.Feedback
{
    public class DroneSoundFeedbackTests
    {
        private IManagedDisposable _feedback;
        private IBroadcastingProperty<bool> _parentCruiserHasActiveDrones;
        private IAudioSource _audioSource;

        [SetUp]
        public void TestSetup()
        {
            _parentCruiserHasActiveDrones = Substitute.For<IBroadcastingProperty<bool>>();
            _audioSource = Substitute.For<IAudioSource>();
            _feedback = new DroneSoundFeedback(_parentCruiserHasActiveDrones, _audioSource);
        }

        [Test]
        public void _parentCruiserHasActiveDrones_ValueChanged_True()
        {
            _parentCruiserHasActiveDrones.Value.Returns(true);
            _parentCruiserHasActiveDrones.ValueChanged += Raise.Event();

            _audioSource.Received().Play(isSpatial: true, loop: true);
        }

        [Test]
        public void _parentCruiserHasActiveDrones_ValueChanged_False()
        {
            _parentCruiserHasActiveDrones.Value.Returns(false);
            _parentCruiserHasActiveDrones.ValueChanged += Raise.Event();

            _audioSource.Received().Stop();
        }

        [Test]
        public void DisposeManagedState()
        {
            _feedback.DisposeManagedState();

            _parentCruiserHasActiveDrones.Value.Returns(false);
            _parentCruiserHasActiveDrones.ValueChanged += Raise.Event();

            _audioSource.DidNotReceive().Stop();
        }
    }
}