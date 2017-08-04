using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene
{
    public interface IUIFactory
    {
        GameObject CreatePanel(bool isActive);
        Button CreateBuildingCategoryButton(HorizontalLayoutGroup buttonParent, IBuildingGroup group);
        BuildingButtonController CreateBuildingButton(HorizontalLayoutGroup buttonParent, IBuildableWrapper<IBuilding> buildingWrapper);
        UnitButtonController CreateUnitButton(HorizontalLayoutGroup buttonParent, IBuildableWrapper<IUnit> unitWrapper);
        Button CreateBackButton(HorizontalLayoutGroup buttonParent);
    }
}
