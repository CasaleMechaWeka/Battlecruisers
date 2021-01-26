namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface IAssignerFactory
    {
        IAssigner CreateAssigner(int numOfOptions);
    }
}
