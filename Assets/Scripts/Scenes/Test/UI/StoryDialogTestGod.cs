using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.UI
{
    public class StoryDialogTestGod : TestGodBase
    {
        public TrashScreenController trashScreen;
        public LevelButtonsPanel levelButtonsPanel;

        [Header("Peter can change these :D")]
        [Range(1, 25)]
        public int startingLevelNum = 2;

        protected override void Setup(Utilities.Helper helper)
        {
            Helper.AssertIsNotNull(trashScreen, levelButtonsPanel);


            trashScreen
                .Initialise(
                    Substitute.For<ScreensSceneGod>(),
                    Substitute.For<SingleSoundPlayer>(),
                    Substitute.For<MusicPlayer>());

            levelButtonsPanel
                .Initialise(
                    trashScreen,
                    startingLevelNum);
        }
    }
}