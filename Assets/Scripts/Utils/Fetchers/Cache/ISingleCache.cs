namespace BattleCruisers.Utils.Fetchers.Cache
{
    public interface ISingleCache<TPrefab> where TPrefab : class
    {
        TPrefab GetPrefab();
    }
}