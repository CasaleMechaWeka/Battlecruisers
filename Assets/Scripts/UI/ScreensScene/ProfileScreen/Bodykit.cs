using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class Bodykit : Prefab, IBodykit
    {
        public Sprite bodykitImage;
        public Sprite BodykitImage => bodykitImage;

        public HullType cruiserType;
        public HullType CruiserType => cruiserType;

    }
}
