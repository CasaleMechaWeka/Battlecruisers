using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting
{
    // FELIX  Test :)
    public class Highlighter : IHighlighter
    {
        private readonly IHighlightFactory _factory;
        private readonly IList<IHighlight> _highlights;

        public Highlighter(IHighlightFactory factory)
        {
            Assert.IsNotNull(factory);

            _factory = factory;
            _highlights = new List<IHighlight>();
        }

        public void Highlight(IList<IHighlightable> toHighlight)
        {
            Assert.IsTrue(_highlights.Count == 0, "Should only highlight group of IHighlightables at a time.");

            foreach (IHighlightable highlightable in toHighlight)
            {
                _highlights.Add(CreateHighlight(highlightable));
            }
        }

        private IHighlight CreateHighlight(IHighlightable highlightable)
        {
            float radius = highlightable.Size.x / 2 * highlightable.SizeMultiplier;

            switch (highlightable.HighlightableType)
            {
                case HighlightableType.InGame:
                    Vector2 spawnPosition = (Vector2)highlightable.Transform.position + highlightable.PositionAdjustment;
                    return _factory.CreateInGameHighlight(radius, spawnPosition);

                case HighlightableType.OnCanvas:
                    return _factory.CreateOnCanvasHighlight(radius, highlightable.Transform, highlightable.PositionAdjustment);
                
                default:
                    throw new ArgumentException();
            }
        }

        public void UnhighlightAll()
        {
            foreach (IHighlight highlight in _highlights)
            {
                highlight.Destroy();
            }

            _highlights.Clear();
        }
    }
}
