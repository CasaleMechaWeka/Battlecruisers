using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.BattleScene.Navigation
{
    public class NavigationWheelPanelTests
    {
        private INavigationWheelPanel _wheelPanel;
        private IPyramid _panelArea;
        private INavigationWheel _navigationWheel;

        [SetUp]
        public void TestSetup()
        {
            _panelArea = Substitute.For<IPyramid>();
            _navigationWheel = Substitute.For<INavigationWheel>();

            _wheelPanel = new NavigationWheelPanel(_panelArea, _navigationWheel);

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void FindYProportion_NavigationWheelIsTeLeftOfArea_Throws()
        {
            _navigationWheel.CenterPosition.Returns(new Vector2(0, -1));
            _panelArea.BottomLeftVertex.Returns(new Vector2(0, 0));

            Assert.Throws<UnityAsserts.AssertionException>(() => _wheelPanel.FindYProportion());
        }

        [Test]
        public void FindYProportion_NavigationWheelIsTeRightOfArea_Throws()
        {
            _navigationWheel.CenterPosition.Returns(new Vector2(0, 5));
            _panelArea.BottomLeftVertex.Returns(new Vector2(0, 0));
            _panelArea.Height.Returns(4);

            Assert.Throws<UnityAsserts.AssertionException>(() => _wheelPanel.FindYProportion());
        }

        [Test]
        public void FindYProportion_NavigationWheelIsInArea_FindsProportion()
        {
            _navigationWheel.CenterPosition.Returns(new Vector2(0, 3));
            _panelArea.BottomLeftVertex.Returns(new Vector2(0, 0));
            _panelArea.Height.Returns(4);

            float expectedProportion = (_navigationWheel.CenterPosition.y - _panelArea.BottomLeftVertex.y) / _panelArea.Height;

            Assert.IsTrue(Mathf.Approximately(expectedProportion, _wheelPanel.FindYProportion()));
        }

        [Test]
        public void FindXProportion_WheelPositionIsClamped()
        {
            // Wheel position is slightly outside (0.9999f compared to 1) to
            // valid x range at height.  Should result in clamping.
            _navigationWheel.CenterPosition.Returns(new Vector2(0.9999f, 3));
            _panelArea.BottomLeftVertex.Returns(new Vector2(0, 0));
            _panelArea.Height.Returns(4);
            float localYPosition = _navigationWheel.CenterPosition.y - _panelArea.BottomLeftVertex.y;

            IRange<float> globalXRangeAtHeight = new Range<float>(min: 1, max: 2);
            _panelArea.FindGlobalXRange(localYPosition).Returns(globalXRangeAtHeight);

            float expectedProportion = 0;
            Assert.AreEqual(expectedProportion, _wheelPanel.FindXProportion());
        }

        [Test]
        public void FindXProportion_WheelPositionIsValid()
        {
            _navigationWheel.CenterPosition.Returns(new Vector2(1.25f, 3));
            _panelArea.BottomLeftVertex.Returns(new Vector2(0, 0));
            _panelArea.Height.Returns(4);
            float localYPosition = _navigationWheel.CenterPosition.y - _panelArea.BottomLeftVertex.y;

            IRange<float> globalXRangeAtHeight = new Range<float>(min: 1, max: 2);
            _panelArea.FindGlobalXRange(localYPosition).Returns(globalXRangeAtHeight);

            float expectedProportion = (_navigationWheel.CenterPosition.x - globalXRangeAtHeight.Min) / (globalXRangeAtHeight.Max - globalXRangeAtHeight.Min);
            Assert.AreEqual(expectedProportion, _wheelPanel.FindXProportion());
        }
    }
}