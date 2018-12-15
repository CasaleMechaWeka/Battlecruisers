//using BattleCruisers.Cruisers.Slots;
//using BattleCruisers.Data.Models.PrefabKeys;
//using BattleCruisers.Tutorial.Steps.Providers;

//namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
//{
//    public class EnemyBomberStepsFactory : EnemyUnitDefenceStepsFactory
//    {

//        protected override IPrefabKey FactoryKey
//        {
//            get
//            {
//                throw new System.NotImplementedException();
//            }
//        }

//        protected override BuildableInfo UnitToBuild => throw new System.NotImplementedException();

//        protected override ISingleBuildableProvider UnitBuiltProvider => throw new System.NotImplementedException();

//        protected override BuildableInfo DefenceToBuild => throw new System.NotImplementedException();

//        protected override SlotSpecification SlotSpecification => throw new System.NotImplementedException();

//        protected override CameraFocuserTarget UnitCameraFocusTarget => throw new System.NotImplementedException();

//        public EnemyBomberStepsFactory(
//            ITutorialStepArgsFactory argsFactory, 
//            ITutorialArgs tutorialArgs, 
//            ICreateProducingFactoryStepsFactory createProducingFactoryStepsFactory, 
//            IAutoNavigationStepFactory autoNavigationStepFactory, 
//            IExplanationDismissableStepFactory explanationDismissableStepFactory, 
//            IConstructBuildingStepsFactory constructBuildingStepsFactory, 
//            IChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory, 
//            ITutorialProvider tutorialProvider) 
//            : base(argsFactory, tutorialArgs, createProducingFactoryStepsFactory, autoNavigationStepFactory, explanationDismissableStepFactory, constructBuildingStepsFactory, changeCruiserBuildSpeedStepFactory, tutorialProvider)
//        {
//        }
//    }
//}