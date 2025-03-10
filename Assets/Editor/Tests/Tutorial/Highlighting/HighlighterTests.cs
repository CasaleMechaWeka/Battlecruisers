using BattleCruisers.Tutorial.Highlighting;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Tutorial.Highlighting
{
    public class HighlighterTests
    {
        private IHighlighter _highlighter;
        private ICoreHighlighter _coreHighlighter;
        private IHighlightArgsFactory _highlightArgsFactory;
        private IHighlightable _highlightable;
        private HighlightArgs _highlightArgs;

        [SetUp]
        public void SetuUp()
        {
            _coreHighlighter = Substitute.For<ICoreHighlighter>();
            _highlightArgsFactory = Substitute.For<IHighlightArgsFactory>();

            _highlighter = new Highlighter(_coreHighlighter, _highlightArgsFactory);

            _highlightArgs = new HighlightArgs(new Vector2(9, 9), new Vector2(1, 2), new Vector2(-1, 2));
            _highlightable = Substitute.For<IHighlightable>();
            _highlightable.CreateHighlightArgs(_highlightArgsFactory).Returns(_highlightArgs);
        }

        [Test]
        public void Highlight()
        {
            _highlighter.Highlight(_highlightable);
            _coreHighlighter.Received().Highlight(_highlightArgs);
        }

        [Test]
        public void UnHighlight()
        {
            _highlighter.Highlight(_highlightable);
            _highlighter.Unhighlight();

            _coreHighlighter.Received().Unhighlight();
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
