namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public class AssignerFactory : IAssignerFactory
    {
        public IAssigner CreateAssigner(int numOfOptions)
        {
            return new LinearProportionAssigner(numOfOptions);
        }
    }
}
