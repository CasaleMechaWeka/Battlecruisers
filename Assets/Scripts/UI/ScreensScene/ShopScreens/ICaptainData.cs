using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public interface ICaptainData
    {
        int CaptainCost { get; }
        string NameStringKeyBase { get; }
        string DescriptionKeyBase { get; }
        bool IsOwned { get; }
        int Index { get; }
    }
}