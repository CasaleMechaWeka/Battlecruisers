using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class UnitButtonController : BuildableButtonController
	{
		private IBuildableWrapper<IUnit> _unitWrapper;
		private IFactory _factory;

        public void Initialise(IBuildableWrapper<IUnit> unitWrapper, IUIManager uiManager, IActivenessDecider<IBuildable> activenessDecider)
		{
            base.Initialise(unitWrapper.Buildable, uiManager, activenessDecider);

			_unitWrapper = unitWrapper;
		}

		public override void OnPresenting(object activationParameter)
		{
			_factory = activationParameter.Parse<IFactory>();

			if (_factory.BuildableState != BuildableState.Completed)
			{
				_factory.CompletedBuildable += _factory_CompletedBuildable;
			}

			// Usually have this at the start of the overriding method, but 
			// do not want parent to call ShouldBeEnabled() until we have
			// set our _factory field.
			base.OnPresenting(activationParameter);
		}

		private void _factory_CompletedBuildable(object sender, System.EventArgs e)
		{
			UpdateButtonActiveness();
		}

		public override void OnDismissing()
		{
			base.OnDismissing();

			_factory.CompletedBuildable -= _factory_CompletedBuildable;
			_factory = null;
		}

		protected override bool ShouldBeEnabled()
		{
            return
                base.ShouldBeEnabled()
                && _factory.BuildableState == BuildableState.Completed;
		}

		protected override void HandleClick()
		{
            base.HandleClick();

			_factory.UnitWrapper = _unitWrapper;
			_uiManager.ShowUnitDetails(_unitWrapper.Buildable);
		}
	}
}