using BattleCruisers.Buildables.Buildings.Factories;

namespace BattleCruisers.Buildables.BuildProgress
{
    public interface IUnitBuildProgressTrigger
    {
        IFactory Factory { set; }
    }
}