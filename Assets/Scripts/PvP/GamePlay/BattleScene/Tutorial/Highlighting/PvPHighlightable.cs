using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting
{
    public class PvPHighlightable : MonoBehaviour, IPvPHighlightable
    {
        private RectTransform _rectTransform;

        public float sizeMultiplier = 1;

        public void Initialise()
        {
            _rectTransform = transform.Parse<RectTransform>();
        }

        public PvPHighlightArgs CreateHighlightArgs(IPvPHighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForOnCanvasObject(_rectTransform, sizeMultiplier);
        }
    }
}