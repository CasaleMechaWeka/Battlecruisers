using System;

namespace BattleCruisers.Buildables.Boost
{
    /// <summary>
    /// Consumes boost producer(s).  Simply provides the cumulative boost
    /// of all boost providers.
    /// </summary>
    public interface IBoostConsumer
    {
        float CumulativeBoost { get; }

        event EventHandler BoostChanged;

		void AddBoostProvider(IBoostProvider boostProvider);
        void RemoveBoostProvider(IBoostProvider boostProvider);
	}
}
