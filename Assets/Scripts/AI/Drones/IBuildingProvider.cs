using BattleCruisers.Buildables;

namespace BattleCruisers.AI.Drones
{
    public interface IBuildingProvider
    {
        // FELIX  IBuilding once events change :D
        IBuildable Building { get; }
    }
}
