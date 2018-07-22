using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI
{
    public interface IBroadcastingFilter
    {
        event EventHandler PotentialMatchChange;

        bool IsMatch { get; }
    }

    public interface IBroadcastingFilter<TElement> : IFilter<TElement>
    {
        event EventHandler PotentialMatchChange;
    }
}
