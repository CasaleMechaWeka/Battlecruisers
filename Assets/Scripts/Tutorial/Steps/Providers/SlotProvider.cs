using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class SlotProvider :
        IItemProvider<Slot>,
        IItemProvider<IHighlightable>,
        IItemProvider<IClickableEmitter>
    {
        private readonly SlotAccessor _slotAccessor;
        private readonly ISlotSpecification _slotSpecification;

        private Slot _slot;
        private Slot Slot
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

        public SlotProvider(SlotAccessor slotAccessor, ISlotSpecification slotSpecification)
        {
            Helper.AssertIsNotNull(slotAccessor, slotSpecification);

            _slotAccessor = slotAccessor;
            _slotSpecification = slotSpecification;
        }

        IClickableEmitter IItemProvider<IClickableEmitter>.FindItem()
        {
            return Slot;
        }

        IHighlightable IItemProvider<IHighlightable>.FindItem()
        {
            return Slot;
        }

        public Slot FindItem()
        {
            return Slot;
        }
    }
}
