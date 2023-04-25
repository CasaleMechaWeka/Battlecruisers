
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public enum PvPSlotType
    {
        Utility = 1,
        Mast = 2,
        Bow = 3,
        Platform = 4,
        Deck = 5
    }
    public interface IPvPSlot : IPvPClickableEmitter, IPvPHighlightable
    {
        bool IsFree { get; }
        PvPSlotType Type { get; }
        /// <summary>
        /// The type of building this slot is well positioned for.  Eg, for AntiShip
        /// buildings that is the cruiser front.  For shields that is spread accross
        /// the cruiser.
        /// </summary>
        PvPBuildingFunction BuildingFunctionAffinity { get; }
        PvPDirection Direction { get; }
        IPvPBroadcastingProperty<IPvPBuilding> Building { get; }
        ObservableCollection<IPvPBoostProvider> BoostProviders { get; }
        bool IsVisible { get; set; }
        Vector2 Position { get; }
        IPvPTransform Transform { get; }

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
        ReadOnlyCollection<IPvPSlot> NeighbouringSlots { get; }

        void Initialise(IPvPCruiser parentCruiser, ReadOnlyCollection<IPvPSlot> neighbouringSlots, IPvPBuildingPlacer buildingPlacer);
        void SetBuilding(IPvPBuilding building);

        void controlBuildingPlacementFeedback(bool active);

        void stopBuildingPlacementFeedback();
    }
}

