namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public class AssignerFactory
    {
        public IAssigner CreateAssigner(int numOfOptions)
        {
            return new LinearProportionAssigner(numOfOptions);
        }
    }
}
