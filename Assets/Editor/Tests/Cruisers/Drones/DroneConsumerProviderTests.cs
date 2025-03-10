using BattleCruisers.Cruisers.Drones;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Drones
{
    public class DroneConsumerProviderTests
    {
        private IDroneConsumerProvider _droneConsumerProvider;

        private IDroneManager _droneManager;
        private IDroneConsumer _droneConsumer;

        [SetUp]
        public void SetuUp()
        {
            _droneManager = Substitute.For<IDroneManager>();
            _droneConsumer = Substitute.For<IDroneConsumer>();

            _droneConsumerProvider = new DroneConsumerProvider(_droneManager);
        }

        #region ActivateDroneConsumer
        [Test]
        public void ActivateDroneConsumer_AddsDroneConsumer()
        {
            _droneManager.CanSupportDroneConsumer(_droneConsumer.NumOfDrones).Returns(true);
            _droneManager.HasDroneConsumer(_droneConsumer).Returns(false);

            _droneConsumerProvider.ActivateDroneConsumer(_droneConsumer);

            _droneManager.Received().AddDroneConsumer(_droneConsumer);
        }

        [Test]
        public void ActivateDroneConsumer_CannotSupportConsumer_Throws()
        {
            _droneManager.CanSupportDroneConsumer(_droneConsumer.NumOfDrones).Returns(false);
            Assert.Throws<UnityAsserts.AssertionException>(() => _droneConsumerProvider.ActivateDroneConsumer(_droneConsumer));
        }

        [Test]
        public void ActivateDroneConsumer_AddExistingConsumer_Throws()
        {
            _droneManager.CanSupportDroneConsumer(_droneConsumer.NumOfDrones).Returns(true);
            _droneManager.HasDroneConsumer(_droneConsumer).Returns(true);

            Assert.Throws<UnityAsserts.AssertionException>(() => _droneConsumerProvider.ActivateDroneConsumer(_droneConsumer));
        }
        #endregion ActivateDroneConsumer

        #region ReleaseDroneConsumer
        [Test]
        public void ReleaseDroneConsumer_ManagerHasConsumer_RemovesConsumer()
        {
            _droneManager.HasDroneConsumer(_droneConsumer).Returns(true);
            _droneConsumerProvider.ReleaseDroneConsumer(_droneConsumer);
            _droneManager.Received().RemoveDroneConsumer(_droneConsumer);
        }

        [Test]
        public void ReleaseDroneConsumer_ManagerDoesNotHaveConsumer_DoesNothing()
        {
            _droneManager.HasDroneConsumer(_droneConsumer).Returns(false);
            _droneConsumerProvider.ReleaseDroneConsumer(_droneConsumer);
            _droneManager.DidNotReceive().RemoveDroneConsumer(_droneConsumer);
        }
        #endregion ReleaseDroneConsumer
    }
}
