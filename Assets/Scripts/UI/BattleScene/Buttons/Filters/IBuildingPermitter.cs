using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    public interface IBuildingPermitter
    {
        IPrefabKey PermittedBuilding { set; }
    }
}
