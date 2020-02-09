using BattleCruisers.Buildables.Buildings.Factories;

namespace BattleCruisers.Buildables.BuildProgress
{
    public interface IUnitBuildProgress
    {
        IFactory Factory { set; }
    }
}