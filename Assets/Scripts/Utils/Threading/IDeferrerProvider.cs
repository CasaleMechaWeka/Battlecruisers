namespace BattleCruisers.Utils.Threading
{
    public interface IDeferrerProvider
    {
        IDeferrer Deferrer { get; }
        IDeferrer RealTimeDeferrer { get; }
    }
}