using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Presentables;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public abstract class NEWBuildablesMenuController<TBuildable> : PresentableController
        where TBuildable : class, IBuildable
	{
        public ReadOnlyCollection<IBuildableButton> BuildableButtons { get; private set; }

		public void Initialise(IList<IBuildableWrapper<TBuildable>> buildables)
		{
			base.Initialise();

            Assert.IsNotNull(buildables);

            IList<BuildableButtonController> buildableButtons = GetComponentsInChildren<BuildableButtonController>().ToList();
            Assert.IsTrue(buildables.Count <= buildableButtons.Count);

			for (int i = 0; i < buildables.Count; ++i)
			{
                BuildableButtonController buildableButton = buildableButtons[i];

                if (i < buildables.Count)
                {
                    // Have buildable for button
                    InitialiseBuildableButton(buildables[i]);
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

        protected abstract void InitialiseBuildableButton(IBuildableWrapper<TBuildable> buildable);
	}
}
