using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class SystemInfoBC : ISystemInfo
    {
        // FELIX  TEMP  Test touch :)
        public DeviceType DeviceType => DeviceType.Handheld;
        //public DeviceType DeviceType => SystemInfo.deviceType;
    }
}