using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public interface ICaptainExoData
    {
        Sprite CaptainExoImage { get; }
        string CaptainExoName { get; }
        int CaptainExoCost { get; }
        string StringKeyBase { get; }
        int CaptainIndex { get; }

        bool IsOwned { get; set; }
    }
}

