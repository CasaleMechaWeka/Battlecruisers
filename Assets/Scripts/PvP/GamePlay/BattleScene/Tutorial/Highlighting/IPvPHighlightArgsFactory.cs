using BattleCruisers.Tutorial.Highlighting;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting
{
    public interface IPvPHighlightArgsFactory
    {
        HighlightArgs CreateForOnCanvasObject(RectTransform rectTransform, float sizeMultiplier);
        HighlightArgs CreateForInGameObject(Vector2 objectWorldPosition, Vector2 objectWorldSize);
    }
}
