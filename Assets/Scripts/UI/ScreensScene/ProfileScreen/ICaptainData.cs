using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public interface ICaptainData
    {
        Sprite CaptainImage { get; }
        string CaptainName { get; }
        string StringKeyBase { get; }
    }
}

