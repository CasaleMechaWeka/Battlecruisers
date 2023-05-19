using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using UnityEngine;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public interface IPvPBattleSceneHelper
    {
        IPvPPrefabKey PlayerCruiser { get; }
        IPvPBuildingCategoryPermitter BuildingCategoryPermitter { get; }
        IPvPLevel GetPvPLevel();
        IPvPSlotFilter CreateHighlightableSlotFilter();
        IPvPBuildProgressCalculator CreatePlayerCruiserBuildProgressCalculator();
        IPvPBuildProgressCalculator CreateAICruiserBuildProgressCalculator();
    }
}

