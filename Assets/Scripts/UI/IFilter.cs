using System;

namespace BattleCruisers.UI
{
    public interface IFilter
    {
        event EventHandler PotentialMatchChange;

        bool IsMatch { get; }
    }

    public interface IFilter<TElement>
    {
        event EventHandler PotentialMatchChange;

        bool IsMatch(TElement element);
    }
}
