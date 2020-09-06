using BattleCruisers.Tutorial.Providers;
using BattleCruisers.UI;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    public class ExplanationClickStep : TutorialStep
    {
        private readonly IItemProvider<IClickableEmitter> _clickableProvider;
        private IClickableEmitter _clickableEmitter;

        private readonly bool _shouldUnhighlight;
        protected override bool ShouldUnhighlight => _shouldUnhighlight;

        public ExplanationClickStep(ITutorialStepArgs args, IClickableEmitter clickable)
            : this(args, new StaticProvider<IClickableEmitter>(clickable))
        {
            // empty
        }

        public ExplanationClickStep(ITutorialStepArgs args, IItemProvider<IClickableEmitter> clickableProvider, bool shouldUnhighlight = true)
            : base(args)
        {
            Assert.IsNotNull(clickableProvider);
            _clickableProvider = clickableProvider;
            _shouldUnhighlight = shouldUnhighlight;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _clickableEmitter = _clickableProvider.FindItem();
            Assert.IsNotNull(_clickableEmitter);
            _clickableEmitter.Clicked += _clickableEmitter_Clicked;
        }

        private void _clickableEmitter_Clicked(object sender, EventArgs e)
        {
            _clickableEmitter.Clicked -= _clickableEmitter_Clicked;
            OnCompleted();
        }
    }
}