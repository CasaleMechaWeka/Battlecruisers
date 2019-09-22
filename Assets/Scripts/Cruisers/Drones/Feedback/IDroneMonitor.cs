using BattleCruisers.Buildables;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public interface IDroneMonitor
    {
        bool ShouldPlaySound(Faction faction);
    }
}