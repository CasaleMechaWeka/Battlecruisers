using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class SlotsProvider : ISlotsProvider
    {
        private readonly ISlotWrapper _slotWrapper;
        private readonly SlotSpecification _slotSpecification;

        private IList<ISlot> _slots;
        private IList<ISlot> Slots
        {
            get
            {
                if (_slots == null)
                {
                    if (_slotSpecification.PreferFromFront)
                    {
                        ISlot frontMostSlot = _slotWrapper.GetFreeSlot(_slotSpecification);
                        Assert.IsNotNull(frontMostSlot);

                        _slots = new List<ISlot>()
                        {
                            frontMostSlot
                        };
                    }
                    else
                    {
                        _slots = _slotWrapper.GetFreeSlots(_slotSpecification.SlotType);
                    }
                }

                return _slots;
            }
        }

        public SlotsProvider(ISlotWrapper slotWrapper, SlotSpecification slotSpecification)
        {
            Helper.AssertIsNotNull(slotWrapper, slotSpecification);

            _slotWrapper = slotWrapper;
            _slotSpecification = slotSpecification;
        }

        IList<ISlot> IListProvider<ISlot>.FindItems()
        {
            return Slots;
        }

        public IList<IHighlightable> FindItems()
        {
            return Slots.Cast<IHighlightable>().ToList();
        }

        IList<IClickableEmitter> IListProvider<IClickableEmitter>.FindItems()
        {
            return Slots.Cast<IClickableEmitter>().ToList();
        }
    }
}
