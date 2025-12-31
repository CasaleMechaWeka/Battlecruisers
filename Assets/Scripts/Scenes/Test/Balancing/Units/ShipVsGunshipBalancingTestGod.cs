namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class ShipVsGunshipBalancingTestGod : MultiCameraTestGod<ShipVsGunshipBalancingTest>
    {
        protected override void InitialiseScenario(ShipVsGunshipBalancingTest scenario)
        {
            scenario.Initialise(_baseHelper);
        }
    }
}
