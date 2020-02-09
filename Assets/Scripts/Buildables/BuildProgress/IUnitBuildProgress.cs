using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Buildables.BuildProgress
{
    public interface IUnitBuildProgress
    {
        void ShowBuildProgressIfNecessary(IUnit unit, IFactory factory);
    }
}