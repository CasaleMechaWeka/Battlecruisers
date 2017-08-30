namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class ShieldGenerator : Building
	{
		public ShieldController shieldController;

		public override TargetValue TargetValue { get { return TargetValue.Medium; } }

		protected override void OnInitialised()
		{
			base.OnInitialised();

			shieldController.Initialise(Faction);
			shieldController.gameObject.SetActive(false);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			shieldController.gameObject.SetActive(true);
		}
	}
}
