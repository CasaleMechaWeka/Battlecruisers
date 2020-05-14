using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Tutorial.Highlighting
{
    public class HighlighterTests
    {
        private IHighlighter _highlighter;
        private IMaskHighlighter _maskHighlighter;
        private IHighlightArgsFactory _highlightArgsFactory;
        private IMaskHighlightable _highlightable;
        private HighlightArgs _highlightArgs;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _maskHighlighter = Substitute.For<IMaskHighlighter>();
            _highlightArgsFactory = Substitute.For<IHighlightArgsFactory>();

            _highlighter = new Highlighter(_maskHighlighter, _highlightArgsFactory);

            // FELIX  Fix :P
            _highlightArgs = new HighlightArgs(new Vector2(9, 9), new Vector2(1, 2), new Vector2(-1, 2));
            _highlightable = Substitute.For<IMaskHighlightable>();
            _highlightable.CreateHighlightArgs(_highlightArgsFactory).Returns(_highlightArgs);
        }

        [Test]
        public void Highlight()
        {
            _highlighter.Highlight(_highlightable);
            _maskHighlighter.Received().Highlight(_highlightArgs);
        }

        [Test]
        public void UnHighlight()
        {
            _highlighter.Highlight(_highlightable);
            _highlighter.Unhighlight();

            _maskHighlighter.Received().Unhighlight();
        }

        [Test]
        public void Highlight_UnHighlight_Highlight()
        {
            _highlighter.Highlight(_highlightable);
            _highlighter.Unhighlight();
            _highlighter.Highlight(_highlightable);
        }
    }
}
