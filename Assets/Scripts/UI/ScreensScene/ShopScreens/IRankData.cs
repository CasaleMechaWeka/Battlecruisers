using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface IRankData
    {
        string RankImage { get; }
        string RankNumber { get; }
        string RankNameKeyBase { get; }
    }
}
