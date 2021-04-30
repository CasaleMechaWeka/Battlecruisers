using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    // FELIX  Test :D
    public class HelpStateFinder : IHelpStateFinder
    {
        private readonly ISlidingPanel _informatorPanel, _selectorPanel;
        private readonly IHelpState _bothCollapsed, _selectorShown, _informatorShown, _bothShown;

        public HelpStateFinder(
            ISlidingPanel informatorPanel, 
            ISlidingPanel selectorPanel, 
            IHelpState bothCollapsed, 
            IHelpState selectorShown, 
            IHelpState informatorShown, 
            IHelpState bothShown)
        {
            Helper.AssertIsNotNull(informatorPanel, selectorPanel, bothCollapsed, selectorPanel, informatorPanel, bothShown);

            _informatorPanel = informatorPanel;
            _selectorPanel = selectorPanel;
            _bothCollapsed = bothCollapsed;
            _selectorShown = selectorShown;
            _informatorShown = informatorShown;
            _bothShown = bothShown;
        }

        public IHelpState FindHelpState()
        {
            if (_informatorPanel.TargetState == PanelState.Shown)
            {
                if (_selectorPanel.TargetState == PanelState.Shown)
                {
                    return _bothShown;
                }
                else
                {
                    return _informatorShown;
                }
            }
            else
            {
                if (_selectorPanel.TargetState == PanelState.Shown)
                {
                    return _selectorShown;
                }
                else
                {
                    return _bothCollapsed;
                }
            }
        }
    }
}