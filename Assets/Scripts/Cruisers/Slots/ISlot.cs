using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI;
using System;
using System.Collections.ObjectModel;
using UnityCommon.PlatformAbstractions;
using UnityCommon.Properties;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots
{
    public enum SlotType
	{
        // Explicitly set integner values, because the Unity inspector binds
        // to the interger values.  So now, if I decide to get rid of a slot
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
        public ISlot BuildingParent { get; }

        public SlotBuildingDestroyedEventArgs(ISlot buildingParent)
        {
            BuildingParent = buildingParent;
        }
    }

    public interface ISlot : IClickableEmitter, IHighlightable
    {
        bool IsFree { get; }
        SlotType Type { get; }
        /// <summary>
        /// The type of building this slot is well positioned for.  Eg, for AntiShip
        /// buildings that is the cruiser front.  For shields that is spread accross
        /// the cruiser.
        /// </summary>
        BuildingFunction BuildingFunctionAffinity { get; }
        Direction Direction { get; }
        IBroadcastingProperty<IBuilding> Building { get; }
        ObservableCollection<IBoostProvider> BoostProviders { get; }
        bool IsVisible { get; set; }
        Vector2 Position { get; }
        ITransform Transform { get; }

        /// <summary>
        /// Reference point used to line up buildings with.
        /// </summary>
        Vector3 BuildingPlacementPoint { get; }

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

        void Initialise(ICruiser parentCruiser, ReadOnlyCollection<ISlot> neighbouringSlots, IBuildingPlacer buildingPlacer);
        void SetBuilding(IBuilding building);
	}
}
