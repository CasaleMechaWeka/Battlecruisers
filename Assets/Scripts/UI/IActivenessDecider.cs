using System;

namespace BattleCruisers.UI
{
    // FELIX  Should really be renamed to filter :/
    public interface IActivenessDecider
    {
        /// <summary>
        /// Emitted when element activeness may have changed.
        /// </summary>
        event EventHandler PotentialActivenessChange;

        bool ShouldBeEnabled { get; }
    }

    // FELIX  Should really be renamed to filter :/
    public interface IActivenessDecider<TElement>
    {
        /// <summary>
        /// Emitted when element activeness may have changed.
        /// </summary>
        event EventHandler PotentialActivenessChange;

        bool ShouldBeEnabled(TElement element);
    }
}
