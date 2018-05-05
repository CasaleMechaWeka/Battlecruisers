using System;

namespace BattleCruisers.UI
{
    public interface IActivenessDecider
    {
        /// <summary>
        /// Emitted when element activeness may have changed.
        /// </summary>
        event EventHandler PotentialActivenessChange;

        bool ShouldBeEnabled { get; }
    }

    public interface IActivenessDecider<TElement>
    {
        /// <summary>
        /// Emitted when element activeness may have changed.
        /// </summary>
        event EventHandler PotentialActivenessChange;

        bool ShouldBeEnabled(TElement element);
    }
}
