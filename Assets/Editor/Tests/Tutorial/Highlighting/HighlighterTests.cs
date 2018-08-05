using BattleCruisers.Tutorial.Highlighting;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Tutorial.Highlighting
{
    public class HighlighterTests
    {
        private IHighlighter _highlighter;
        private IHighlightHelper _helper;
        private IHighlightable _highlightable;
        private IList<IHighlightable> _highlightables;
        private IHighlight _highlight;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _helper = Substitute.For<IHighlightHelper>();
            _highlighter = new Highlighter(_helper);

            _highlight = Substitute.For<IHighlight>();
            _highlightable = Substitute.For<IHighlightable>();
            _helper.CreateHighlight(_highlightable).Returns(_highlight);

            _highlightables = new List<IHighlightable>()
            {
                _highlightable
            };
        }

        #region Highlight
        [Test]
        public void Highlight_CreatesHighlight()
        {
            _highlighter.Highlight(_highlightables);
            _helper.Received().CreateHighlight(_highlightable);
        }

        [Test]
        public void Highlight_DoubleHighlightThrows()
        {
            // Valid first highlight
            _highlighter.Highlight(_highlightables);

            // Invalid second highlight (without intervening UnhighlightAll())
            Assert.Throws<UnityAsserts.AssertionException>(() => _highlighter.Highlight(_highlightables));
        }
        #endregion Highlight

        [Test]
        public void UnHighlight()
        {
            _highlighter.Highlight(_highlightables);

            _highlighter.UnhighlightAll();

            _highlight.Received().Destroy();
        }

        [Test]
        public void Highlight_UnHighlight_Highlight()
        {
            _highlighter.Highlight(_highlightables);
            _highlighter.UnhighlightAll();
            _highlighter.Highlight(_highlightables);
        }
    }
}
