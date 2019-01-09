using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Tutorial.Highlighting
{
    public class HighlightHelperTests
    {
        private IHighlightHelper _highlightHelper;
        private IHighlightFactory _factory;
        private IHighlightable _highlightable;
        private GameObject _gameObj;
        private IHighlight _highlight;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _factory = Substitute.For<IHighlightFactory>();
            _highlightHelper = new HighlightHelper(_factory);
            _highlight = Substitute.For<IHighlight>();

            // Highlightable
            _highlightable = Substitute.For<IHighlightable>();
            Vector2 highlightableSize = new Vector2(12, 17);
            _highlightable.Size.Returns(highlightableSize);
            _highlightable.SizeMultiplier.Returns(1.5f);
            Vector2 positionAdjustment = new Vector2(3, 3);
            _highlightable.PositionAdjustment.Returns(positionAdjustment);
            _gameObj = new GameObject();
            ITransform transform = new TransformBC(_gameObj.transform);
            _highlightable.Transform.Returns(transform);
        }

        [Test]
        public void Highlight_InGameHighlightable()
        {
            _highlightable.HighlightableType.Returns(HighlightableType.InGame);

			float radius = _highlightable.Size.x / 2 * _highlightable.SizeMultiplier;
			Vector2 spawnPosition = (Vector2)_gameObj.transform.position + _highlightable.PositionAdjustment;
            bool usePulsingAnimation = true;
            _factory.CreateInGameHighlight(radius, spawnPosition, usePulsingAnimation).Returns(_highlight);

            IHighlight highlight = _highlightHelper.CreateHighlight(_highlightable);

            _factory.Received().CreateInGameHighlight(radius, spawnPosition, usePulsingAnimation);
            Assert.AreSame(_highlight, highlight);
        }

        [Test]
        public void Highlight_OnCanvasHighlightable()
        {
            _highlightable.HighlightableType.Returns(HighlightableType.OnCanvas);

            float radius = _highlightable.Size.x / 2 * _highlightable.SizeMultiplier;
            _factory.CreateOnCanvasHighlight(radius, _gameObj.transform, _highlightable.PositionAdjustment).Returns(_highlight);

            IHighlight highlight = _highlightHelper.CreateHighlight(_highlightable);

            _factory.Received().CreateOnCanvasHighlight(radius, _gameObj.transform, _highlightable.PositionAdjustment);
            Assert.AreSame(_highlight, highlight);
        }
    }
}
