using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.Network.Multiplay.Scenes
{
    public interface IMultiplayScreensSceneGod
    {
      
        void GotoSettingScreen();
        void GotoShopScreen();
        void LoadMainMenuScene();
        void LoadMatchmakingScene();
    }
}

