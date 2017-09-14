using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots
{
    // FELIX  Remove SternTop & BowTop slots?
    public enum SlotType
	{
		None, SternTop, SternBottom, BowTop, BowBottom, Platform, Deck, Utility, Mast
	}

    public interface ISlot
    {
        Vector2 Position { get; }
        bool IsFree { get; }
        SlotType Type { get; }
        bool IsActive { set; }
        IBuilding Building { set; }
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
	}
}
