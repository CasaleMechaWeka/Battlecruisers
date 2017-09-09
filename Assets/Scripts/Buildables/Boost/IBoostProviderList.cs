using System;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost
{
    public interface IBoostProviderList : IBoostUser
	{
        ReadOnlyCollection<IBoostProvider> BoostProviders { get; }

		event EventHandler ProvidersChanged;
	}
}
