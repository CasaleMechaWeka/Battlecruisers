using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test
{
    public class MissileStationaryTargetTestsGod : MissileTestsGod 
	{
		void Start () 
		{
			Helper helper = new Helper();

			TestAircraftController target = FindObjectOfType<TestAircraftController>();
            target.UseDummyMovementController = true;
			helper.InitialiseBuildable(target);
			target.StartConstruction();

			SetupMissiles(target);
		}
	}
}
