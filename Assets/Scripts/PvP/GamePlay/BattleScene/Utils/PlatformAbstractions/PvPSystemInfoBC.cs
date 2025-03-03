using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public class PvPSystemInfoBC : ISystemInfo
    {
        private static ISystemInfo _instance;
        public static ISystemInfo Instance
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