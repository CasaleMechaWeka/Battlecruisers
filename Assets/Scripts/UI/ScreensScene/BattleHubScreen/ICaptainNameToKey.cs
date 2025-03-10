namespace BattleCruisers.Data.Models.PrefabKeys
{
    public interface ICaptainNameToKey
    {
        IPrefabKey GetKey(string hullName);
    }
}
