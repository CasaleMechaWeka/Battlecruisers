using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IInformatorPanel : IPanel
    {
        IBuildableDetails<IBuilding> BuildingDetails { get; }
        IBuildableDetails<IUnit> UnitDetails { get; }
        ICruiserDetails CruiserDetails { get; }
    }
}