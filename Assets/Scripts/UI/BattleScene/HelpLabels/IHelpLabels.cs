using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    // FELIX  Remove all old help labels :P
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