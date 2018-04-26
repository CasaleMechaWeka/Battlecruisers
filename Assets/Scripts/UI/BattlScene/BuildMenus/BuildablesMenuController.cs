using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Sorting;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public abstract class BuildablesMenuController<TBuildable> : Presentable
        where TBuildable : class, IBuildable
	{
		public void Initialise(
			IUIFactory uiFactory,
            IList<IBuildableWrapper<TBuildable>> buildables,
            IBuildableSorter<TBuildable> sorter)
		{
			base.Initialise();

            Helper.AssertIsNotNull(uiFactory, buildables, sorter);

            buildables = sorter.Sort(buildables);

			// Create buildable buttons
			HorizontalLayoutGroup buttonGroup = GetComponent<HorizontalLayoutGroup>();

			for (int i = 0; i < buildables.Count; ++i)
			{
                IPresentable button = CreateBuildableButton(uiFactory, buttonGroup, buildables[i]);
				_childPresentables.Add(button);
			}

			uiFactory.CreateBackButton(buttonGroup);
		}

        protected abstract IPresentable CreateBuildableButton(IUIFactory uiFactory, HorizontalLayoutGroup buttonParent, IBuildableWrapper<TBuildable> buildable);
	}
}
