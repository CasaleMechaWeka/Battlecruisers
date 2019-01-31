namespace BattleCruisers.Data.Models.PrefabKeys
{
    public interface IHullNameToKey
    {
        HullKey GetKey(string hullName);
    }
}