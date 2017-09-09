using System;

namespace BattleCruisers.Buildables.Boost
{
	public interface IBoostUser
	{
		void AddBoostProvider(IBoostProvider boostProvider);
		void RemoveBoostProvider(IBoostProvider boostProvider);
	}
}
