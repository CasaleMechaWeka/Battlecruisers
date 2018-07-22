using System;

namespace BattleCruisers.UI
{
    public interface IBroadcastingFilter
    {
        event EventHandler PotentialMatchChange;

        bool IsMatch { get; }
    }

    public interface IBroadcastingFilter<TElement>
    {
        event EventHandler PotentialMatchChange;

        bool IsMatch(TElement element);
    }
}
