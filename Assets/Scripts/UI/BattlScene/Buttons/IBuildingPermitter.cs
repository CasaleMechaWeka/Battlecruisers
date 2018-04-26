using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public interface IBuildingPermitter
    {
        IPrefabKey PermittedBuilding { set; }
    }
}
