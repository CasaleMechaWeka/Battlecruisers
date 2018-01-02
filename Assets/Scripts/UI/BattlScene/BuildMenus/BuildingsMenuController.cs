using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingsMenuController : BuildablesMenuController<IBuilding>
	{
        protected override IPresentable CreateBuildableButton(IUIFactory uiFactory, HorizontalLayoutGroup buttonParent, IBuildableWrapper<IBuilding> buildable)
        {
            return uiFactory.CreateBuildingButton(buttonParent, buildable);
        }
    }
}
