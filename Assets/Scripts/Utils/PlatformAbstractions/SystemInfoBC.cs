using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class SystemInfoBC : ISystemInfo
    {
        private static ISystemInfo _instance;
        public static ISystemInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SystemInfoBC();
                }
                return _instance;
            }
        }

        private SystemInfoBC() { }

        public bool IsHandheld => SystemInfo.deviceType == DeviceType.Handheld;
    }
}