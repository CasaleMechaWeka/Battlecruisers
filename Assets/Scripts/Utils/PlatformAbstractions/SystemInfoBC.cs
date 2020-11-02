using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class SystemInfoBC : ISystemInfo
    {
        public bool IsHandheld => SystemInfo.deviceType == DeviceType.Handheld;
    }
}