namespace BattleCruisers.Utils.Threading
{
    public interface IDeferrerProvider
    {
        IVariableDelayDeferrer VariableDelayDeferrer { get; }
    }
}