using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public abstract class NEWBuildablesMenuController<TButton, TBuildable> : PresentableController, IBuildablesMenu
        where TButton : BuildableButtonController
        where TBuildable : class, IBuildable
	{
        protected IUIManager _uiManager;
        protected IBroadcastingFilter<IBuildable> _shouldBeEnabledFilter;

        public ReadOnlyCollection<IBuildableButton> BuildableButtons { get; private set; }

		public virtual void Initialise(
            IList<IBuildableWrapper<TBuildable>> buildables, 
            IUIManager uiManager, 
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter)
		{
			base.Initialise();

            Helper.AssertIsNotNull(buildables, uiManager, shouldBeEnabledFilter);

            _uiManager = uiManager;
            _shouldBeEnabledFilter = shouldBeEnabledFilter;

            IList<TButton> buildableButtons = GetComponentsInChildren<TButton>().ToList();
            Assert.IsTrue(buildables.Count <= buildableButtons.Count);

			for (int i = 0; i < buildables.Count; ++i)
			{
                TButton buildableButton = buildableButtons[i];

                if (i < buildables.Count)
                {
                    // Have buildable for button
                    InitialiseBuildableButton(buildableButton, buildables[i]);
                    AddChildPresentable(buildableButton);
                }
                else
                {
                    // Have no buildable for button (user has not unlocked it yet)
                    Destroy(buildableButton);
                }
			}

            BuildableButtons
                = buildableButtons
                    .Select(button => (IBuildableButton)button)
                    .ToList()
                    .AsReadOnly();
		}

        // FELIX  Rename buildable to buildableWrapper
        protected abstract void InitialiseBuildableButton(TButton button, IBuildableWrapper<TBuildable> buildable);
	}
}
