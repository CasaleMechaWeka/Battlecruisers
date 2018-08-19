using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public abstract class BuildablesMenuController<TBuildable> : PresentableController
        where TBuildable : class, IBuildable
	{
        public ReadOnlyCollection<IBuildableButton> BuildableButtons { get; private set; }

		public void Initialise(
			IUIFactory uiFactory,
            IList<IBuildableWrapper<TBuildable>> buildables,
            IBuildableSorter<TBuildable> sorter)
		{
			base.Initialise();

            Helper.AssertIsNotNull(uiFactory, buildables, sorter);

            buildables = sorter.Sort(buildables);
            List<IBuildableButton> buildableButtons = new List<IBuildableButton>();

			// Create buildable buttons
			HorizontalLayoutGroup buttonGroup = GetComponent<HorizontalLayoutGroup>();

			for (int i = 0; i < buildables.Count; ++i)
			{
                BuildableButtonController button = CreateBuildableButton(uiFactory, buttonGroup, buildables[i]);
				_childPresentables.Add(button);
                buildableButtons.Add(button);
			}

            BuildableButtons = buildableButtons.AsReadOnly();
			uiFactory.CreateBackButton(buttonGroup);
		}

        protected abstract BuildableButtonController CreateBuildableButton(IUIFactory uiFactory, HorizontalLayoutGroup buttonParent, IBuildableWrapper<TBuildable> buildable);
	}
}
