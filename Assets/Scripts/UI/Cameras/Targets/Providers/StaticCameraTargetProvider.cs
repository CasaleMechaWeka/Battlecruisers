namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Test :)
    public class StaticCameraTargetProvider : UserInputCameraTargetProvider, IStaticCameraTargetProvider
    {
        public override int Priority => 5;

        public void SetTarget(ICameraTarget target)
        {
            if (Target == null
                && target != null)
            {
                RaiseUserInputStarted();
            }
            else if (Target != null
                && target == null)
            {
                RaiseUserInputEnded();
            }

            Target = target;
        }
    }
}