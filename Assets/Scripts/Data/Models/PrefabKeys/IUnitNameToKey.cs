namespace BattleCruisers.Data.Models.PrefabKeys
{
    public interface IUnitNameToKey
    {
        UnitKey GetKey(string unitName);
    }
}