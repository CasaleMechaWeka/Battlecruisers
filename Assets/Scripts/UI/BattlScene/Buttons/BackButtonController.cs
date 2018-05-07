using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BackButtonController : MonoBehaviour 
	{
        public void Initialise(IUIManager uiManager, IFilter shouldBeEnabledFilter)
		{
            Helper.AssertIsNotNull(uiManager, shouldBeEnabledFilter);

            ButtonWrapper buttonWrapper = GetComponent<ButtonWrapper>();
            Assert.IsNotNull(buttonWrapper);
            buttonWrapper.Initialise(uiManager.ShowBuildingGroups, shouldBeEnabledFilter);
		}
	}
}
