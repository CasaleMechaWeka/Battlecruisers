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
        Button CreateBuildingCategoryButton(HorizontalLayoutGroup buttonParent, BuildingGroup group);
        BuildingButtonController CreateBuildingButton(HorizontalLayoutGroup buttonParent, BuildingWrapper buildingWrapper);
        UnitButtonController CreateUnitButton(HorizontalLayoutGroup buttonParent, UnitWrapper unitWrapper);
        Button CreateBackButton(HorizontalLayoutGroup buttonParent);
    }
}
