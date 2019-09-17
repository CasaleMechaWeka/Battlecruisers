using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Cruisers.Slots.Feedback
{
    public interface IBoostStateFinder
    {
        BoostState FindState(int numOfLocalBoosters, IBuilding building);
    }
}