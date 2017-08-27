using System;

namespace BattleCruisers.Buildables
{
    public interface IRepairManager : IDisposable
    {
        void Repair(float deltaTimeInS);
    }
}
