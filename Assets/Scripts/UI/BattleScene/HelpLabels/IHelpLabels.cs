using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    public interface IHelpLabels
    {
        Panel CruiserHealth { get; }
        Panel LeftBottom { get; }
        Panel BuildingCategories { get; }
        Panel RightBottom { get; }
        Panel Informator { get; }
        Panel BuildMenu { get; }
    }
}