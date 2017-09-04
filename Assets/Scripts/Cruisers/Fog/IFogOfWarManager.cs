using System;

namespace BattleCruisers.Cruisers.Fog
{
    public interface IFogOfWarManager : IDisposable
    {
        bool IsFogEnabled { get; }

        event EventHandler IsFogEnabledChanged;
    }
}
