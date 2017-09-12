using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class ShieldGenerator : Building
	{
        private ShieldController _shieldController;

		public override TargetValue TargetValue { get { return TargetValue.Medium; } }

		public override void StaticInitialise()
        {
            base.StaticInitialise();

            _shieldController = GetComponentInChildren<ShieldController>(includeInactive: true);
            Assert.IsNotNull(_shieldController);
        }

		protected override void OnInitialised()
		{
			base.OnInitialised();

			_shieldController.Initialise(Faction);
			_shieldController.gameObject.SetActive(false);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_shieldController.gameObject.SetActive(true);
		}
	}
}
