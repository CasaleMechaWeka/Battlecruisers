using System;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserDamageMonitor
    {
        event EventHandler CruiserOrBuildingDamaged;
    }
}