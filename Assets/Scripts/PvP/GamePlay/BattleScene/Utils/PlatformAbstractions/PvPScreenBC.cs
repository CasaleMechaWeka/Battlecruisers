using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public class PvPScreenBC : IPvPScreen
    {
        public float WidthInPixels => Screen.width;
        public float HeightInPixels => Screen.height;
    }
}
