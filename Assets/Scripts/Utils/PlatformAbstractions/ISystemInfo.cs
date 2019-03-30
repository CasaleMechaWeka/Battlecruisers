using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public interface ISystemInfo
    {
        DeviceType DeviceType { get; }
    }
}