using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public interface IBodykit
    {
        Sprite BodykitImage { get; }
        HullType CruiserType { get; }
    }

    public enum HullType
    {
        Bullshark,
        Eagle,
        Hammerhead,
        Longbow,
        Megalodon,
        Raptor,
        Rockjaw,
        Trident,
        TasDevil,
        Yeti,
        Rickshaw,
        BlackRig,
        None   //  no bodykit
    }
}
