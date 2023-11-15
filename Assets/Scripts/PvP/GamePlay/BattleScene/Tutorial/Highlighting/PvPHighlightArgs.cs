using UnityEngine;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting
{
    public class PvPHighlightArgs
    {
        public Vector2 CenterPosition { get; }
        public Vector2 BottomLeftPosition { get; }
        public Vector2 Size { get; }

        public PvPHighlightArgs(Vector2 centerPosition, Vector2 bottomLeftPosition, Vector2 size)
        {
            CenterPosition = centerPosition;
            BottomLeftPosition = bottomLeftPosition;
            Size = size;
        }
    }
}

