using System;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairManager : IDisposable
    {
        void Repair(float deltaTimeInS);
    }
}
