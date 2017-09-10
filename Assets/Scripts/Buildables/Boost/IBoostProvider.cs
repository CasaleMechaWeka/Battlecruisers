namespace BattleCruisers.Buildables.Boost
{
    public interface IBoostProvider
    {
		// < 1 to reduce performance, > 1 to improve performance, 1 by default
		float BoostMultiplier { get; }

        void AddBoostConsumer(IBoostConsumer boostConsumer);
        void RemoveBoostConsumer(IBoostConsumer boostConsumer);
        void ClearBoostConsumers();
	}
}
