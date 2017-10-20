using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BackButtonController : MonoBehaviour 
	{
		public void Initialize(UIManager uiManager)
		{
			Button button = GetComponent<Button>();
			button.onClick.AddListener(() => uiManager.ShowBuildingGroups());
		}
	}
}
