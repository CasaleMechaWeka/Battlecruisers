using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    public interface IHelpLabels
    {
        IPanel CruiserHealth { get; }
        IPanel LeftBottom { get; }
        IPanel BuildingCategories { get; }
        IPanel RightBottom { get; }
        IPanel Informator { get; }
        IPanel BuildMenu { get; }
    }
}