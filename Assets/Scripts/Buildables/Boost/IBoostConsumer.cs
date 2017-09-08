using System;

namespace BattleCruisers.Buildables.Boost
{
    public interface IBoostConsumer
    {
        float CumulativeBoost { get; }

        event EventHandler BoostChanged;

		void AddBoostProvider(IBoostProvider boostProvider);
		void RemoveBoostProvider(IBoostProvider boostProvider);
	}
}
