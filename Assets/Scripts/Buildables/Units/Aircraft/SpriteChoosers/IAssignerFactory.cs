namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface IAssignerFactory
    {
        IAssigner CreateRecursiveProportionAssigner(
            int numOfOptions,
            float baseCutoff = RecursiveProportionAssigner.DEFAULT_BASE_CUTOFF);
    }
}
