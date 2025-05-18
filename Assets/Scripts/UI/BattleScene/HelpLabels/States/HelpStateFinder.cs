using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.HelpLabels.States
{
    public class HelpStateFinder
    {
        private readonly SlidingPanel _informatorPanel, _selectorPanel;
        private readonly HelpState _bothCollapsed, _selectorShown, _informatorShown, _bothShown;

        public HelpStateFinder(
            SlidingPanel informatorPanel,
            SlidingPanel selectorPanel,
            HelpState bothCollapsed,
            HelpState selectorShown,
            HelpState informatorShown,
            HelpState bothShown)
        {
            Helper.AssertIsNotNull(informatorPanel, selectorPanel, bothCollapsed, selectorPanel, informatorPanel, bothShown);

            _informatorPanel = informatorPanel;
            _selectorPanel = selectorPanel;
            _bothCollapsed = bothCollapsed;
            _selectorShown = selectorShown;
            _informatorShown = informatorShown;
            _bothShown = bothShown;
        }

        public HelpState FindHelpState()
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