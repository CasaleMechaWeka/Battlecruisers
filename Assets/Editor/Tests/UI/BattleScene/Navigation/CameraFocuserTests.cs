using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.BattleScene.Navigation
{
    public class CameraFocuserTests
    {
        private ICameraFocuser _cameraFocuser;
        private IPyramid _navigationPanelArea;
        private INavigationWheel _navigationWheel;

        [SetUp]
        public void TestSetup()
        {
            _navigationPanelArea = Substitute.For<IPyramid>();
            _navigationWheel = Substitute.For<INavigationWheel>();
            _cameraFocuser = new CameraFocuser(_navigationPanelArea, _navigationWheel);

            _navigationPanelArea.BottomLeftVertex.Returns(new Vector2(7, 7));
            _navigationPanelArea.BottomRightVertex.Returns(new Vector2(-3, -3));
        }

        [Test]
        public void FocusOnPlayerCruiser()
        {
            _cameraFocuser.FocusOnPlayerCruiser();
            _navigationWheel.Received().CenterPosition = _navigationPanelArea.BottomLeftVertex;
        }

        [Test]
        public void FocusOnAiCruiser()
        {
            _cameraFocuser.FocusOnAiCruiser();
            _navigationWheel.Received().CenterPosition = _navigationPanelArea.BottomRightVertex;
        }
    }
}