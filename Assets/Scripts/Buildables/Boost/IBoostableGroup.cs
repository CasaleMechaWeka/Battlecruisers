namespace BattleCruisers.Buildables.Boost
{
    public interface IBoostableGroup
    {
        IBoostConsumer BoostConsumer { get; }

        void AddBoostable(IBoostable boostable);
        bool RemoveBoostable(IBoostable boostable);
    }
}
