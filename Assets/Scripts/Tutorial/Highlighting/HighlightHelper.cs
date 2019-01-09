using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class HighlightHelper : IHighlightHelper
    {
        private readonly IHighlightFactory _highlightFactory;

        public HighlightHelper(IHighlightFactory highlightFactory)
        {
            Assert.IsNotNull(highlightFactory);
            _highlightFactory = highlightFactory;
        }

        public IHighlight CreateHighlight(IHighlightable highlightable, bool usePulsingAnimation = true)
        {
            float radius = highlightable.Size.x / 2 * highlightable.SizeMultiplier;

            switch (highlightable.HighlightableType)
            {
                case HighlightableType.InGame:
                    Vector2 spawnPosition = (Vector2)highlightable.Transform.Position + highlightable.PositionAdjustment;
                    return _highlightFactory.CreateInGameHighlight(radius, spawnPosition, usePulsingAnimation);

                case HighlightableType.OnCanvas:
                    return _highlightFactory.CreateOnCanvasHighlight(radius, highlightable.Transform.PlatformObject, highlightable.PositionAdjustment);

                default:
                    throw new ArgumentException();
            }
        }
    }
}