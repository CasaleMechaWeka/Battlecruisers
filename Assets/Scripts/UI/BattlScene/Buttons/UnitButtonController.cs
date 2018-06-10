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

		public override bool IsMatch
		{
			get
			{
				return 
                    base.IsMatch
                    && _factory != null
                    && _factory.BuildableState == BuildableState.Completed;
			}
		}

		public void Initialise(IBuildableWrapper<IUnit> unitWrapper, IUIManager uiManager, IFilter<IBuildable> shouldBeEnabledFilter)
		{
            base.Initialise(unitWrapper.Buildable, uiManager, shouldBeEnabledFilter);

			_unitWrapper = unitWrapper;
		}

		public override void OnPresenting(object activationParameter)
		{
			_factory = activationParameter.Parse<IFactory>();

			if (_factory.BuildableState != BuildableState.Completed)
			{
				_factory.CompletedBuildable += _factory_CompletedBuildable;
			}

            TriggerPotentialMatchChange();

			// Usually have this at the start of the overriding method, but 
			// do not want parent to call ShouldBeEnabled() until we have
			// set our _factory field.
			base.OnPresenting(activationParameter);
		}

		private void _factory_CompletedBuildable(object sender, System.EventArgs e)
		{
            TriggerPotentialMatchChange();
		}

		public override void OnDismissing()
		{
			base.OnDismissing();

			_factory.CompletedBuildable -= _factory_CompletedBuildable;
			_factory = null;
		}

		protected override void HandleClick()
		{
            base.HandleClick();

			_factory.UnitWrapper = _unitWrapper;
			_uiManager.ShowUnitDetails(_unitWrapper.Buildable);
		}
	}
}