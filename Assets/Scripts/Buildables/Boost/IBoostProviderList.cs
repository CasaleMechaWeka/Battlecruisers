using System;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost
{
	public interface IBoostProviderList
	{
        ReadOnlyCollection<IBoostProvider> BoostProviders { get; }

		event EventHandler ProvidersChanged;

		void AddBoostProvider(IBoostProvider boostProvider);
		void RemoveBoostProvider(IBoostProvider boostProvider);
	}
}
