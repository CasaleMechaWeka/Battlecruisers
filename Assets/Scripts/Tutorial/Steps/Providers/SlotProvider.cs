using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class SlotProvider : ISlotProvider
    {
        private readonly ISlotAccessor _slotAccessor;
        private readonly SlotSpecification _slotSpecification;

        private ISlot _slot;
        private ISlot Slot
        {
            get
            {
                if (_slot == null)
                {
                    _slot = _slotAccessor.GetFreeSlot(_slotSpecification);
                    Assert.IsNotNull(_slot);
                }

                return _slot;
            }
        }

        public SlotProvider(ISlotAccessor slotAccessor, SlotSpecification slotSpecification)
        {
            Helper.AssertIsNotNull(slotAccessor, slotSpecification);

            _slotAccessor = slotAccessor;
            _slotSpecification = slotSpecification;
        }

        ISlot IItemProvider<ISlot>.FindItem()
        {
            return _slot;
        }

        // TUTORIAL  Remove?
        public IHighlightable FindItem()
        {
            return _slot;
        }

        IClickableEmitter IItemProvider<IClickableEmitter>.FindItem()
        {
            return _slot;
        }
    }
}
