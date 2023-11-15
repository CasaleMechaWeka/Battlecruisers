using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public class PvPSystemInfoBC : IPvPSystemInfo
    {
        private static IPvPSystemInfo _instance;
        public static IPvPSystemInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PvPSystemInfoBC();
                }
                return _instance;
            }
        }

        private PvPSystemInfoBC() { }

        public bool IsHandheld => SystemInfo.deviceType == DeviceType.Handheld;
    }
}