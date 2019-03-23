using System;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserDestroyedMonitor
    {
        event EventHandler CruiserDestroyed;
    }
}