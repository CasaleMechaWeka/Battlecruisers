using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class SystemInfoBC : ISystemInfo
    {
        // FELIX  Create singleton :)

        public bool IsHandheld => SystemInfo.deviceType == DeviceType.Handheld;
    }
}