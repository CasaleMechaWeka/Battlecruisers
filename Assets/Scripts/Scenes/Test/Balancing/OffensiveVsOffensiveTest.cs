using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
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

        // FELIX  Remove :)
        //protected override BuildableInitialisationArgs CreateLeftGroupArgs(Helper helper, Vector2 spawnPosition, IUpdaterProvider updaterProvider)
        //{
        //    _targetsOnRight = new ObservableCollection<ITarget>();

        //    return 
        //        new BuildableInitialisationArgs(
        //            helper, 
        //            Faction.Blues, 
        //            parentCruiserDirection: Direction.Right,
        //            targetFactories: helper.CreateTargetFactories(_targetsOnRight));
        //}

        // FELIX  Remove :)
        //protected override BuildableInitialisationArgs CreateRightGroupArgs(Helper helper, Vector2 spawnPosition, IUpdaterProvider updaterProvider)
        //{
        //    _targetsOnLeft = new ObservableCollection<ITarget>();

        //    return
        //        new BuildableInitialisationArgs(
        //            helper,
        //            Faction.Reds,
        //            parentCruiserDirection: Direction.Left,
        //            targetFactories: helper.CreateTargetFactories(_targetsOnLeft));
        //}

        // FELIX  Remove :)
        //protected override void OnInitialised()
        //{
        //    // Add targets, which will reach the global target processor :)
        //    foreach (IBuildable buildableOnLeft in _leftGroup.Buildables)
        //    {
        //        _targetsOnLeft.Add(buildableOnLeft);
        //    }

        //    foreach (IBuildable buildableOnRight in _rightGroup.Buildables)
        //    {
        //        _targetsOnRight.Add(buildableOnRight);
        //    }
        //}
    }
}
