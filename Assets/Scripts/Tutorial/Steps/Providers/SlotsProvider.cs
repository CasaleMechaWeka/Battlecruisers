using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
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
        private readonly BuildingFunction _buildingFunction;
        private readonly bool _preferFrontmostSlot;

        private IList<ISlot> _slots;
        private IList<ISlot> Slots
        {
            get
            {
                if (_slots == null)
                {
                    if (_preferFrontmostSlot)
                    {
                        ISlot frontMostSlot = _slotWrapper.GetFreeSlot(_slotType, _buildingFunction, _preferFrontmostSlot);
                        Assert.IsNotNull(frontMostSlot);

                        _slots = new List<ISlot>()
                        {
                            frontMostSlot
                        };
                    }
                    else
                    {
                        _slots = _slotWrapper.GetFreeSlots(_slotType);
                    }
                }

                return _slots;
            }
        }

        public SlotsProvider(
            ISlotWrapper slotWrapper, 
            SlotType slotType, 
            BuildingFunction buildingFunction,
            bool preferFrontmostSlot)
        {
            Assert.IsNotNull(slotWrapper);

            _slotWrapper = slotWrapper;
            _slotType = slotType;
            _buildingFunction = buildingFunction;
            _preferFrontmostSlot = preferFrontmostSlot;
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
