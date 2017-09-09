using System;

namespace BattleCruisers.Buildables.Boost
{
    public interface IBoostConsumer : IBoostUser
    {
        float CumulativeBoost { get; }

        event EventHandler BoostChanged;
	}
}
