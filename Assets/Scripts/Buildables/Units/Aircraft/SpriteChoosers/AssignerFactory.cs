namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public class AssignerFactory : IAssignerFactory
    {
        public IAssigner CreateRecursiveProportionAssigner(
            int numOfOptions,
            // FELIX  Remove?  Propagate :)
            float baseCutoff = RecursiveProportionAssigner.DEFAULT_BASE_CUTOFF)
        {
            //  FELIX
            return new LinearProportionAssigner(numOfOptions);
            //return new RecursiveProportionAssigner(numOfOptions, baseCutoff);
        }
    }
}
