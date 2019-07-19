using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class SystemInfoBC : ISystemInfo
    {
        public DeviceType DeviceType => SystemInfo.deviceType;
    }
}