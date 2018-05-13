using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class SlotsProvider : ISlotsProvider
    {
        private readonly ISlotWrapper _slotWrapper;
        private readonly SlotType _slotType;
        private readonly bool _preferFrontmostSlot;

        private IList<ISlot> _slots;

        public SlotsProvider(ISlotWrapper slotWrapper, SlotType slotType, bool preferFrontmostSlot)
        {
            Assert.IsNotNull(slotWrapper);

            _slotWrapper = slotWrapper;
            _slotType = slotType;
            _preferFrontmostSlot = preferFrontmostSlot;
        }

        IList<ISlot> IListProvider<ISlot>.FindItems()
        {
            if (_slots == null)
            {
                if (_preferFrontmostSlot)
                {
                    ISlot frontMostSlot = _slotWrapper.GetFreeSlot(_slotType, _preferFrontmostSlot);
                    Assert.IsNotNull(frontMostSlot);

                    _slots = new List<ISlot>()
                    {
                        frontMostSlot
                    };
                }
                else
                {
                    _slots = _slotWrapper.GetSlotsForType(_slotType);
                }
            }

            return _slots;
        }

        public IList<IHighlightable> FindItems()
        {
            return _slots.Cast<IHighlightable>().ToList();
        }

        IList<IClickableEmitter> IListProvider<IClickableEmitter>.FindItems()
        {
            return _slots.Cast<IClickableEmitter>().ToList();
        }
    }
}
