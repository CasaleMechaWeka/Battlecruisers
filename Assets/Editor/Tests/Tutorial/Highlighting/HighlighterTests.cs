using System.Collections.Generic;
using BattleCruisers.Tutorial.Highlighting;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Tutorial.Highlighting
{
    public class HighlighterTests
    {
        private IHighlighter _highlighter;
        private IHighlightFactory _factory;
        private IHighlightable _highlightable;
        private IList<IHighlightable> _highlightables;
        private GameObject _gameObj;
        private IHighlight _highlight;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _factory = Substitute.For<IHighlightFactory>();
            _highlighter = new Highlighter(_factory);
            _highlight = Substitute.For<IHighlight>();

            // Highlightable
            _highlightable = Substitute.For<IHighlightable>();
            Vector2 highlightableSize = new Vector2(12, 17);
            _highlightable.Size.Returns(highlightableSize);
            _highlightable.SizeMultiplier.Returns(1.5f);
            Vector2 positionAdjustment = new Vector2(3, 3);
            _highlightable.PositionAdjustment.Returns(positionAdjustment);
            _gameObj = new GameObject();
            _highlightable.Transform.Returns(_gameObj.transform);

            _highlightables = new List<IHighlightable>()
            {
                _highlightable
            };
        }

        #region Highlight
        [Test]
        public void Highlight_InGameHighlightable()
        {
            _highlightable.HighlightableType.Returns(HighlightableType.InGame);

			float radius = _highlightable.Size.x / 2 * _highlightable.SizeMultiplier;
			Vector2 spawnPosition = (Vector2)_gameObj.transform.position + _highlightable.PositionAdjustment;
			_factory.CreateInGameHighlight(radius, spawnPosition).Returns(_highlight);

            _highlighter.Highlight(_highlightables);

            _factory.Received().CreateInGameHighlight(radius, spawnPosition);
        }

        [Test]
        public void Highlight_OnCanvasHighlightable()
        {
            _highlightable.HighlightableType.Returns(HighlightableType.OnCanvas);

            float radius = _highlightable.Size.x / 2 * _highlightable.SizeMultiplier;
            _factory.CreateOnCanvasHighlight(radius, _gameObj.transform, _highlightable.PositionAdjustment);

            _highlighter.Highlight(_highlightables);

            _factory.Received().CreateOnCanvasHighlight(radius, _gameObj.transform, _highlightable.PositionAdjustment);
        }

        [Test]
        public void Highlight_DoubleHighlightThrows()
        {
            // Valid first highlight
            Highlight_InGameHighlightable();

            // Invalid second highlight (without intervening UnhighlightAll())
            Assert.Throws<UnityAsserts.AssertionException>(() => _highlighter.Highlight(_highlightables));
        }
        #endregion Highlight

        [Test]
        public void UnHighlight()
        {
            Highlight_InGameHighlightable();

            _highlighter.UnhighlightAll();

            _highlight.Received().Destroy();
        }

        [Test]
        public void Highlight_UnHighlight_Highlight()
        {
            Highlight_InGameHighlightable();
            _highlighter.UnhighlightAll();
            Highlight_OnCanvasHighlightable();
        }
    }
}
