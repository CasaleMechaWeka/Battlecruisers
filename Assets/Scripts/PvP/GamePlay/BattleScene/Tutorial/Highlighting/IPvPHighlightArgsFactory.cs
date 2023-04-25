using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting
{
    public interface IPvPHighlightArgsFactory
    {
        PvPHighlightArgs CreateForOnCanvasObject(RectTransform rectTransform, float sizeMultiplier);
        PvPHighlightArgs CreateForInGameObject(Vector2 objectWorldPosition, Vector2 objectWorldSize);
    }
}
