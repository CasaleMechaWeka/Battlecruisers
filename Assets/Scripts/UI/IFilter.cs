using System;

namespace BattleCruisers.UI
{
    public interface IFilter
    {
        // FELIX  Rename all event handlers :P
        event EventHandler PotentialMatchChange;

        // FELIX  Change to parameterless method, to indicate that some work may be involved (ie, not just returning stored value)
        bool IsMatch { get; }
    }

    public interface IFilter<TElement>
    {
        event EventHandler PotentialMatchChange;

        bool IsMatch(TElement element);
    }
}
