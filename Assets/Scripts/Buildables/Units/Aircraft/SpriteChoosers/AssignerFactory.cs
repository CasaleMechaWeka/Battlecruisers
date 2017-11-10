namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public class AssignerFactory : IAssignerFactory
    {
        public IAssigner CreateRecursiveProportionAssigner(
            int numOfOptions,
            float baseCutoff = RecursiveProportionAssigner.DEFAULT_BASE_CUTOFF)
        {
            return new RecursiveProportionAssigner(numOfOptions, baseCutoff);
        }
    }
}
