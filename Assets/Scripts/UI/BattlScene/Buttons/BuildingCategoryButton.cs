using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingCategoryButton : MonoBehaviour 
	{
		public void Initialize(IBuildingGroup buildingGroup, IUIManager uiManager)
		{
			Button button = GetComponent<Button>();
			button.GetComponentInChildren<Text>().text = buildingGroup.BuildingGroupName;
			button.onClick.AddListener(() => uiManager.SelectBuildingGroup(buildingGroup.BuildingCategory));
		}
	}
}
