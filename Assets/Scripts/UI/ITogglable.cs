using System;

namespace BattleCruisers.UI
{
    public interface ITogglable
    {
        event EventHandler EnabledChange;

        bool Enabled { get; set; }
    }
}