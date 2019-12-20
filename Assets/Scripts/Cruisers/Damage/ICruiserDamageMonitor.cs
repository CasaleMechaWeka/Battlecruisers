using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.Cruisers.Damage
{
    public interface ICruiserDamageMonitor
    {
        ITarget LastCruiserDamageSource { get; }

        event EventHandler CruiserOrBuildingDamaged;
    }
}