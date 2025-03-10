using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public class PvPScreenBC : IScreen
    {
        public float WidthInPixels => Screen.width;
        public float HeightInPixels => Screen.height;
    }
}
