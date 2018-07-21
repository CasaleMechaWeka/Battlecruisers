using BattleCruisers.Buildables;

namespace BattleCruisers.AI.Drones
{
    public interface IBuildingMonitor
    {
        IBuildable GetNonFocusedAffordableBuilding();
    }
}
