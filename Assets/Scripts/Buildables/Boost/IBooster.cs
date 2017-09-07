using System;

namespace BattleCruisers.Buildables.Boost
{
    public interface IBooster
    {
		// < 1 to reduce performance, > 1 to improve performance, 1 by default
		float BoostMultiplier { get; set; }

        event EventHandler BoostChanged;
	}
}
