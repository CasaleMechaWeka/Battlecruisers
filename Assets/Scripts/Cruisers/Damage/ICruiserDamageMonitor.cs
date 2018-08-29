using System;

namespace BattleCruisers.Cruisers.Damage
{
    public interface ICruiserDamageMonitor
    {
        event EventHandler CruiserOrBuildingDamaged;
    }
}