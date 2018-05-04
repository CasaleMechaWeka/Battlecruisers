using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class HomePanel : Presentable, IBuildingCategoryButtonsPanel
    {
        private IDictionary<BuildingCategory, IBuildingCategoryButton> _cagetoryToButton;

        public override void Initialise()
        {
            base.Initialise();
            _cagetoryToButton = new Dictionary<BuildingCategory, IBuildingCategoryButton>();
        }
		
        public void AddCategoryButton(IBuildingCategoryButton buttonToAdd)
        {
            Assert.IsFalse(_cagetoryToButton.ContainsKey(buttonToAdd.Category));
            _cagetoryToButton.Add(buttonToAdd.Category, buttonToAdd);
        }

		public IBuildingCategoryButton GetCategoryButton(BuildingCategory category)
		{
			IBuildingCategoryButton button = null;
			
			if (_cagetoryToButton.ContainsKey(category))
			{
				button = _cagetoryToButton[category];
			}
			
			return button;
		}
    }
}
