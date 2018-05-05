using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders
{
    public interface IBuildingPermitter
    {
        IPrefabKey PermittedBuilding { set; }
    }
}
