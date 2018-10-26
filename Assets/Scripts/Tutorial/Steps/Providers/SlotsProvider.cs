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
        private readonly ISlotAccessor _slotAccessor;
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
                        ISlot frontMostSlot = _slotAccessor.GetFreeSlot(_slotSpecification);
                        Assert.IsNotNull(frontMostSlot);

                        _slots = new List<ISlot>()
                        {
                            frontMostSlot
                        };
                    }
                    else
                    {
                        _slots = _slotAccessor.GetFreeSlots(_slotSpecification.SlotType);
                    }
                }

                return _slots;
            }
        }

        public SlotsProvider(ISlotAccessor slotAccessor, SlotSpecification slotSpecification)
        {
            Helper.AssertIsNotNull(slotAccessor, slotSpecification);

            _slotAccessor = slotAccessor;
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
