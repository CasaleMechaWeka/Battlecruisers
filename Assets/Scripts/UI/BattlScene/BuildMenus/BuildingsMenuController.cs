using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingsMenuController : BuildablesMenuController<IBuilding>
	{
        protected override BuildableButtonController CreateBuildableButton(IUIFactory uiFactory, HorizontalLayoutGroup buttonParent, IBuildableWrapper<IBuilding> buildable)
        {
            return uiFactory.CreateBuildingButton(buttonParent, buildable);
        }
    }
}
