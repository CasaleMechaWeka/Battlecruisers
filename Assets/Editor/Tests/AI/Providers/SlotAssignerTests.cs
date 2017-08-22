using BattleCruisers.AI.Providers.BuildingKey;
using BattleCruisers.AI.Providers.Strategies.Requests;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Providers
{
    public class SlotAssignerTests
    {
        private ISlotAssigner _slotAssigner;
        private IOffensiveRequest _lowAirRequest, _highAirRequest, _lowOffensiveRequest, _highUltrasRequest;
        private int _numOfSlotsAvailable;

        [SetUp]
        public void SetuUp()
        {
            _slotAssigner = new SlotAssigner();

            // Air
            _lowAirRequest = new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low);
            _highAirRequest = new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High);

            // Offensive
            _lowOffensiveRequest = new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low);

            // Ultras
            _highUltrasRequest = new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High);
        }

        #region High_Low
        [Test]
        public void High_Low_1Slot()
        {
            _numOfSlotsAvailable = 1;
            _slotAssigner.AssignSlots(new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest }, _numOfSlotsAvailable);

			Assert.AreEqual(1, _highAirRequest.NumOfSlotsToUse);
            Assert.AreEqual(0, _lowOffensiveRequest.NumOfSlotsToUse);
        }

        [Test]
        public void High_Low_2Slots()
        {
            _numOfSlotsAvailable = 2;
            _slotAssigner.AssignSlots(new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest }, _numOfSlotsAvailable);

			Assert.AreEqual(1, _highAirRequest.NumOfSlotsToUse);
			Assert.AreEqual(1, _lowOffensiveRequest.NumOfSlotsToUse);
        }

        [Test]
        public void High_Low_3Slots()
        {
            _numOfSlotsAvailable = 3;
            _slotAssigner.AssignSlots(new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest }, _numOfSlotsAvailable);

			Assert.AreEqual(2, _highAirRequest.NumOfSlotsToUse);
            Assert.AreEqual(1, _lowOffensiveRequest.NumOfSlotsToUse);
        }
    
        [Test]
        public void High_Low_4Slots()
        {
			_numOfSlotsAvailable = 4;
			_slotAssigner.AssignSlots(new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest }, _numOfSlotsAvailable);

			Assert.AreEqual(3, _highAirRequest.NumOfSlotsToUse);
			Assert.AreEqual(1, _lowOffensiveRequest.NumOfSlotsToUse);
        }
        #endregion High_Low

        [Test]
        public void High_Low_Low_4Slots()
        {
            _numOfSlotsAvailable = 4;
            _slotAssigner.AssignSlots(new IOffensiveRequest[] { _lowAirRequest, _lowOffensiveRequest, _highUltrasRequest }, _numOfSlotsAvailable);

            Assert.AreEqual(1, _lowAirRequest.NumOfSlotsToUse);
            Assert.AreEqual(1, _lowOffensiveRequest.NumOfSlotsToUse);
			Assert.AreEqual(2, _highUltrasRequest.NumOfSlotsToUse);
        }

        #region High_High_Low
        [Test]
        public void High_High_Low_2Slots()
        {
            _numOfSlotsAvailable = 2;
            _slotAssigner.AssignSlots(new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest, _highUltrasRequest }, _numOfSlotsAvailable);

            Assert.AreEqual(1, _highAirRequest.NumOfSlotsToUse);
            Assert.AreEqual(0, _lowOffensiveRequest.NumOfSlotsToUse);
			Assert.AreEqual(1, _highUltrasRequest.NumOfSlotsToUse);
        }

        [Test]
        public void High_High_Low_4Slots()
        {
            _numOfSlotsAvailable = 4;
            _slotAssigner.AssignSlots(new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest, _highUltrasRequest }, _numOfSlotsAvailable);

            Assert.AreEqual(2, _highAirRequest.NumOfSlotsToUse);
            Assert.AreEqual(1, _lowOffensiveRequest.NumOfSlotsToUse);
			Assert.AreEqual(1, _highUltrasRequest.NumOfSlotsToUse);
        }

        [Test]
        public void High_High_Low_5Slots()
        {
            _numOfSlotsAvailable = 5;
            _slotAssigner.AssignSlots(new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest, _highUltrasRequest }, _numOfSlotsAvailable);

			Assert.AreEqual(2, _highAirRequest.NumOfSlotsToUse);
			Assert.AreEqual(1, _lowOffensiveRequest.NumOfSlotsToUse);
			Assert.AreEqual(2, _highUltrasRequest.NumOfSlotsToUse);
        }
		#endregion High_High_Low

		[Test]
        public void High_High_1Slots()
        {
            _numOfSlotsAvailable = 1;
            _slotAssigner.AssignSlots(new IOffensiveRequest[] { _highUltrasRequest, _highAirRequest }, _numOfSlotsAvailable);

			Assert.AreEqual(1, _highUltrasRequest.NumOfSlotsToUse);
            Assert.AreEqual(0, _highAirRequest.NumOfSlotsToUse);
        }

        [Test]
        public void LeftOverSlots()
        {
            _numOfSlotsAvailable = 2;
            _slotAssigner.AssignSlots(new IOffensiveRequest[] { _lowAirRequest }, _numOfSlotsAvailable);

            Assert.AreEqual(1, _lowAirRequest.NumOfSlotsToUse);
        }
    }
}
