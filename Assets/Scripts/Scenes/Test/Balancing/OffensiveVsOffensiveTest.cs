using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing
{
    /// <summary>
    /// Creates custom targets factory for offensive buildings.
    /// </summary>
    public class OffensiveVsOffensiveTest : BuildableVsBuildableTest
    {
        private ObservableCollection<ITarget> _targetsOnLeft, _targetsOnRight;

        protected override BuildableInitialisationArgs CreateLeftGroupArgs(Helper helper, Vector2 spawnPosition)
        {
            _targetsOnRight = new ObservableCollection<ITarget>();

            return 
                new BuildableInitialisationArgs(
                    helper, 
                    Faction.Blues, 
                    parentCruiserDirection: Direction.Right,
                    targetFactories: helper.CreateTargetFactories(_targetsOnRight));
        }

        protected override BuildableInitialisationArgs CreateRightGroupArgs(Helper helper, Vector2 spawnPosition)
        {
            _targetsOnLeft = new ObservableCollection<ITarget>();

            return
                new BuildableInitialisationArgs(
                    helper,
                    Faction.Reds,
                    parentCruiserDirection: Direction.Left,
                    targetFactories: helper.CreateTargetFactories(_targetsOnLeft));
        }

        protected override void OnInitialised()
        {
            // Add targets, which will reach the global target processor :)
            foreach (IBuildable buildableOnLeft in _leftGroup.Buildables)
            {
                _targetsOnLeft.Add(buildableOnLeft);
            }

            foreach (IBuildable buildableOnRight in _rightGroup.Buildables)
            {
                _targetsOnRight.Add(buildableOnRight);
            }
        }
    }
}
