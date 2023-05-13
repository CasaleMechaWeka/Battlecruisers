using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public interface IPvPInformatorPanel : IPvPSlidingPanel
    {
        IPvPComparableItemDetails<IPvPBuilding> BuildingDetails { get; }
        IPvPComparableItemDetails<IPvPUnit> UnitDetails { get; }
        IPvPComparableItemDetails<IPvPCruiser> CruiserDetails { get; }
        IPvPInformatorButtons Buttons { get; }
        IPvPSlidingPanel ExtendedPanel { get; }

        void Show(IPvPTarget item);
    }
}