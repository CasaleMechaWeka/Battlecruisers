namespace BattleCruisers.Data.Models.PrefabKeys
{
    public interface IBuildingNameToKey
    {
        BuildingKey GetKey(string buildingName);
    }
}