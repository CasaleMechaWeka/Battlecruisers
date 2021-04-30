using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IInformatorPanel : ISlidingPanel
    {
        IComparableItemDetails<IBuilding> BuildingDetails { get; }
        IComparableItemDetails<IUnit> UnitDetails { get; }
        IComparableItemDetails<ICruiser> CruiserDetails { get; }
        IInformatorButtons Buttons { get; }
        ISlidingPanel ExtendedPanel { get; }

        void Show(ITarget item);
    }
}