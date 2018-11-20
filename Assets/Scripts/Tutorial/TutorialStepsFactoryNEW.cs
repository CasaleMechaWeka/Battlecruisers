using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Strings;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialStepsFactoryNEW : ITutorialStepsFactory
    {
        private readonly IHighlighterNEW _highlighter;
        private readonly ITextDisplayer _displayer;
        private readonly IVariableDelayDeferrer _deferrer;
        private readonly ITutorialArgsNEW _tutorialArgs;
        private readonly ISingleBuildableProvider _lastPlayerIncompleteBuildingStartedProvider;

        public TutorialStepsFactoryNEW(
            IHighlighterNEW highlighter,
            ITextDisplayer displayer,
            IVariableDelayDeferrer deferrer,
            ITutorialArgsNEW tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, displayer, deferrer, tutorialArgs);

            _highlighter = highlighter;
            _displayer = displayer;
            _deferrer = deferrer;
            _tutorialArgs = tutorialArgs;

            _lastPlayerIncompleteBuildingStartedProvider = _tutorialArgs.TutorialProvider.CreateLastIncompleteBuildingStartedProvider(_tutorialArgs.PlayerCruiser);
        }

        public Queue<ITutorialStep> CreateTutorialSteps()
        {
            Queue<ITutorialStep> steps = new Queue<ITutorialStep>();

            // 0. Wait until initial camera movement is complete
            //steps.Enqueue(CreateStep_NavigationWaitStep(CameraState.PlayerCruiser));

            return steps;
        }
    }
}