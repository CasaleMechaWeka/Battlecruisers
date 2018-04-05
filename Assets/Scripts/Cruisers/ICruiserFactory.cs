﻿using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Cameras;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserFactory
    {
        void InitialiseCruiser(
            Cruiser cruiser, 
            ICruiser enemyCruiser, 
            HealthBarController healthBar, 
            IUIManager uiManager, 
            ICameraController cameraController,
            Faction faction, 
            Direction facingDirection);       
    }
}