using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Cruisers.Slots
{
    public enum SlotType
	{
        // Explicitly set integner values, because the Unity inspector binds
        // to the interger values.  So now, if I decide to get fid of a slot
        // type (yet again), I don't need to adjust every single prefab 
        // that has a slot type field.  Thanks Manya!
        Utility = 1, 
        Mast = 2, 
        Bow = 3, 
        Platform = 4, 
        Deck = 5
	}

    public class SlotBuildingDestroyedEventArgs : EventArgs
    {
        public ISlot BuildingParent { get; private set; }

        public SlotBuildingDestroyedEventArgs(ISlot buildingParent)
        {
            BuildingParent = buildingParent;
        }
    }

    public interface ISlot : IGameObject, IHighlightable, IClickable
    {
        bool IsFree { get; }
        SlotType Type { get; }
        IBuilding Building { get; set; }
        IObservableCollection<IBoostProvider> BoostProviders { get; }

        /// <summary>
        /// Slots are ordered via their index, from the crusier front (low
        /// index) to the cruiser rear (high index).  This is float and not
        /// int to allow the later insertion of slots without having to change
        /// the index of existing slots.
        /// </summary>
        float Index { get; }

        /// <summary>
		/// Usually contains 2 slots, the neighbour to the right and the neighbour to the left.
		/// Each cruiser will have two slots (the left most and the right most) that
		/// will only have one neighbour.
        /// </summary>
        ReadOnlyCollection<ISlot> NeighbouringSlots { get; }

        event EventHandler<SlotBuildingDestroyedEventArgs> BuildingDestroyed;

        void Initialise(ICruiser parentCruiser, IList<ISlot> neighbouringSlots, ISlotFilter clickableFilter);
        void HighlightSlot();
        void UnhighlightSlot();
	}
}
