using BattleCruisers.Cruisers.Slots;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Slots
{
    public class SpecificSlotFilterTests
    {
        private SpecificSlotsFilter _filter;
        private ISlot _slot1, _slot2;

        [SetUp]
        public void SetuUp()
        {
            _filter = new SpecificSlotsFilter();

            _slot1 = Substitute.For<ISlot>();
            _slot2 = Substitute.For<ISlot>();
        }

        [Test]
        public void IsMatch_False_BecauseNonePermitted()
        {
            Assert.IsFalse(_filter.IsMatch(_slot1));
        }

        [Test]
        public void IsMatch_False_BecauseDifferentSlotPermitted()
        {
            _filter.PermittedSlot = _slot2;
            Assert.IsFalse(_filter.IsMatch(_slot1));
        }
		
		[Test]
		public void IsMatch_True()
		{
            _filter.PermittedSlot = _slot1;
			Assert.IsTrue(_filter.IsMatch(_slot1));
		}
    }
}
