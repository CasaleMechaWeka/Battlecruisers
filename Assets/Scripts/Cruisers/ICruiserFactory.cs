﻿using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.ProgressBars;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserFactory
    {
        void InitialiseCruiser(Cruiser cruiser, ICruiser enemyCruiser, HealthBarController healthBar, Faction faction, Direction facingDirection);       
    }
}