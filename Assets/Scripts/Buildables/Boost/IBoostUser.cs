namespace BattleCruisers.Buildables.Boost
{
    // FELIX  Merge with boost consumer :)
    public interface IBoostUser
	{
		void AddBoostProvider(IBoostProvider boostProvider);
		void RemoveBoostProvider(IBoostProvider boostProvider);
	}
}
