using System.Collections.Generic;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class SlotProvider : ISlotProvider
    {
        private readonly ISlotAccessor _slotAccessor;
        private readonly SlotSpecification _slotSpecification;
        private IList<ISlot> _slots;

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

        IClickableEmitter IItemProvider<IClickableEmitter>.FindItem()
        {
            return Slot;
        }

        IMaskHighlightable IItemProvider<IMaskHighlightable>.FindItem()
        {
            return Slot;
        }

        public IList<ISlot> FindItems()
        {
            if (_slots == null)
            {
                _slots = new List<ISlot>() { Slot };
            }
            return _slots;
        }
    }
}
