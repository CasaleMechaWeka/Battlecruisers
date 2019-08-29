using System;

namespace BattleCruisers.Cruisers.Construction
{
    public interface IPopulationLimitMonitor
    {
        bool IsPopulationLimitReached { get; }

        event EventHandler PopulationLimitReached;
    }
}