using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public interface IUnitSpawnDecider
    {
        bool CanSpawnUnit(IUnit unitToSpawn);
    }
}