using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Panels;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public interface IPvPInformatorPanel : ISlidingPanel
    {
        IPvPComparableItemDetails<IPvPBuilding> BuildingDetails { get; }
        IPvPComparableItemDetails<IPvPUnit> UnitDetails { get; }
        IPvPComparableItemDetails<IPvPCruiser> CruiserDetails { get; }
        IInformatorButtons Buttons { get; }
        ISlidingPanel ExtendedPanel { get; }

        void Show(ITarget item);
    }
}