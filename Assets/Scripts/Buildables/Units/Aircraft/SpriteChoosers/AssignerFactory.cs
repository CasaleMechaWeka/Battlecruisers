namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public static class AssignerFactory
    {
        public static IAssigner CreateAssigner(int numOfOptions)
        {
            return new LinearProportionAssigner(numOfOptions);
        }
    }
}
