using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetDetectors;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetDetectors
{
    public class TargetColliderHandlerTests
    {
        private ITargetColliderHandler _colliderHandler;
        private ITargetDetectorEventEmitter _eventEmitter;
        private ITarget _target;

        [SetUp]
        public void TestSetup()
        {
            _eventEmitter = Substitute.For<ITargetDetectorEventEmitter>();
            _colliderHandler = new TargetColliderHandler(_eventEmitter);
            _target = Substitute.For<ITarget>();
        }

        [Test]
        public void OnTargetColliderEntered_InvokesEnteredEvent()
        {
            _colliderHandler.OnTargetColliderEntered(_target);
            _eventEmitter.Received().InvokeTargetEnteredEvent(_target);
        }

        [Test]
        public void OnTargetColliderExited_BeforeTargetDestroyed_InvokesExitedEvent()
        {
            _target.IsDestroyed.Returns(false);
            _colliderHandler.OnTargetColliderExited(_target);
            _eventEmitter.Received().InvokeTargetExitedEvent(_target);
        }

        [Test]
        public void OnTargetColliderExited_AfterTargetDestroyed_DoesNotInvokeExitedEvent()
        {
            _target.IsDestroyed.Returns(true);
            _colliderHandler.OnTargetColliderExited(_target);
            _eventEmitter.DidNotReceiveWithAnyArgs().InvokeTargetExitedEvent(null);
        }

        [Test]
        public void TargetDestroyed_BeforeColliderExited_InvokesExitedEvent()
        {
            EnterTarget();

            _target.Destroyed += Raise.EventWith(new DestroyedEventArgs(_target));
            _eventEmitter.Received().InvokeTargetExitedEvent(_target);
        }

        [Test]
        public void TargetDestroyed_AfterColliderExited_DoesNotInvokeExitedEvent()
        {
            // Target entered
            EnterTarget();

            // Target exited
            _colliderHandler.OnTargetColliderExited(_target);

            // Target destroyed
            _eventEmitter.ClearReceivedCalls();
            _target.Destroyed += Raise.EventWith(new DestroyedEventArgs(_target));

            _eventEmitter.DidNotReceiveWithAnyArgs().InvokeTargetExitedEvent(null);
        }

        private void EnterTarget()
        {
            OnTargetColliderEntered_InvokesEnteredEvent();
        }
    }
}