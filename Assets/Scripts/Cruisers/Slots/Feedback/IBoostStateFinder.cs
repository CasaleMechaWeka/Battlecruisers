namespace BattleCruisers.Cruisers.Slots.Feedback
{
    public interface IBoostStateFinder
    {
        BoostState FindState(int numOfLocalBoosters, bool isBuildingBoostable);
    }
}