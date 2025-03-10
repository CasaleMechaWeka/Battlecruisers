using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.AI.Drones
{
    public interface IBuildingProvider
    {
        IBuilding Building { get; }
    }
}
